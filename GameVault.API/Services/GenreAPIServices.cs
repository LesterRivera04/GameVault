using GameVault.API.DTO;
using GameVault.API.Models;
using GameVault.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.API.Services
{
    public class GenreAPIServices : IGenreAPIServices
    {
        private readonly HttpClient _httpClient;
        private readonly IGenreRepository _genreRepository;
        private readonly IConfiguration _configuration;

        public GenreAPIServices(IHttpClientFactory httpClientFactory,
            IGenreRepository genreRepository,
            IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("RawgApi");
            _genreRepository = genreRepository;
            _configuration = configuration;
        }

        // traer datos desde RAWG
        public async Task<List<Genre>> GetGenresFromRawg()
        {
            var apiKey = _configuration["Rawg:ApiKey"];
            var response = await _httpClient.GetFromJsonAsync<RawgGenreResponse>(
                $"genres?key={apiKey}");
            return response.Results.Select(g => new Genre
            {
                Name = g.Name
            }).ToList();
        }

        // Sincronizar datos con la base de datos
        public async Task SyncGenres()
        {
            var rawgGenres = await GetGenresFromRawg();
            var existingGenres = await _genreRepository.GetAllGenresAsync();
            foreach (var genre in rawgGenres)
            {
                var exists = existingGenres.Any(g => g.Name == genre.Name);
                if (!exists)
                {
                    await _genreRepository.AddGenreAsync(new Genre
                    {
                        Name = genre.Name
                    });
                }
            }
        }

        // metodos manuales para el Controller y el CRUD
        [HttpGet]
        public async Task<List<Genre>> GetAllAsync()
        {
            return await _genreRepository.GetAllGenresAsync();
        }

        [HttpGet]
        public async Task<Genre?> GetByIdAsync(int id)
        {
            return await _genreRepository.GetGenreByIdAsync(id);
        }

        [HttpPost]
        public async Task AddAsync(GenreCreateDto genre)
        {
            var newGenre = new Genre
            {
                Name = genre.Name
            };
            await _genreRepository.AddGenreAsync(newGenre);
        }
    }
}
