using ValueBlue.Movies.Domain.Models;

namespace ValueBlue.Movies.Application.Interfaces
{
    public interface IMovieSearchService
    {
        Task<Movie> GetMovieByTitle(string title);
    }
}
