namespace GameVault.API.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password {  get; set; }
        public string Role { get; set; } = "User"; // "User" o "Admin"
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public List<UserGame> UserGames { get; set; } = new();
    }
}
