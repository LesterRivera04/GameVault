namespace MinimalAPI.Dashboard.Models
{
    public class UserGame
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public DateTime AddedAt { get; set; }
        public bool IsFavorite { get; set; }

        // Navigation properties / propiedades de navegación
        //[ForeignKey("UserId")] no es del todo necesario si seguimos las convenciones de Entity Framework, pero se puede usar para mayor claridad
        public Usuario Usuario { get; set; }
        //[ForeignKey("GameId")]
        public Game Game { get; set; }
    }
}
