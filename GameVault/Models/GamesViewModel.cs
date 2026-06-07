namespace GameVault.Models
{
    public class GamesViewModel
    {
        public List<GameViewModel> Games { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
}
