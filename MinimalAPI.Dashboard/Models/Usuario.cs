namespace MinimalAPI.Dashboard.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<UserGame> UserGames { get; set; }
    }
}
