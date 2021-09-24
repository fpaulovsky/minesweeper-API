using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinesweeperAPI.Model;
using System.Threading.Tasks;

namespace MinesweeperAPI.Data
{
    public class PlayerDataService
    {
        private readonly GameContext _dbContext;

        public PlayerDataService(GameContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Player> GetById(int id)
        {
            return await _dbContext.Players.FindAsync(id);
        }

        public async Task<Player> GetByUserName(string userName)
        {
            return await _dbContext.Players.FirstOrDefaultAsync(p => p.UserName == userName);
        }

        public async Task Create(Player player)
        {
            _dbContext.Players.Add(player);
            await _dbContext.SaveChangesAsync();
        }
    }
}
