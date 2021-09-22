using System;
using System.Collections.Generic;

namespace MinesweeperAPI.Model
{
    public class Board
    {
        public Board(int width, int height, int minesCount)
        {
            if (minesCount > width * height)
            {
                throw new Exception("Cannot build a board with more mines than cells");
            }
            
            Width = width;
            Height = height;
            MinesCount = minesCount;

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
                throw new Exception("Cell position on axis X cannot be less than 0");
            }

            if (x > Width)
            {
                throw new Exception("Cell position on axis X cannot be greater than board width");
            }

            if (y < 0)
            {
                throw new Exception("Cell position on axis Y cannot be less than 0");
            }

            if (y > Height)
            {
                throw new Exception("Cell position on axis Y cannot be greater than board height");
            }

            return _cells[x, y];
        }

        public bool HasMineOnCell(int x, int y)
        {
            return GetCell(x, y).HasMine;
        }

        public bool TrySetMineOnCell(int x, int y)
        {
            var cell = GetCell(x, y);
            if (cell.HasMine)
            {
                // If cell already has a mine we don't do anything...
                return false;
            }

            // Otherwise we mark the cell as it has a mine...
            cell.HasMine = true;
            
            // And update all cells that are adjacent to this cell...
            foreach (var coordinate in GetAdjacentCellsCoordinates(x, y))
            {
                var adjacentCell = GetCell(coordinate);
                adjacentCell.AdjacentMinesCount++;
            }

            return true;
        }

        public List<BoardCellCoordinate> GetAdjacentCellsCoordinates(int x, int y)
        {
            var coords = new List<BoardCellCoordinate>(8);
            for (var i = x - 1; i <= x + 1; i++)
            {
                for (var j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < Width && j >= 0 && j < Height && i != x && j != y)
                    {
                        coords.Add(new BoardCellCoordinate(i, j));
                    }
                }
            }
            return coords;
        }
    }
}
