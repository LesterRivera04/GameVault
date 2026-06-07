namespace GameVault.API.DTO
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
            public string Role { get; set; }
            public DateTime CreatedAt { get; set; }
            public bool IsActive { get; set; }
    }
}
