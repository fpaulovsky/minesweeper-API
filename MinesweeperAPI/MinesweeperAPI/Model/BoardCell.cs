using System;

namespace MinesweeperAPI.Model
{
    [Serializable]
    public class BoardCell
    {
        public BoardCell()
        {
            State = BoardCellState.Covered;
            AdjacentMinesCount = 0;
        }

        public bool HasMine { get; set; }

        public int AdjacentMinesCount { get; set; }

        public BoardCellState State { get; private set; }

        public bool Uncover()
        {
            if (State == BoardCellState.Uncovered) return false;
            State = BoardCellState.Uncovered;
            return true;
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
        Covered,
        Flagged,
        Uncovered
    }
}
