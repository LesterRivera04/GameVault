namespace GameVault.API.DTO
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double Rating { get; set; }
        public string ImageUrl { get; set; }
            public List<string> Genres { get; set; }
            public List<string> Platforms { get; set; }
    }
}
