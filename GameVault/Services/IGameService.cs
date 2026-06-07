using GameVault.Models;

namespace GameVault.Services
{
    public interface IGameService
    {
        Task<GamesViewModel> GetAllGamesAsync(int page, string? search = null, int? genreId = null, int? platformId = null);
        Task<IEnumerable<GameViewModel>> GetAllSearchGamesAsync(string search);
    }
}
