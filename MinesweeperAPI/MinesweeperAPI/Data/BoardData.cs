using System.Collections.Generic;

namespace MinesweeperAPI.Data
{
    public class BoardData
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public List<BoardCellData> Cells { get; set; }
    }
}
