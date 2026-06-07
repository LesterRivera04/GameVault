using GameVault.API.Models;

namespace GameVault.API.Repository
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllGamesAsync();
        Task<Game?> GetGameByIdAsync(int id);
        Task AddGameAsync(Game game);
        Task UpdateGameAsync(Game game);
        Task DeleteGameAsync(int id);
        Task AddUserGameAsync(UserGame userGame);
        Task<IEnumerable<Game>> GetMostRecentGamesAsync(int count);
        Task<IEnumerable<Game>> GetMostPopularGamesAsync(int count);
        Task IncrementTimesAddedAsync(int id);
        Task<bool> ExistsUserGameAsync(int userId, int gameId);
    }
}
