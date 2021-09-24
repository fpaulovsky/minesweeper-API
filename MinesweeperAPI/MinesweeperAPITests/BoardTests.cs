using MinesweeperAPI.Model;
using MinesweeperAPI.Model.BoardBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MinesweeperAPITests
{
    public class BoardTests
    {
        [Fact]
        public void GetBoardDimensionsAfterBuildingBoard()
        {
            var board = new Board(15, 27);
            Assert.Equal(15, board.Width);
            Assert.Equal(27, board.Height);
        }

        [Fact]
        public void TrySetMineOnCellWithInvalidCoordinateThrowsException()
        {
            var board = new Board(2, 2);
            Assert.Throws<Exception>(() => board.TrySetMineOnCell(new BoardCellCoordinate(-1, 3)));
        }

        [Fact]
        public void TrySetMineOnAlreadySetMineReturnsFalse()
        {
            var board = new Board(2, 2);
            var mineSet = board.TrySetMineOnCell(new BoardCellCoordinate(0, 0));
            Assert.True(mineSet);
            mineSet = board.TrySetMineOnCell(new BoardCellCoordinate(0, 0));
            Assert.False(mineSet);
        }

        [Fact]
        public void HasMineOnCellAfterTrySetMineOnCell()
        {
            var board = new Board(9, 5);
            var mineSet = board.TrySetMineOnCell(new BoardCellCoordinate(0, 1));
            Assert.True(mineSet);
            Assert.True(board.HasMineOnCell(new BoardCellCoordinate(0, 1)));
        }

        [Fact]
        public void CheckMinesCountAfterAddingSeveralMines()
        {
            var minesCoordinates = new List<BoardCellCoordinate>
            {
                new BoardCellCoordinate(0, 0),
                new BoardCellCoordinate(1, 2),
                new BoardCellCoordinate(0, 6),
                new BoardCellCoordinate(1, 3)
            };

            var board = new FixedBoardBuilder { CoordinatesWithMines = minesCoordinates }.Build(2, 8);

            Assert.Equal(4, board.MinesCount);
        }

        [Theory]
        [ClassData(typeof(AdjacentCellCoordinatesData))]
        public void GetAdjacentCellsCoordinatesTheory(Board board, int x, int y, List<BoardCellCoordinate> expectedCoordinates)
        {
            var result = board.GetAdjacentCellsCoordinates(new BoardCellCoordinate(x, y));
            Assert.Equal(expectedCoordinates, result);
        }

        [Fact]
        public void GetFlaggedOrUncoveredCellsOfInitialBoardIsEmpty()
        {
            var board = new Board(3, 3);
            board.TrySetMineOnCell(new BoardCellCoordinate(0, 0));
            board.TrySetMineOnCell(new BoardCellCoordinate(1, 1));
            board.TrySetMineOnCell(new BoardCellCoordinate(2, 2));

            var flaggedOrUncoveredCells = board.GetFlaggedOrUncoveredCells();
            Assert.Empty(flaggedOrUncoveredCells);
        }

        [Fact]
        public void GetFlaggedOrUncoveredCellsRevealsAdjancentMines()
        {
            var board = new Board(3, 3);
            board.TrySetMineOnCell(new BoardCellCoordinate(0, 0));
            board.TrySetMineOnCell(new BoardCellCoordinate(1, 1));

            board.UncoverCell(new BoardCellCoordinate(0, 1));

            var flaggedOrUncoveredCells = board.GetFlaggedOrUncoveredCells();

            Assert.Single(flaggedOrUncoveredCells);
            Assert.Equal(BoardCellState.Uncovered, flaggedOrUncoveredCells[0].State);
            Assert.Equal(2, flaggedOrUncoveredCells[0].AdjacentMinesCount);
        }

        [Fact]
        public void GetFlaggedOrUncoveredCellsWithNoMinesRevealsWholeBoard()
        {
            var board = new Board(3, 3);
            board.UncoverCell(new BoardCellCoordinate(0, 0));

            var flaggedOrUncoveredCells = board.GetFlaggedOrUncoveredCells();
            Assert.Equal(9, flaggedOrUncoveredCells.Count);
        }

        [Fact]
        public void GetFlaggedOrUncoveredCellsWithSingleMineRevealsBoard()
        {
            var board = new Board(3, 3);
            board.TrySetMineOnCell(new BoardCellCoordinate(2, 2));

            board.UncoverCell(new BoardCellCoordinate(0, 0));

            var flaggedOrUncoveredCells = board.GetFlaggedOrUncoveredCells();
            
            var cell00 = flaggedOrUncoveredCells.First(c => c.X == 0 && c.Y == 0);
            Assert.Equal(BoardCellState.Uncovered, cell00.State);
            Assert.Equal(0, cell00.AdjacentMinesCount);

            var cell01 = flaggedOrUncoveredCells.First(c => c.X == 0 && c.Y == 1);
            Assert.Equal(BoardCellState.Uncovered, cell01.State);
            Assert.Equal(0, cell01.AdjacentMinesCount);

            var cell02 = flaggedOrUncoveredCells.First(c => c.X == 0 && c.Y == 2);
            Assert.Equal(BoardCellState.Uncovered, cell02.State);
            Assert.Equal(0, cell02.AdjacentMinesCount);

            var cell10 = flaggedOrUncoveredCells.First(c => c.X == 1 && c.Y == 0);
            Assert.Equal(BoardCellState.Uncovered, cell10.State);
            Assert.Equal(0, cell10.AdjacentMinesCount);

            var cell20 = flaggedOrUncoveredCells.First(c => c.X == 2 && c.Y == 0);
            Assert.Equal(BoardCellState.Uncovered, cell20.State);
            Assert.Equal(0, cell20.AdjacentMinesCount);

            var cell11 = flaggedOrUncoveredCells.First(c => c.X == 1 && c.Y == 1);
            Assert.Equal(BoardCellState.Uncovered, cell11.State);
            Assert.Equal(1, cell11.AdjacentMinesCount);

            var cell12 = flaggedOrUncoveredCells.First(c => c.X == 1 && c.Y == 2);
            Assert.Equal(BoardCellState.Uncovered, cell12.State);
            Assert.Equal(1, cell12.AdjacentMinesCount);

            var cell21 = flaggedOrUncoveredCells.First(c => c.X == 2 && c.Y == 1);
            Assert.Equal(BoardCellState.Uncovered, cell21.State);
            Assert.Equal(1, cell21.AdjacentMinesCount);
        }

        [Fact]
        public void FlagCellChangesStateToFlagged()
        {
            var board = new Board(3, 3);
            board.FlagCell(new BoardCellCoordinate(0, 0));
            
            var flaggedOrUncoveredCells = board.GetFlaggedOrUncoveredCells();
            var cell00 = flaggedOrUncoveredCells.First(c => c.X == 0 && c.Y == 0);
            Assert.Equal(BoardCellState.Flagged, cell00.State);
        }

        [Fact]
        public void UnFlagCellChangesStateToCovered()
        {
            var board = new Board(3, 3);
            board.FlagCell(new BoardCellCoordinate(0, 0));
            board.UnFlagCell(new BoardCellCoordinate(0, 0));

            var flaggedOrUncoveredCells = board.GetFlaggedOrUncoveredCells();
            Assert.Empty(flaggedOrUncoveredCells);
        }

        [Fact]
        public void CannotFlagUncoveredCell()
        {
            var board = new Board(3, 3);
            board.UncoverCell(new BoardCellCoordinate(0, 0));
            Assert.Throws<Exception>(() => board.FlagCell(new BoardCellCoordinate(0, 0)));
        }

        [Fact]
        public void CannotUnFlagUnFlaggedCell()
        {
            var board = new Board(3, 3);
            Assert.Throws<Exception>(() => board.UnFlagCell(new BoardCellCoordinate(0, 0)));
        }

        [Fact]
        public void RandomBoardBuilderBuildsAValidBoard()
        {
            var width = 10;
            var height = 12;
            
            var builder = new RandomBoardBuilder(40);
            var board = builder.Build(width, height);

            var minesCount = 0;
            
            for (var i = 0; i < width; i++) 
            {
                for (var j = 0; j < height; j++)
                {
                    if (board.HasMineOnCell(new BoardCellCoordinate(i, j)))
                    {
                        minesCount++;
                    }
                }
            }
        }

        [Fact]
        public void DoesNotHaveUncoveredCellsWithoutMinesAfterUncoverACell()
        {
            var board = new Board(2, 2);

            board.TrySetMineOnCell(new BoardCellCoordinate(0, 0));
            Assert.True(board.HasCoveredCellsWithoutMines());

            board.UncoverCell(new BoardCellCoordinate(0, 1));
            board.UncoverCell(new BoardCellCoordinate(1, 0));
            board.UncoverCell(new BoardCellCoordinate(1, 1));
            Assert.False(board.HasCoveredCellsWithoutMines());
        }
    }
}
