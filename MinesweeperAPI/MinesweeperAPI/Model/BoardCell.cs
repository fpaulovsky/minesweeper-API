using System;

namespace MinesweeperAPI.Model
{
    [Serializable]
    public class BoardCell
    {
        public BoardCell(int x, int y)
        {
            X = x;
            Y = y;
            State = BoardCellState.Covered;
            AdjacentMinesCount = 0;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public bool HasMine { get; set; }

        public int AdjacentMinesCount { get; set; }

        public string State { get; set; }

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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as BoardCell);
        }

        public virtual bool Equals(BoardCell other)
        {
            if (other == null) return false;

            return other.X.Equals(X) &&
                other.Y.Equals(Y) &&
                other.State.Equals(State) &&
                other.AdjacentMinesCount.Equals(AdjacentMinesCount);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() * 17 +
                Y.GetHashCode() +
                State.GetHashCode() +
                AdjacentMinesCount.GetHashCode();
        }
    }

    public class BoardCellState
    {
        public const string Covered = "covered";
        public const string Flagged = "flagged";
        public const string Uncovered = "uncovered";
    }
}
