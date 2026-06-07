using GameVault.API.DTO;
using GameVault.API.Models;
using GameVault.API.Repository;
using GameVault.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformController : Controller
    {
        private readonly IPlatformAPIServices _platformAPIService;
        public PlatformController(IPlatformAPIServices platformAPIService)
        {
            _platformAPIService = platformAPIService;
        }

        //GET: Platform
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var platforms = await _platformAPIService.GetAllAsync();
            return Ok(platforms); //uso return Ok en lugar de return View() xq es una API, y lo que manda a MVC es un JSON, no una vista
        }

        [HttpPost] //esto por si quiero hacer actualizacion manual de las plataformas, aunque lo ideal es que se actualicen automaticamente cada cierto tiempo con un servicio en segundo plano
        public async Task<IActionResult> Add(PlatformCreateDto platformCreateDto)
        {
            await _platformAPIService.AddAsync(platformCreateDto);
            return Ok();
        }

        [HttpPost("sync")] //esto es logica externa cuando hay integracion, para sincronizar consolas con la API RAWG
        public async Task<IActionResult> Sync()
        {
            await _platformAPIService.SyncPlatforms();
            return Ok("Platforms synchronized successfully");
        }

        //[HttpPost("sync")] //esto es logica para sincronizar las plataformas de la API externa con mi base de datos, lo ideal es que esto se haga automaticamente cada cierto tiempo con un servicio en segundo plano, pero por ahora lo hago manualmente con este endpoint
        //public async Task<IActionResult> SyncWithExternalAPI()
        //{
        //    var externalPlatforms = await _platformAPIServices.GetPlatformsAsync();
        //    foreach (var platform in externalPlatforms)
        //    {
        //        var existingPlatform = await _platformRepository.GetPlatformByIdAsync(platform.Id);
        //        if (existingPlatform == null)
        //        {
        //            await _platformRepository.AddPlatformAsync(platform);
        //        }
        //        else
        //        {
        //            existingPlatform.Name = platform.Name; //actualizo el nombre por si ha cambiado en la API externa
        //            await _platformRepository.UpdatePlatformAsync(existingPlatform);
        //        }
        //    }
        //    return Ok();
        //}
    }
}
