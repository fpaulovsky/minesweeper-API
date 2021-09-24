using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MinesweeperAPI.Data;
using MinesweeperAPI.Dtos;
using MinesweeperAPI.Model;
using MinesweeperAPI.Model.BoardBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;
        private readonly IMapper _mapper;
        private readonly GameContext _dbContext;

        private readonly GameDataService _gameDataService;
        private readonly PlayerDataService _playerDataService;

        public GamesController(ILogger<GamesController> logger, IMapper mapper, GameContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;

            _gameDataService = new GameDataService(mapper, dbContext);
            _playerDataService = new PlayerDataService(dbContext);
        }

        [HttpGet]
        public IEnumerable<GameDto> Get([FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            if (pageSize <= 0) pageSize = 20;
            if (pageNumber <= 0) pageNumber = 1;

            var playerId = (int) HttpContext.Items["PlayerId"];
            
            return _gameDataService.GetAll(playerId, pageSize, pageNumber).Select(g => _mapper.Map<GameDto>(g));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> Get(int id)
        {
            var playerId = (int)HttpContext.Items["PlayerId"];
            var game = await _gameDataService.GetById(id, playerId);
            if (game == null) return NotFound();
            return _mapper.Map<GameDto>(game);
        }

        [HttpPost]
        public async Task<ActionResult<GameDto>> Post(NewGameDto setup)
        {
            var board = new RandomBoardBuilder(setup.MinesCount).Build(setup.Width, setup.Height);

            var playerId = (int)HttpContext.Items["PlayerId"];
            var player = await _playerDataService.GetById(playerId);

            var game = new Game
            {
                State = GameState.Started,
                StartDate = DateTime.UtcNow,
                Board = board,
                Player = player
            };

            await _gameDataService.Create(game);

            return CreatedAtAction(nameof(Get), new { id = game.Id }, _mapper.Map<GameDto>(game));
        }

        [HttpPatch("{id}/UncoverCell")]
        public async Task<ActionResult<GameDto>> UncoverCell(int id, [FromQuery] int x, [FromQuery] int y)
        {
            return await ExecuteActionOnGame(id, g => g.UncoverCell(new BoardCellCoordinate(x, y)));
        }

        [HttpPatch("{id}/FlagCell")]
        public async Task<ActionResult<GameDto>> FlagCell(int id, [FromQuery] int x, [FromQuery] int y)
        {
            return await ExecuteActionOnGame(id, g => g.FlagCell(new BoardCellCoordinate(x, y)));
        }

        [HttpPatch("{id}/UnFlagCell")]
        public async Task<ActionResult<GameDto>> UnFlagCell(int id, [FromQuery] int x, [FromQuery] int y)
        {
            return await ExecuteActionOnGame(id, g => g.UnFlagCell(new BoardCellCoordinate(x, y)));
        }

        private async Task<ActionResult<GameDto>> ExecuteActionOnGame(int id, Action<Game> action)
        {
            var playerId = (int)HttpContext.Items["PlayerId"];

            var game = await _gameDataService.GetById(id, playerId);
            if (game == null) return NotFound();

            action(game);

            await _gameDataService.Update(game);
            return _mapper.Map<GameDto>(game);
        }
    }
}
