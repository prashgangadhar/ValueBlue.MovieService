using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using ValueBlue.Movies.Application.Exceptions;
using ValueBlue.Movies.Application.Interfaces;
using ValueBlue.Movies.Domain.Models;

namespace ValueBlue.Movies.Infrastructure.ServiceAgent
{
    public class OmdbServiceAgent : IOmdbServiceAgent
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<OmdbServiceAgent> logger;
        private readonly string apiKey;

        public OmdbServiceAgent(HttpClient httpClient, IOptions<OmdbServiceConfig> config, ILogger<OmdbServiceAgent> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            apiKey = config.Value.ApiKey;
        }

        public async Task<Movie> SearchMovieByTitle(string title)
        {
            var url = $"?t={title}&apikey={apiKey}";
            try
            {
                var response = await httpClient.GetAsync(url);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var omdbResponse = await JsonSerializer.DeserializeAsync<Movie>(await response.Content.ReadAsStreamAsync());
                    return omdbResponse;
                }

                throw new MovieSearchFailedException($"Failed to retrieve movie details with title {title}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
