using GameVault.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task<IActionResult> Index(int page = 1, string? search = null, int? genreId = null, int? platformId = null)
        {
            var model = await _gameService.GetAllGamesAsync(page, search, genreId, platformId);

            if (page > model.TotalPages && model.TotalPages > 0)
            {
                return RedirectToAction("Index", new { page = model.TotalPages, search, genreId, platformId });
            }

            ViewBag.Search = search;
            ViewBag.GenreId = genreId;
            ViewBag.PlatformId = platformId;

            return View(model);
        }

        //public async Task<IActionResult> Index(string search = "zelda")
        //{
        //    var games = await _gameService.GetAllSearchGamesAsync(search);
        //    return View(games);
        //}
    }
}
