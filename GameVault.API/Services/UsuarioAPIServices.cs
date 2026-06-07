using GameVault.API.DTO;
using GameVault.API.Models;
using GameVault.API.Repository;

namespace GameVault.API.Services
{
    public class UsuarioAPIServices : IUsuarioAPIServices
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioAPIServices(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync()
        {
            var usuario = await _usuarioRepository.GetAllUsuariosAsync();
            return usuario.Select(u => new UsuarioDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Role = u.Role,
                CreatedAt = u.CreatedAt,
                IsActive = u.IsActive,
            });
        }
        public async Task<UsuarioDto?> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
            if (usuario == null) 
                return null;
            return new UsuarioDto
            {
                Id = usuario.Id,
                UserName= usuario.UserName,
                Email= usuario.Email,
                Role = usuario.Role,
                CreatedAt = DateTime.Now,
                IsActive = usuario.IsActive,
            };
        }
        public async Task AddUsuarioAsync(CreateUsuarioDto usuario)
        {
            var existingUsuario = await _usuarioRepository.GetByEmailAsync(usuario.Email);
            if (existingUsuario != null)
            {
                throw new Exception("Ese email ya está registrado");
            }
            var nuevoUsuario = new Usuario
            {
                UserName = usuario.UserName,
                Email = usuario.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password),
                Role = usuario.Role,
                IsActive=usuario.IsActive,
                CreatedAt = DateTime.Now
            };
            await _usuarioRepository.AddUsuarioAsync(nuevoUsuario);
        }
        public async Task UpdateUsuarioAsync(int id, EditUsuarioDto usuario)
        {
            var existingUsuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
            if (existingUsuario == null)
            {
                throw new Exception("Usuario no encontrado");
            }
            if (!string.IsNullOrEmpty(usuario.UserName))
            {
                existingUsuario.UserName = usuario.UserName;
            }
            if (!string.IsNullOrWhiteSpace(usuario.Email))
            {
                existingUsuario.Email = usuario.Email;
            }
            if (!string.IsNullOrWhiteSpace(usuario.Password))
            {
                existingUsuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            }
            if (usuario.IsActive.HasValue)
            {
                existingUsuario.IsActive = usuario.IsActive.Value;
            }
            if (!string.IsNullOrWhiteSpace(usuario.Role))
            {
                existingUsuario.Role = usuario.Role;
            }

            // esto hace que se actualice el objeto completo, o sea obliga a cambiar reescribir todos los campos
            //if (existingUsuario != null)
            //{
            //    existingUsuario.UserName = usuario.UserName;
            //    existingUsuario.Email = usuario.Email;
            //    existingUsuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            //    await _usuarioRepository.UpdateUsuarioAsync(existingUsuario);
            //}

            await _usuarioRepository.UpdateUsuarioAsync(existingUsuario);
        }
        public async Task DeleteUsuarioAsync(int id)
        {
            await _usuarioRepository.DeleteUsuarioAsync(id);
        }
    }
}
