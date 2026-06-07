using GameVault.API.DTO;
using GameVault.API.Models;

namespace GameVault.API.Services
{
    public interface IUsuarioAPIServices
    {
        Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync();
        Task<UsuarioDto?> GetUsuarioByIdAsync(int id);
        Task AddUsuarioAsync(CreateUsuarioDto usuario);
        Task UpdateUsuarioAsync(int id, EditUsuarioDto usuario);
        Task DeleteUsuarioAsync(int id);
    }
}
