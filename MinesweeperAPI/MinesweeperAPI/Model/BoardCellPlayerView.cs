namespace MinesweeperAPI.Model
{
    public class BoardCellPlayerView
    {
        public BoardCellState State { get; set; }

        public int? AdjacentMinesCount { get; set; }
    }
}
