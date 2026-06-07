using GameVault.API.DTO;
using GameVault.API.DTO.Auth;
using GameVault.API.Models;
using GameVault.API.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameVault.API.Services.AuthServices
{
    public class AuthServices : IAuthServices
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;
        public AuthServices(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task RegisterAsync(RegisterDto usuario)
        {
            var existingUsuario = await _usuarioRepository.GetByEmailAsync(usuario.Email);
            if(existingUsuario != null)
            {
                throw new Exception("no puede usar ese email, porque ya está registrado");
            }
            var nuevoUsuario = new Usuario
            {
                UserName = usuario.UserName,
                Email = usuario.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password),
                // valores FORZADOS
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };
            await _usuarioRepository.AddUsuarioAsync(nuevoUsuario);
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var existingUsuario = await _usuarioRepository.GetByEmailAsync(dto.Email);
            if(existingUsuario == null)
            {
                return null;
            }

            bool passwordMatches = BCrypt.Net.BCrypt.Verify(dto.Password, existingUsuario.Password);
            if (!passwordMatches)
            {
                return null;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, existingUsuario.Id.ToString()),
                new Claim(ClaimTypes.Name, existingUsuario.UserName),
                new Claim(ClaimTypes.Email, existingUsuario.Email),
                new Claim(ClaimTypes.Role, existingUsuario.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),     // expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDto
            {
                Token = jwt,

                UsuarioResponseDto = new UsuarioResponseDto
                {
                    Id = existingUsuario.Id,
                    UserName = existingUsuario.UserName,
                    Email = existingUsuario.Email,
                    Role = existingUsuario.Role
                }
            };
        }
    }
}
