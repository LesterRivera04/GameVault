using GameVault.API.Data;
using GameVault.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _context;
        public GameRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            return await _context.Games
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .ToListAsync();
        }

        public async Task<Game?> GetGameByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task AddGameAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGameAsync(Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGameAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
        }

        // Método adicional para agregar a favoritos
        public async Task AddUserGameAsync(UserGame userGame)
        {
            var userExists = await _context.Usuarios.AnyAsync(u => u.Id == userGame.UserId);
            var gameExists = await _context.Games.AnyAsync(g => g.Id == userGame.GameId);

            if (!userExists || !gameExists)
            {
                throw new Exception("el Usuario o el Juego no existen");
            }

            var exists = await _context.UserGames.AnyAsync(ug => ug.UserId == userGame.UserId && ug.GameId == userGame.GameId);
            if (!exists)
            {
                _context.UserGames.Add(userGame);
                await _context.SaveChangesAsync();
            }
            
        }

        // Método adicional para obtener los juegos más recientes
        public async Task<IEnumerable<Game>> GetMostRecentGamesAsync(int count)
        {
            return await _context.Games
                .OrderByDescending(g => g.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        // Método adicional para obtener los juegos más populares
        public async Task<IEnumerable<Game>> GetMostPopularGamesAsync(int count)
        {
            return await _context.Games
                .OrderByDescending(g => g.TimesAdded)
                .Take(count)
                .ToListAsync();
        }

        // Método adicional para incrementar TimesAdded
        public async Task IncrementTimesAddedAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                game.TimesAdded++;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsUserGameAsync(int userId, int gameId)
        {
            return await _context.UserGames.AnyAsync(ug => ug.UserId == userId && ug.GameId == gameId);
        }
    }
}
