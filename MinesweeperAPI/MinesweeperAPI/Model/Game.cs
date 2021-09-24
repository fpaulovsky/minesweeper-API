using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinesweeperAPI.Model
{
    public class Game
    {
        public int Id { get; set; }

        [Required]
        public Player Player { get; set; }

        [Required]
        public string State { get; set; }

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
                if (State == GameState.PlayerLost)
                {
                    return $"Game lost on {EndDate.Value.ToShortDateString()} at {EndDate.Value.ToShortTimeString()}";
                }

                if (State == GameState.PlayerWon)
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
                State = GameState.PlayerLost;
                EndDate = DateTime.UtcNow;
                return;
            }
            
            Board.UncoverCell(coordinate);

            if (!Board.HasCoveredCellsWithoutMines())
            {
                State = GameState.PlayerWon;
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
            if (State != GameState.Started)
            {
                throw new Exception("This game is currently over");
            }
        }
    }

    public class GameState
    {
        public const string Started = "started";
        public const string PlayerWon = "player-won";
        public const string PlayerLost = "player-lost";
    }
}
