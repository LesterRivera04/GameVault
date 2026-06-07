using GameVault.API.DTO.Auth;
using GameVault.API.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authService;
        public AuthController(IAuthServices authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            await _authService.RegisterAsync(registerDto);
            return Ok("Usuario registrado exitosamente");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var token = await _authService.LoginAsync(loginDto);
            if (token == null)
            {
                return Unauthorized("Credenciales inválidas");
            }
            return Ok(new { Token = token });
        }
    }
}
