using System;

namespace MinesweeperAPI.Model
{
    public class BoardCell
    {
        public BoardCell()
        {
            State = BoardCellState.Covered;
        }

        public bool HasMine { get; set; }

        public int AdjacentMinesCount { get; set; }

        public BoardCellState State { get; private set; }

        public void Uncover()
        {
            if (State == BoardCellState.Uncovered)
            {
                throw new Exception("Cannot uncover an already uncovered cell");
            }

            State = BoardCellState.Uncovered;
        }

        public void Flag()
        {
            if (State == BoardCellState.Flagged)
            {
                throw new Exception("Cannot flag an already flagged cell");
            }

            if (State == BoardCellState.Uncovered)
            {
                throw new Exception("Cannot flag an already uncovered cell");
            }

            State = BoardCellState.Flagged;
        }

        public void UnFlag()
        {
            if (State != BoardCellState.Flagged)
            {
                throw new Exception("Cannot unflag a not flagged cell");
            }

            State = BoardCellState.Covered;
        }
    }

    public enum BoardCellState
    {
        Uncovered,
        Covered,
        Flagged
    }
}
