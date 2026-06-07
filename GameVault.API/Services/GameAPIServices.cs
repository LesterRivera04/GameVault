using GameVault.API.DTO;
using GameVault.API.Models;
using GameVault.API.Repository;
using System.Text.Json;

namespace GameVault.API.Services
{
    public class GameAPIServices : IGameAPIServices
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IPlatformRepository _platformRepository;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public GameAPIServices(IGameRepository gameRepository,
            IGenreRepository genreRepository,
            IPlatformRepository platformRepository,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _gameRepository = gameRepository;
            _genreRepository = genreRepository;
            _platformRepository = platformRepository;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IEnumerable<GameDto>> GetAllGamesAsync()
        {
            var games = await _gameRepository.GetAllGamesAsync();
            return games.Select(g => new GameDto
            {
                Id = g.Id,
                Name = g.Name,
                ReleaseDate = g.ReleaseDate,
                Rating = g.Rating,
                ImageUrl = g.ImageUrl,
                Genres = g.GameGenres.Select(x => x.Genre.Name).ToList(),
                Platforms = g.GamePlatforms.Select(x => x.Platform.Name).ToList()
            });
        }

        public async Task<GameDto?> GetGameByIdAsync(int id)
        {
            var game = await _gameRepository.GetGameByIdAsync(id);
            if(game == null)
                return null;

            return new GameDto
            {
                Id = game.Id,
                Name = game.Name,
                ReleaseDate = game.ReleaseDate,
                Rating = game.Rating,
                ImageUrl = game.ImageUrl,
                Genres = game.GameGenres.Select(x => x.Genre.Name).ToList(),
                Platforms = game.GamePlatforms.Select(x => x.Platform.Name).ToList()
            };
        }

        public async Task AddGameAsync(CreateGameDto game)
        {
            var nuevoGame = new Game
            {
                Name = game.Name,
                ReleaseDate = game.ReleaseDate,
                Rating = game.Rating,
                ImageUrl = game.ImageUrl,
                CreatedAt = DateTime.Now,
                TimesAdded = 0,
                GameGenres = new List<GameGenre>(),
                GamePlatforms = new List<GamePlatform>()
            };

            // relacionar con géneros
            foreach (var genreId in game.GenreIds)
            {
                nuevoGame.GameGenres.Add(new GameGenre
                {
                    GenreId = genreId
                });
            }

            foreach (var platformId in game.PlatformIds)
            {
                nuevoGame.GamePlatforms.Add(new GamePlatform
                {
                    PlatformId = platformId
                });
            }
            await _gameRepository.AddGameAsync(nuevoGame);
        }

        public async Task UpdateGameAsync(int id, EditGameDto gameDto)
        {
            var existingGame = await _gameRepository.GetGameByIdAsync(id);
            if (existingGame == null)
                return;

            // actualiazar campos básicos
            existingGame.Name = gameDto.Name;
            existingGame.ReleaseDate = gameDto.ReleaseDate;
            existingGame.Rating = gameDto.Rating;
            existingGame.ImageUrl = gameDto.ImageUrl;

            // limpiar relaciones actuales
            existingGame.GameGenres.Clear();
            existingGame.GamePlatforms.Clear();

            // volver a agregar relaciones con géneros
            foreach (var genreId in gameDto.GenreIds)
            {
                existingGame.GameGenres.Add(new GameGenre
                {
                    GenreId = genreId
                });
            }

            // volver a agregar relaciones con plataformas
            foreach (var platformId in gameDto.PlatformIds)
            {
                existingGame.GamePlatforms.Add(new GamePlatform
                {
                    PlatformId = platformId
                });
            }

            await _gameRepository.UpdateGameAsync(existingGame);
        }

        public async Task DeleteGameAsync(int id)
        {
            await _gameRepository.DeleteGameAsync(id);
        }

        // leer TODOS los juegos desde RAWG, vamos con paginacion
        public async Task<object> GetAllGamesFromRawg(
            int page = 1,
            string? search = null,
            int? genreId = null,
            int? platformId = null)
        {
            var apiKey = _configuration["Rawg:ApiKey"];
            var url = $"https://api.rawg.io/api/games?key={apiKey}&page={page}&page_size=20";

            // SEARCH
            if (!string.IsNullOrEmpty(search))
            {
                url += $"&search={Uri.EscapeDataString(search)}";
            }

            // GENRE
            if (genreId.HasValue)
            {
                url += $"&genres={genreId.Value}";
            }

            // PLATFORM
            if (platformId.HasValue)
            {
                url += $"&platforms={platformId.Value}";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<object>(json);

        }

        // leer todos los juegos desde RAWG usando un string search
        public async Task<object> GetGamesFromRawg(string search)
        {
            var apiKey = _configuration["Rawg:ApiKey"];
            var response = await _httpClient.GetAsync(
                $"https://api.rawg.io/api/games?key={apiKey}&search={search}");

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<object>(json);
            
        }

        public async Task<IEnumerable<GameDto>> FilterGamesAsync(string? name, int? genreId, int? platformId)
        {
            var games = await _gameRepository.GetAllGamesAsync();

            // filtro por NOMBRE
            if (!string.IsNullOrEmpty(name))
            {
                games = games.Where(g => g.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            // filtro por GÉNERO
            if (genreId.HasValue)
            {
                games = games.Where(g => g.GameGenres.Any(gg => gg.GenreId == genreId.Value));
            }

            // filtro por CONSOLA/PLATAFORMA
            if (platformId.HasValue)
            {
                games = games.Where(g => g.GamePlatforms.Any(gp => gp.PlatformId == platformId.Value));
            }
            return games.Select(g => new GameDto
            {
                Id = g.Id,
                Name = g.Name,
                ReleaseDate = g.ReleaseDate,
                Rating = g.Rating,
                ImageUrl = g.ImageUrl,
                Genres = g.GameGenres.Select(x => x.Genre.Name).ToList(),
                Platforms = g.GamePlatforms.Select(x => x.Platform.Name).ToList()
            });
        }

        public async Task AddFavoriteAsync(FavoriteDto dto)
        {
            // validar que el juego existe
            var game = await _gameRepository.GetGameByIdAsync(dto.GameId);
            if (game == null)
            {
                throw new Exception("El juego no existe");
            }

            // verificar si el usuario ya agregó ese juego a favoritos
            var exists = await _gameRepository.ExistsUserGameAsync(dto.UserId, dto.GameId);

            // crear relación UserGame
            var userGame = new UserGame
            {
                UserId = dto.UserId,
                GameId = dto.GameId,
                IsFavorite = dto.IsFavorite,
                AddedAt = DateTime.Now
            };

            // guardar relacion en la base de datos
            await _gameRepository.AddUserGameAsync(userGame);

            // incrementar contador de veces agregado
            game.TimesAdded++;

            await _gameRepository.UpdateGameAsync(game);
        }
    }
}
