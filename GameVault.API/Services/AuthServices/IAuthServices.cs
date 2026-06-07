using GameVault.API.DTO.Auth;

namespace GameVault.API.Services.AuthServices
{
    public interface IAuthServices
    {
        Task RegisterAsync(RegisterDto usuario);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}
