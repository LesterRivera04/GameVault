using GameVault.API.DTO;
using GameVault.API.Models;

namespace GameVault.API.Services
{
    public interface IGameAPIServices
    {
        Task<IEnumerable<GameDto>> GetAllGamesAsync();
        Task<GameDto?> GetGameByIdAsync(int id);
        Task AddGameAsync(CreateGameDto game);
        Task UpdateGameAsync(int id, EditGameDto gameDto);
        Task DeleteGameAsync(int id);
        Task<object> GetAllGamesFromRawg(int page = 1, string? search = null, int? genreId = null, int? platformId = null);
        Task<object> GetGamesFromRawg(string search);
        Task<IEnumerable<GameDto>> FilterGamesAsync(string? name, int? genreId, int? platformId);
        Task AddFavoriteAsync(FavoriteDto dto);
    }
}
