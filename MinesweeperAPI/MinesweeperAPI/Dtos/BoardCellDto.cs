using MinesweeperAPI.Model;

namespace MinesweeperAPI.Dtos
{
    public class BoardCellDto
    {
        public int X { get; set; }

        public int Y { get; set; }
        
        public string State { get; set; }

        public int AdjacentMinesCount { get; set; }
    }
}
