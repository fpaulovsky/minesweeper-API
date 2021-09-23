using System;
using System.Collections.Generic;

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

            _cells = new BoardCell[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    _cells[x, y] = new BoardCell();
                }
            }
        }

        private BoardCell[,] _cells;

        public int Width { get; private set; }

        public int Height { get; private set; }
        
        public int MinesCount { get; private set; }

        private BoardCell GetCell(BoardCellCoordinate coordinate)
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

            return _cells[x, y];
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

        public List<BoardCellCoordinate> GetAdjacentCellsCoordinates(BoardCellCoordinate coordinate)
        {
            var coords = new List<BoardCellCoordinate>(8);
            for (var i = coordinate.X - 1; i <= coordinate.X + 1; i++)
            {
                for (var j = coordinate.Y - 1; j <= coordinate.Y + 1; j++)
                {
                    if (0 <= i && i < Width && 0 <= j && j < Height && (i != coordinate.X || j != coordinate.Y))
                    {
                        coords.Add(new BoardCellCoordinate(i, j));
                    }
                }
            }
            return coords;
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

        public BoardCellPlayerView[,] GetPlayerView()
        {
            var boardPlayerView = new BoardCellPlayerView[Width, Height];
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    var currentCell = GetCell(i, j);
                    boardPlayerView[i, j] = new BoardCellPlayerView
                    {
                        State = currentCell.State,
                        AdjacentMinesCount = currentCell.State == BoardCellState.Uncovered ? currentCell.AdjacentMinesCount : null
                    };
                }
            }
            return boardPlayerView;
        }

        public bool HasCoveredCellsWithoutMines()
        {
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    var currentCell = GetCell(i, j);
                    if (!currentCell.HasMine && currentCell.State == BoardCellState.Covered)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
