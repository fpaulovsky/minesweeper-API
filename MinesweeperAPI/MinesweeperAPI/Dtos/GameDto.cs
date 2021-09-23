using MinesweeperAPI.Model;

namespace MinesweeperAPI.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public GameStatus Status { get; set; }

        public BoardCellPlayerView[,] Board { get; set; }
    }
}
