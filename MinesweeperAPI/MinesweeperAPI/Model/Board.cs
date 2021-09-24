using MinesweeperAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MinesweeperAPI.Model
{
    [Serializable]
    public class Board
    {
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            MinesCount = 0;

            Cells = new List<BoardCell>(width * height);
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    Cells.Add(new BoardCell(x, y));
                }
            }
        }

        public List<BoardCell> Cells { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }
        
        public int MinesCount { get; private set; }

        public BoardCell GetCell(BoardCellCoordinate coordinate)
        {
            return GetCell(coordinate.X, coordinate.Y);
        }

        private BoardCell GetCell(int x, int y)
        {
            if (x < 0)
            {
                throw new Exception("Cell coordinate on axis X cannot be less than 0");
            }

            if (x > Width)
            {
                throw new Exception("Cell coordinate on axis X cannot be greater than board width");
            }

            if (y < 0)
            {
                throw new Exception("Cell coordinate on axis Y cannot be less than 0");
            }

            if (y > Height)
            {
                throw new Exception("Cell coordinate on axis Y cannot be greater than board height");
            }

            return Cells.First(c => c.X == x && c.Y == y);
        }

        public bool HasMineOnCell(BoardCellCoordinate coordinate)
        {
            return GetCell(coordinate).HasMine;
        }

        public bool TrySetMineOnCell(BoardCellCoordinate coordinate)
        {
            var cell = GetCell(coordinate);
            if (cell.HasMine)
            {
                // If cell already has a mine we don't do anything...
                return false;
            }

            // We mark the cell as having a mine...
            cell.HasMine = true;
            MinesCount++;
            
            // And update all cells that are adjacent to this cell...
            foreach (var adjacentCoordinate in GetAdjacentCellsCoordinates(coordinate))
            {
                var adjacentCell = GetCell(adjacentCoordinate);
                adjacentCell.AdjacentMinesCount++;
            }

            return true;
        }

        public void UncoverCell(BoardCellCoordinate coordinate)
        {
            var cell = GetCell(coordinate);
            if (!cell.HasMine && cell.Uncover() && cell.AdjacentMinesCount == 0)
            {
                foreach (var adjacentCellCoordinate in GetAdjacentCellsCoordinates(coordinate))
                {
                    UncoverCell(adjacentCellCoordinate);
                }
            }
        }

        public void FlagCell(BoardCellCoordinate coordinate)
        {
            var cell = GetCell(coordinate);
            cell.Flag();
        }

        public void UnFlagCell(BoardCellCoordinate coordinate)
        {
            var cell = GetCell(coordinate);
            cell.UnFlag();
        }

        public List<BoardCellCoordinate> GetAdjacentCellsCoordinates(BoardCellCoordinate coordinate)
        {
            var coords = new List<BoardCellCoordinate>(8);
            for (var x = coordinate.X - 1; x <= coordinate.X + 1; x++)
            {
                for (var y = coordinate.Y - 1; y <= coordinate.Y + 1; y++)
                {
                    if (0 <= x && x < Width && 0 <= y && y < Height && (x != coordinate.X || y != coordinate.Y))
                    {
                        coords.Add(new BoardCellCoordinate(x, y));
                    }
                }
            }
            return coords;
        }

        public List<BoardCellDto> GetFlaggedOrUncoveredCells()
        {
            var result = new List<BoardCellDto>();
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var cell = GetCell(x, y);
                    if (cell.State != BoardCellState.Covered)
                    {
                        result.Add(new BoardCellDto
                        {
                            X = x,
                            Y = y,
                            State = cell.State,
                            AdjacentMinesCount = cell.AdjacentMinesCount
                        });
                    }
                }
            }
            return result;
        }

        public bool HasCoveredCellsWithoutMines()
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var currentCell = GetCell(x, y);
                    if (!currentCell.HasMine && currentCell.State == BoardCellState.Covered)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as Board);
        }

        public virtual bool Equals(Board other)
        {
            if (other == null) return false;

            return other.Width.Equals(Width) &&
                other.Height.Equals(Height) &&
                other.MinesCount.Equals(MinesCount) &&
                other.Cells.SequenceEqual(Cells);
        }

        public override int GetHashCode()
        {
            return Width.GetHashCode() * 17 +
                Height.GetHashCode() +
                MinesCount.GetHashCode() +
                Cells.GetHashCode();
        }
    }
}
