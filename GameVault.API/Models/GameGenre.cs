namespace GameVault.API.Models
{
    public class GameGenre
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int GenreId { get; set; }
        // Navigation properties / propiedades de navegación
        // [ForeignKey("Game")] no es del todo necesario si seguimos las convenciones de Entity Framework, pero se puede usar para mayor claridad
        public Game Game { get; set; }
        public Genre Genre { get; set; }
    }
}
