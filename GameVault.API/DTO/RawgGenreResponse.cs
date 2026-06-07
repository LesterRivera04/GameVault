namespace GameVault.API.DTO
{
    public class RawgGenreResponse
    {
        public int Count { get; set; }
        //public string Next { get; set; }
        //public string Previous { get; set; }
        public List<RawgGenreDto> Results { get; set; }
    }
}
