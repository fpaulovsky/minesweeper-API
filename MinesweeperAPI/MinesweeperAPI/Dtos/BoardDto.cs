using MinesweeperAPI.Model;
using System.Collections.Generic;

namespace MinesweeperAPI.Dtos
{
    public class BoardDto
    {
        public int Width { get; set; }

        public int Height { get; set; }
        
        public List<BoardCellDto> Cells { get; set; }
    }
}
