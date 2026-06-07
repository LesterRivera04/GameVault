namespace MinimalAPI.Dashboard.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double Rating { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } //cuando guardo el juego en mi base de datos, no la fecha de lanzamiento del juego
        public int TimesAdded { get; set; }
        public List<UserGame> UserGames { get; set; } = new List<UserGame>();
        public List<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
        public List<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
    }
}
