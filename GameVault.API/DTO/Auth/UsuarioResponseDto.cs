namespace GameVault.API.DTO.Auth
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
            //public DateTime CreatedAt { get; set; }
            //public bool IsActive { get; set; }
    }
}
