using MinesweeperAPI.Model;
using MinesweeperAPI.Model.BoardBuilders;
using System;
using System.Collections.Generic;
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
        public void GetPlayerViewOfInitialBoardIsCoveredAndWithoutAdjancentMinesInfo()
        {
            var board = new Board(3, 3);
            board.TrySetMineOnCell(new BoardCellCoordinate(0, 0));
            board.TrySetMineOnCell(new BoardCellCoordinate(1, 1));
            board.TrySetMineOnCell(new BoardCellCoordinate(2, 2));

            var playerView = board.GetPlayerView();
            for (var i = 0; i < playerView.GetLength(0); i++)
            {
                for (var j = 0; j < playerView.GetLength(1); j++)
                {
                    var cell = playerView[i, j];
                    Assert.Equal(BoardCellState.Covered, cell.State);
                    Assert.Null(cell.AdjacentMinesCount);
                }
            }
        }

        [Fact]
        public void GetPlayerViewRevealsAdjancentMines()
        {
            var board = new Board(3, 3);
            board.TrySetMineOnCell(new BoardCellCoordinate(0, 0));
            board.TrySetMineOnCell(new BoardCellCoordinate(1, 1));

            board.UncoverCell(new BoardCellCoordinate(0, 1));

            var playerView = board.GetPlayerView();
            
            Assert.Equal(BoardCellState.Covered, playerView[0, 0].State);
            Assert.Equal(BoardCellState.Uncovered, playerView[0, 1].State);
            Assert.Equal(2, playerView[0, 1].AdjacentMinesCount);
        }

        [Fact]
        public void GetPlayerViewWithNoMinesRevealsWholeBoard()
        {
            var board = new Board(3, 3);
            board.UncoverCell(new BoardCellCoordinate(0, 0));

            var playerView = board.GetPlayerView();

            for (var i = 0; i < playerView.GetLength(0); i++)
            {
                for (var j = 0; j < playerView.GetLength(1); j++)
                {
                    var cell = playerView[i, j];
                    Assert.Equal(BoardCellState.Uncovered, cell.State);
                }
            }
        }

        [Fact]
        public void GetPlayerViewWithSingleMineRevealsBoard()
        {
            var board = new Board(3, 3);
            board.TrySetMineOnCell(new BoardCellCoordinate(2, 2));

            board.UncoverCell(new BoardCellCoordinate(0, 0));

            var playerView = board.GetPlayerView();

            Assert.Equal(BoardCellState.Uncovered, playerView[0, 0].State);
            Assert.Equal(0, playerView[0, 0].AdjacentMinesCount);
            
            Assert.Equal(BoardCellState.Uncovered, playerView[0, 1].State);
            Assert.Equal(0, playerView[0, 1].AdjacentMinesCount);
            
            Assert.Equal(BoardCellState.Uncovered, playerView[0, 2].State);
            Assert.Equal(0, playerView[0, 2].AdjacentMinesCount);

            Assert.Equal(BoardCellState.Uncovered, playerView[1, 0].State);
            Assert.Equal(0, playerView[1, 0].AdjacentMinesCount);

            Assert.Equal(BoardCellState.Uncovered, playerView[2, 0].State);
            Assert.Equal(0, playerView[2, 0].AdjacentMinesCount);

            Assert.Equal(BoardCellState.Uncovered, playerView[1, 1].State);
            Assert.Equal(1, playerView[1, 1].AdjacentMinesCount);
            
            Assert.Equal(BoardCellState.Uncovered, playerView[1, 2].State);
            Assert.Equal(1, playerView[1, 2].AdjacentMinesCount);

            Assert.Equal(BoardCellState.Uncovered, playerView[2, 1].State);
            Assert.Equal(1, playerView[2, 1].AdjacentMinesCount);

            Assert.Equal(BoardCellState.Covered, playerView[2, 2].State);
        }

        [Fact]
        public void FlagCellChangesStateToFlagged()
        {
            var board = new Board(3, 3);
            board.FlagCell(new BoardCellCoordinate(0, 0));
            
            var playerView = board.GetPlayerView();
            Assert.Equal(BoardCellState.Flagged, playerView[0, 0].State);
        }

        [Fact]
        public void UnFlagCellChangesStateToCovered()
        {
            var board = new Board(3, 3);
            board.FlagCell(new BoardCellCoordinate(0, 0));
            board.UnFlagCell(new BoardCellCoordinate(0, 0));

            var playerView = board.GetPlayerView();
            Assert.Equal(BoardCellState.Covered, playerView[0, 0].State);
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
