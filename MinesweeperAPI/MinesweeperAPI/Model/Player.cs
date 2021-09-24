using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MinesweeperAPI.Model
{
    public class Player
    {
        public int Id { get; set; }

        
        [Required]
        [MaxLength(255)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Password { get; set; }

        public ICollection<Game> Games { get; set; }
    }
}
