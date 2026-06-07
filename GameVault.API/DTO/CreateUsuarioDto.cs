namespace GameVault.API.DTO
{
    public class CreateUsuarioDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User"; // "User" o "Admin" usar toogle
        public bool IsActive { get; set; } = true; // usar toogle
    }
}
