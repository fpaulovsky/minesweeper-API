namespace MinesweeperAPI.Model
{
    public struct BoardCellCoordinate
    {
        public readonly int X;
        public readonly int Y;

        public BoardCellCoordinate(int x, int y)
        {
            X = x < 0 ? 0 : x;
            Y = y < 0 ? 0 : y;
        }
    }
}
