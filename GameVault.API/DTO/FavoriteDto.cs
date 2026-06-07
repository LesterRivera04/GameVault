namespace GameVault.API.DTO
{
    public class FavoriteDto
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public bool IsFavorite { get; set; }
    }
}
