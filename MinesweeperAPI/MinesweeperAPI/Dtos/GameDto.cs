using System;

namespace MinesweeperAPI.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string State { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public BoardDto Board { get; set; }
    }
}
