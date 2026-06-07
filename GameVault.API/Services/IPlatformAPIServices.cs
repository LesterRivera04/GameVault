using GameVault.API.DTO;
using GameVault.API.Models;

namespace GameVault.API.Services
{
    public interface IPlatformAPIServices
    {
        Task<List<Platform>> GetPlatformsFromRawg();
        Task SyncPlatforms();
        Task<List<Platform>> GetAllAsync();
        Task<Platform?> GetByIdAsync(int id);
        Task AddAsync(PlatformCreateDto platform);

    }
}
