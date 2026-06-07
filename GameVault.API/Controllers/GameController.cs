using GameVault.API.DTO;
using GameVault.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly IGameAPIServices _gameAPIServices;
        public GameController(IGameAPIServices gameAPIServices)
        {
            _gameAPIServices = gameAPIServices;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            var games = await _gameAPIServices.GetAllGamesAsync();
            return Ok(games);
        }

        // GET by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            var game = await _gameAPIServices.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> CreateGame(CreateGameDto game)
        {
            await _gameAPIServices.AddGameAsync(game);
            return Ok();
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, EditGameDto game)
        {
            var existingGame = await _gameAPIServices.GetGameByIdAsync(id);
            if (existingGame == null)
            {
                return NotFound();
            }
            await _gameAPIServices.UpdateGameAsync(id, game);
            return Ok();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var existingGame = await _gameAPIServices.GetGameByIdAsync(id);
            if (existingGame == null)
            {
                return NotFound();
            }
            await _gameAPIServices.DeleteGameAsync(id);
            return Ok();
        }

        // filtros
        [HttpGet("filter")]
        public async Task<IActionResult> FilterGames([FromQuery] string? name, [FromQuery] int? genreId, [FromQuery] int? platformId)
        {
            var results = await _gameAPIServices.FilterGamesAsync(name, genreId, platformId);
            return Ok(results);
        }

        // favoritos
        [HttpPost("favorite")]
        public async Task<IActionResult> AddFavority(FavoriteDto dto)
        {
            await _gameAPIServices.AddFavoriteAsync(dto);
            return Ok();
        }

        // leer TODOS los juegos RAWG
        [HttpGet("all_games")]
        public async Task<IActionResult> GetAllExternalGames(
            int page = 1,
            string? search = null,
            int? genreId = null,
            int? platformId = null)
        {
            var result = await _gameAPIServices.GetAllGamesFromRawg(page, search, genreId, platformId);
            return Ok(result);
        }

        // leer juegos RAWG con un string search
        [HttpGet("external")]
        public async Task<IActionResult> GetExternalGames(string search)
        {
            var result = await _gameAPIServices.GetGamesFromRawg(search);
            return Ok(result);
        }
    }
}
