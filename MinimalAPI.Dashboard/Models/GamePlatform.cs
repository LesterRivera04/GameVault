namespace MinimalAPI.Dashboard.Models
{
    public class GamePlatform
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int PlatformId { get; set; }

        // Navigation properties / propiedades de navegación
        // [ForeignKey("Game")] no es del todo necesario si seguimos las convenciones de Entity Framework, pero se puede usar para mayor claridad
        public Game Game { get; set; }
        public Platform Platform { get; set; }
    }
}
