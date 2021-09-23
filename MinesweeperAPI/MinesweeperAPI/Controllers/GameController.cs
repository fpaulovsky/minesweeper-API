using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MinesweeperAPI.Data;
using MinesweeperAPI.Dtos;
using MinesweeperAPI.Model;
using MinesweeperAPI.Model.BoardBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MinesweeperAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;
        private readonly IMapper _mapper;
        private readonly GameContext _dbContext;

        public GameController(ILogger<GameController> logger, IMapper mapper, GameContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        private bool GameExists(int id) => _dbContext.Games.Any(e => e.Id == id);

        [HttpGet]
        public IEnumerable<GameDto> Get()
        {
            return _dbContext.Games.Select(p => _mapper.Map<GameDto>(p));
        }

        [HttpPost]
        public async Task<ActionResult<GameDto>> Post(GameSetupDto setup)
        {
            var board = new RandomBoardBuilder(setup.MinesCount).Build(setup.Width, setup.Height);

            var game = new Game
            {
                Status = GameStatus.Started,
                StartDate = DateTime.UtcNow,
                Board = board,
                BoardJSON = JsonSerializer.Serialize(board)
            };
            
            _dbContext.Games.Add(game);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = game.Id }, _mapper.Map<GameDto>(game));
        }
    }
}
