namespace GameVault.API.DTO
{
    public class RawgPlatformResponse
    {
        public int Count { get; set; }
        //public string Next { get; set; }
        //public string Previous { get; set; }
        public List<RawgPlatformDto> Results { get; set; }
    }
}
