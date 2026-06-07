using GameVault.API.DTO;
using GameVault.API.Models;
using GameVault.API.Repository;

namespace GameVault.API.Services
{
    public class PlatformAPIServices : IPlatformAPIServices
    {
        private readonly HttpClient _httpClient;
        private readonly IPlatformRepository _platformRepository;
        private readonly IConfiguration _configuration;
        public PlatformAPIServices(IHttpClientFactory httpClientFactory,
            IPlatformRepository platformRepository,
            IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("RawgApi");
            _platformRepository = platformRepository;
            _configuration = configuration;
        }

        // Este método obtiene las plataformas desde la API de RAWG, mapea los resultados a la entidad Platform y devuelve una lista de plataformas.
        // basicamente solo trae datos desde RAWG, no hace nada con la base de datos, eso lo hace el método SyncPlatforms
        public async Task<List<Platform>> GetPlatformsFromRawg()
        {
            var apiKey = _configuration["Rawg:ApiKey"];
            var response = await _httpClient.GetFromJsonAsync<RawgPlatformResponse>(
                $"platforms?key={apiKey}"); // lo estoy llamando desde appsettings.json, no hardcodeo la key
            return response.Results.Select(p => new Platform
            {
                Name = p.Name
            }).ToList();
        }

        public async Task SyncPlatforms()
        {
            var rawgPlatforms = await GetPlatformsFromRawg();
            var existingPlatforms = await _platformRepository.GetAllPlatformsAsync();
            foreach (var platform in rawgPlatforms)
            {
                var exists = existingPlatforms.Any(p => p.Name == platform.Name);
                if (!exists)
                {
                    await _platformRepository.AddPlatformAsync(new Platform
                    {
                        Name = platform.Name
                    });
                }
            }
        }

        public async Task<List<Platform>> GetAllAsync()
        {
            return await _platformRepository.GetAllPlatformsAsync();
        }

        public async Task<Platform?> GetByIdAsync(int id)
        {
            return await _platformRepository.GetPlatformByIdAsync(id);
        }

        public async Task AddAsync(PlatformCreateDto platform)
        {
            var nuevoPlatform = new Platform
            {
                Name = platform.Name
            };
            await _platformRepository.AddPlatformAsync(nuevoPlatform);
        }
    }
}
