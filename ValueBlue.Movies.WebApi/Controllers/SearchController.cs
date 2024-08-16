using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ValueBlue.Movies.Application.Exceptions;
using ValueBlue.Movies.Application.Interfaces;
using ValueBlue.Movies.Domain.Entities;
using ValueBlue.Movies.Domain.Models;

namespace ValueBlue.Movies.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class SearchController : ControllerBase
    {
        private readonly IMovieSearchService movieSearchService;
        private readonly IRepositoryService repositoryService;
        private readonly ILogger<SearchController> logger;

        public SearchController(IMovieSearchService movieSearchService, IRepositoryService repositoryService, ILogger<SearchController> logger)
        {
            this.movieSearchService = movieSearchService;
            this.repositoryService = repositoryService;
            this.logger = logger;
        }

        /// <summary>
        /// Search for a movie by title
        /// </summary>
        /// <param name="title"></param>
        [HttpGet("GetMovieByTitle")]
        public async Task<ActionResult<Movie>> GetMovieByTitle(string title)
        {
            logger.LogInformation("Received request to search for movie with title {title}", title);
            var movie = new Movie();
            try
            {
                var stopwatch = Stopwatch.StartNew();
                movie = await movieSearchService.GetMovieByTitle(title);
                if (movie == null || string.IsNullOrEmpty(movie.Title) || string.IsNullOrEmpty(movie.imdbID))
                {
                    return NotFound("Could not find the requested movie details");
                }

                var searchRequest = new SearchRequest
                {
                    SearchToken = movie.Title,
                    ImdbId = movie.imdbID,
                    ProcessingTime = stopwatch.Elapsed.TotalMilliseconds,
                    Timestamp = DateTime.UtcNow,
                    IpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()
                };
                await repositoryService.CreateEntity(searchRequest);

                return Ok(movie);
            }
            catch (MovieSearchFailedException ex)
            {
                logger.LogError(ex, ex.Message);
                return NotFound("Could not find the requested movie details");
            }
            catch (SaveSearchRequestFailedException ex)
            {
                logger.LogError(ex, ex.Message);
                return Ok(movie);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return NotFound("Could not find the requested movie details");
            }
        }
    }
}
