using System;

namespace MinesweeperAPI.Model
{
    public class Game
    {
        public int Id { get; set; }

        public Board Board { get; set; }

        public Player Player { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
