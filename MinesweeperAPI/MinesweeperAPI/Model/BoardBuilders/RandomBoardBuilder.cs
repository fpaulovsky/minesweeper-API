using System;

namespace MinesweeperAPI.Model.BoardBuilders
{
    public class RandomBoardBuilder : IBoardBuilder
    {
        public RandomBoardBuilder(int minesCount)
        {
            MinesCount = minesCount;
        }

        public int MinesCount { get; set; }
        
        public Board Build(int width, int height)
        {
            var board = new Board(width, height);
            
            var random = new Random();
            var addedMines = 0;

            while (addedMines < MinesCount)
            {
                var mineAdded = false;
                var tries = 3;

                var x = 0;
                var y = 0;

                // We try 3 times to add cells in a random way...
                while (!mineAdded && tries > 0)
                {
                    x = random.Next(width);
                    y = random.Next(height);

                    if (board.TrySetMineOnCell(new BoardCellCoordinate(x, y)))
                    {
                        mineAdded = true;
                        addedMines++;
                    }
                    else
                    {
                        tries--;
                    }
                }
                
                if (!mineAdded)
                {
                    // If after 3 times we cannot add mines randomly we try setting it in any adjacent cell...
                    if (!TrySetMineOnAdjacentCell(x, y, board))
                    {
                        // As we could not set a mine on an adjacent cell,
                        // We transverse the whole board and set a mine in the first cell that does not have a mine...
                        SetMineOnFirstEmptyCell(board);
                    }

                    addedMines++;
                }
            }

            return board;
        }

        private static bool TrySetMineOnAdjacentCell(int x, int y, Board board)
        {
            foreach (var coordinate in board.GetAdjacentCellsCoordinates(new BoardCellCoordinate(x, y)))
            {
                if (board.TrySetMineOnCell(coordinate))
                {
                    return true;
                }
            }

            return false;
        }

        private static void SetMineOnFirstEmptyCell(Board board)
        {
            for (var i = 0; i < board.Width; i++)
            {
                for (var j = 0; j < board.Height; j++)
                {
                    if (board.TrySetMineOnCell(new BoardCellCoordinate(i, j))) return;
                }
            }
        }
    }
}
