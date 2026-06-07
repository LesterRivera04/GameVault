using GameVault.Models;
using System.Text.Json;

namespace GameVault.Services
{
    public class GameService : IGameService
    {
        private readonly HttpClient _httpClient;
        public GameService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GamesViewModel> GetAllGamesAsync(
            int page,
            string? search = null,
            int? genreId = null,
            int? platformId = null)
        {
            var url = $"api/Game/all_games?page={page}";

            if (!string.IsNullOrEmpty(search))
            {
                url += $"&search={Uri.EscapeDataString(search)}";
            }

            if (genreId.HasValue)
            {
                url += $"&genreId={genreId.Value}";
            }

            if (platformId.HasValue)
            {
                url += $"&platformId={platformId.Value}";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(json);
            var results = doc.RootElement.GetProperty("results");
            var count = doc.RootElement.GetProperty("count").GetInt32();
            var totalPages = (int)Math.Ceiling(count / 20.0);
                var totalResults = count;
            var games = new List<GameViewModel>();

            foreach (var game in results.EnumerateArray())
            {
                var name = game.GetProperty("name").GetString();

                Console.WriteLine($"GAME: {name}");

                Console.WriteLine($"Genres: {game.GetProperty("genres").ValueKind}");
                Console.WriteLine($"Platforms: {game.GetProperty("platforms").ValueKind}");

                games.Add(new GameViewModel
                {
                    //Id = game.GetProperty("id").GetInt32(),
                    Name = game.GetProperty("name").GetString() ?? string.Empty,
                    Rating = game.GetProperty("rating").GetDouble(),
                    ImageUrl = game.GetProperty("background_image").GetString() ?? string.Empty,
                    ReleaseDate = DateTime.TryParse(
                        game.GetProperty("released").GetString() ?? string.Empty,
                        out var releaseDate) ? releaseDate : DateTime.MinValue,
                    Genres = game.GetProperty("genres").ValueKind == JsonValueKind.Array
                        ? game.GetProperty("genres")
                        .EnumerateArray()
                        .Select(g => g.GetProperty("name").GetString() ?? string.Empty)
                        .ToList()
                        : new List<string>(),
                    Platforms = game.GetProperty("platforms").ValueKind == JsonValueKind.Array
                        ? game.GetProperty("platforms")
                        .EnumerateArray()
                        .Select(p => p.GetProperty("platform").GetProperty("name").GetString() ?? string.Empty)
                        .ToList()
                        : new List<string>()
                });
            }
            return new GamesViewModel
            {
                Games = games,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalResults = count
            };
        }

        public async Task<IEnumerable<GameViewModel>> GetAllSearchGamesAsync(string search)
        {
            var response = await _httpClient.GetAsync($"api/Game/external?search={search}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(json);
            var results = doc.RootElement.GetProperty("results");
            var games = new List<GameViewModel>();

            foreach (var game in results.EnumerateArray())
            {
                games.Add(new GameViewModel
                {
                    //Id = game.GetProperty("id").GetInt32(),
                    Name = game.GetProperty("name").GetString() ?? string.Empty,
                    Rating = game.GetProperty("rating").GetDouble(),
                    ImageUrl = game.GetProperty("background_image").GetString() ?? string.Empty,
                    ReleaseDate = DateTime.TryParse(
                        game.GetProperty("released").GetString() ?? string.Empty,
                        out var releaseDate) ? releaseDate : DateTime.MinValue,
                    Genres = game.GetProperty("genres").EnumerateArray()
                        .Select(g => g.GetProperty("name").GetString() ?? string.Empty)
                        .ToList(),
                    Platforms = game.GetProperty("platforms").EnumerateArray()
                        .Select(p => p.GetProperty("platform").GetProperty("name").GetString() ?? string.Empty)
                        .ToList()
                });
            }
            return games;
        }
    }
}
