using ValueBlue.Movies.Application.Interfaces;
using ValueBlue.Movies.Domain.Models;

namespace ValueBlue.Movies.Application.Services
{
    public class MovieSearchService : IMovieSearchService
    {
        private readonly IOmdbServiceAgent omdbServiceAgent;

        public MovieSearchService(IOmdbServiceAgent omdbServiceAgent)
        {
            this.omdbServiceAgent = omdbServiceAgent;
        }
        public async Task<Movie> GetMovieByTitle(string title)
        {
            return await omdbServiceAgent.SearchMovieByTitle(title);
        }
    }
}
