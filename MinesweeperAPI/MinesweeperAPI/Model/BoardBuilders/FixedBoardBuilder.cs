using System.Collections.Generic;

namespace MinesweeperAPI.Model.BoardBuilders
{
    public class FixedBoardBuilder : IBoardBuilder
    {
        public IEnumerable<BoardCellCoordinate> CoordinatesWithMines { get; set; }
        
        public Board Build(int width, int height)
        {
            var board = new Board(width, height);

            foreach (var coordinateWithMine in CoordinatesWithMines)
            {
                board.TrySetMineOnCell(coordinateWithMine);
            }

            return board;
        }
    }
}
