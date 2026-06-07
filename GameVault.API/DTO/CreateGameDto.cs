namespace GameVault.API.DTO
{
    public class CreateGameDto
    {
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double Rating { get; set; }
        public string ImageUrl { get; set; }
        
        public List<int> GenreIds { get; set; }
        public List<int> PlatformIds { get; set; }
    }
}
