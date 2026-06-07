using GameVault.API.DTO;
using GameVault.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioAPIServices _usuarioAPIServices;
        public UsuarioController(IUsuarioAPIServices usuarioAPIServices)
        {
            _usuarioAPIServices = usuarioAPIServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsuariosAsync()
        {
            var usuarios = await _usuarioAPIServices.GetAllUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetUsuariosById(int id)
        {
            var usuarios = await _usuarioAPIServices.GetUsuarioByIdAsync(id);
            if(usuarios == null)
            {
                return NotFound();
            }
            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuarioAsync(CreateUsuarioDto usuario)
        {
            await _usuarioAPIServices.AddUsuarioAsync(usuario);
            return Ok("Usuario creado desde perfil Admin, exitosamente");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuarioAsync(int id, EditUsuarioDto usuario)
        {
            var existingUsuario = await _usuarioAPIServices.GetUsuarioByIdAsync(id);
            if (existingUsuario == null)
            {
                return NotFound();
            }
            await _usuarioAPIServices.UpdateUsuarioAsync(id, usuario);
            return Ok("Usuario actualizado exitosamente");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarioAsync(int id)
        {
            var existingUsuario = await _usuarioAPIServices.GetUsuarioByIdAsync(id);
            if (existingUsuario == null)
            {
                return NotFound();
            }
            await _usuarioAPIServices.DeleteUsuarioAsync(id);
            return Ok("Usuario eliminado exitosamente");
        }
    }
}
