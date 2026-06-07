namespace GameVault.API.DTO.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UsuarioResponseDto UsuarioResponseDto { get; set; }
    }
}
