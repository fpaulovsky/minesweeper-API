using MinesweeperAPI.Model;
using System.Collections;
using System.Collections.Generic;

namespace MinesweeperAPITests
{
    public class AdjacentCellCoordinatesData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var board = new Board(3, 3);
            
            yield return new object[] { 
                board, 
                0, 
                0, 
                new List<BoardCellCoordinate> { new BoardCellCoordinate(0, 1), new BoardCellCoordinate(1, 0), new BoardCellCoordinate(1, 1) } 
            };
            
            yield return new object[] { 
                board, 
                2, 
                2, 
                new List<BoardCellCoordinate> { new BoardCellCoordinate(1, 1), new BoardCellCoordinate(1, 2), new BoardCellCoordinate(2, 1) } 
            };

            var allCoordsExceptCenter = new List<BoardCellCoordinate>(9);
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if (i != 1 || j != 1)
                    {
                        allCoordsExceptCenter.Add(new BoardCellCoordinate(i, j));
                    }
                }
            }

            yield return new object[] {
                board,
                1,
                1,
                allCoordsExceptCenter
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
