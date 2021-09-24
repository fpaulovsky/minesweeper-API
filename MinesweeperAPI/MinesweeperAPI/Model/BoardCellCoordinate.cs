namespace MinesweeperAPI.Model
{
    public struct BoardCellCoordinate
    {
        public readonly int X;
        public readonly int Y;

        public BoardCellCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
