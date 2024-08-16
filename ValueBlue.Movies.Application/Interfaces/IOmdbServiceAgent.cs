using ValueBlue.Movies.Domain.Models;

namespace ValueBlue.Movies.Application.Interfaces
{
    public interface IOmdbServiceAgent
    {
        Task<Movie> SearchMovieByTitle(string title);
    }
}