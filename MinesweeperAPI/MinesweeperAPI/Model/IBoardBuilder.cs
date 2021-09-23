namespace MinesweeperAPI.Model
{
    public interface IBoardBuilder
    {
        Board Build(int width, int height);
    }
}
