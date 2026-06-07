using GameVault.API.DTO;
using GameVault.API.Models;

namespace GameVault.API.Services
{
    public interface IGenreAPIServices
    {
        Task<List<Genre>> GetGenresFromRawg();
        Task SyncGenres();
        Task<List<Genre>> GetAllAsync();
        Task<Genre?> GetByIdAsync(int id);
        Task AddAsync(GenreCreateDto genre);
    }
}
