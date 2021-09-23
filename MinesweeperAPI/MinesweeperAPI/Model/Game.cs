using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinesweeperAPI.Model
{
    public class Game
    {
        public int Id { get; set; }

        public Player Player { get; set; }

        [Required]
        public GameStatus Status { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public string BoardJSON { get; set; }

        [NotMapped]
        public Board Board { get; set; }

        public string Name
        {
            get
            {
                if (Status == GameStatus.PlayerLost)
                {
                    return $"Game lost on {EndDate.Value.ToShortDateString()} at {EndDate.Value.ToShortTimeString()}";
                }

                if (Status == GameStatus.PlayerWon)
                {
                    return $"Game won on {EndDate.Value.ToShortDateString()} at {EndDate.Value.ToShortTimeString()}";
                }

                return $"Ongoing game - started on {StartDate.ToShortDateString()} at {StartDate.ToShortTimeString()}";
            }
        }

        public void UncoverCell(BoardCellCoordinate coordinate)
        {
            CheckGameStatus();

            if (Board.HasMineOnCell(coordinate))
            {
                Status = GameStatus.PlayerLost;
                EndDate = DateTime.UtcNow;
                return;
            }
            
            Board.UncoverCell(coordinate);

            if (!Board.HasCoveredCellsWithoutMines())
            {
                Status = GameStatus.PlayerWon;
                EndDate = DateTime.UtcNow;
            }
        }

        public void FlagCell(BoardCellCoordinate coordinate)
        {
            CheckGameStatus();

            Board.FlagCell(coordinate);
        }

        public void UnFlagCell(BoardCellCoordinate coordinate)
        {
            CheckGameStatus();

            Board.UnFlagCell(coordinate);
        }

        public void CheckGameStatus()
        {
            if (Status != GameStatus.Started)
            {
                throw new Exception("This game is currently over");
            }
        }
    }

    public enum GameStatus
    {
        Started,
        PlayerWon,
        PlayerLost
    }
}
