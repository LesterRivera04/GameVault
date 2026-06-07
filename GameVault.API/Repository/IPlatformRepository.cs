using GameVault.API.Models;

namespace GameVault.API.Repository
{
    public interface IPlatformRepository
    {
        Task<List<Platform>> GetAllPlatformsAsync();
        Task<Platform> GetPlatformByIdAsync(int id);
        Task AddPlatformAsync(Platform platform);
        Task UpdatePlatformAsync(Platform platform);
        Task DeletePlatformAsync(int id);
    }
}
