using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinesweeperAPI.Dtos;
using MinesweeperAPI.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperAPI.Data
{
    public class GameDataService
    {
        private readonly IMapper _mapper;
        private readonly GameContext _dbContext;

        public GameDataService(IMapper mapper, GameContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public IEnumerable<Game> GetAll(int playerId, int pageSize, int pageNumber)
        {
            return _dbContext
                .Games
                .Where(g => g.Player.Id == playerId)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();
        }
        
        public async Task<Game> GetById(int id, int playerId)
        {
            var game = await _dbContext.Games.Include(g => g.Player).Where(g => g.Id == id).FirstOrDefaultAsync();
            if (game != null)
            {
                if (game.Player.Id != playerId) return null;
                game.Board = new BoardJsonSerializer(_mapper).Deserialize(game.BoardJSON);
            }            
            return game;
        }

        public async Task Create(Game game)
        {
            game.BoardJSON = new BoardJsonSerializer(_mapper).Serialize(game.Board);

            _dbContext.Games.Add(game);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Game game)
        {
            game.BoardJSON = new BoardJsonSerializer(_mapper).Serialize(game.Board);

            _dbContext.Entry(game).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
