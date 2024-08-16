namespace ValueBlue.Movies.Domain.Models
{
    public class OmdbServiceConfig
    {
        public required string BaseUrl { get; set; }
        public required string ApiKey { get; set; }
    }
}
