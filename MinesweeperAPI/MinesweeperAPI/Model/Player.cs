using System.ComponentModel.DataAnnotations;

namespace MinesweeperAPI.Model
{
    public class Player
    {
        public int Id { get; set; }

        
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
