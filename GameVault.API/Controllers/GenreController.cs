using GameVault.API.DTO;
using GameVault.API.Models;
using GameVault.API.Repository;
using GameVault.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace GameVault.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : Controller
    {
        private readonly IGenreAPIServices _genreAPIServices;

        public GenreController(IGenreAPIServices genreAPIServices)
        {
            _genreAPIServices = genreAPIServices;
        }

        // Endpoint para obtener todos los géneros
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _genreAPIServices.GetAllAsync();
            return Ok(genres); //uso return Ok en lugar de return View() xq es una API, y lo que manda a MVC es un JSON, no una vista
        }

        // Endpoint para sincronizar géneros con la API de RAWG
        [HttpPost("sync")]
        public async Task<IActionResult> SyncGenres()
        {
            await _genreAPIServices.SyncGenres();
            return Ok(new { message = "Géneros sincronizados exitosamente" });
        }

        // Endpoint para agregar un nuevo género manualmente
        [HttpPost]
        public async Task<IActionResult> AddGenre(GenreCreateDto genre)
        {
            await _genreAPIServices.AddAsync(genre);
            return Ok(new { message = "Género agregado exitosamente" });
        }

        // Endpoint para obtener un género por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var genre = await _genreAPIServices.GetByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }
    }
}
