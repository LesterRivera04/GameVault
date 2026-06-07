namespace GameVault.API.Models
{
    public class Platform
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GamePlatform> GamePlatforms { get; set; }
    }
}
