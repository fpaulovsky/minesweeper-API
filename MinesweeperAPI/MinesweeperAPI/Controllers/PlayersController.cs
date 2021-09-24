using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MinesweeperAPI.Data;
using MinesweeperAPI.Dtos;
using MinesweeperAPI.Model;
using System.Threading.Tasks;

namespace MinesweeperAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> _logger;
        private readonly IMapper _mapper;
        private readonly GameContext _dbContext;
        private readonly IJwtUtils _jwtUtils;

        private readonly PlayerDataService _playerDataService;

        public PlayersController(ILogger<PlayersController> logger, IMapper mapper, GameContext dbContext, IJwtUtils jwtUtils)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
            _jwtUtils = jwtUtils;

            _playerDataService = new PlayerDataService(_dbContext);
        }

        [HttpPost]
        public async Task<ActionResult<PlayerDto>> Post(NewPlayerDto newPlayerDto)
        {
            if (string.IsNullOrEmpty(newPlayerDto.UserName))
            {
                return BadRequest("Username cannot be empty");
            }

            if (string.IsNullOrEmpty(newPlayerDto.Password))
            {
                return BadRequest("Password cannot be empty");
            }

            var existingPlayer = await _playerDataService.GetByUserName(newPlayerDto.UserName);
            if  (existingPlayer != null)
            {
                return BadRequest("Specified username is already taken");
            }

            var player = new Player
            {
                UserName = newPlayerDto.UserName,
                Password = new PasswordHasher().Hash(newPlayerDto.Password)
            };

            await _playerDataService.Create(player);

            return _mapper.Map<PlayerDto>(player);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<PlayerDto>> Login(NewPlayerDto newPlayerDto)
        {
            if (string.IsNullOrEmpty(newPlayerDto.UserName))
            {
                return BadRequest("Username cannot be empty");
            }

            if (string.IsNullOrEmpty(newPlayerDto.Password))
            {
                return BadRequest("Password cannot be empty");
            }

            var existingPlayer = await _playerDataService.GetByUserName(newPlayerDto.UserName);
            if (existingPlayer == null) return NotFound();

            var passwordHasher = new PasswordHasher();
            var checkResult = passwordHasher.Check(existingPlayer.Password, newPlayerDto.Password);
            if (checkResult.Verified)
            {
                return Ok(_jwtUtils.GenerateToken(existingPlayer));
            }

            return Unauthorized();
        }
    }
}
