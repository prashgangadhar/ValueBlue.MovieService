using Microsoft.AspNetCore.Mvc;
using ValueBlue.Movies.Application.Interfaces;
using ValueBlue.Movies.Domain.Entities;
using ValueBlue.Movies.Domain.Models;

namespace ValueBlue.Movies.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly ILogger<AdminController> logger;

        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            this.adminService = adminService;
            this.logger = logger;
        }

        /// <summary>
        /// Gets all requests
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllRequests")]
        public async Task<ActionResult<List<SearchRequest>>> GetAllRequests()
        {
            logger.LogInformation("Received request to get all search requests");
            var requests = await adminService.GetAllSearchRequests();
            if (requests == null) return NotFound("No requests found");
            return Ok(requests);
        }

        /// <summary>
        /// Gets request by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetRequestById/{id}")]
        public async Task<ActionResult<SearchRequest>> GetRequestById(string id)
        {
            logger.LogInformation("Received request to get search request by id {id}", id);
            var requests = await adminService.GetSearchRequestById(id);
            if (requests == null) return NotFound("Request not found with the specified id");
            return Ok(requests);
        }

        /// <summary>
        /// Gets top n requests
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        [HttpGet("GetLatestNRequests/{number}")]
        public async Task<ActionResult<SearchRequest>> GetLatestNRequests(int number)
        {
            logger.LogInformation("Received request to get latest {number} search requests", number);
            var requests = await adminService.GetLatestNRequests(number);
            if (requests == null) return NotFound("No requests found");
            return Ok(requests);
        }

        /// <summary>
        /// Gets all the requests between a range of dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("GetRequestsInRange")]
        public async Task<ActionResult<List<SearchRequest>>> GetRequestsInRange(DateTime startDate, DateTime endDate)
        {
            logger.LogInformation("Received request to get search requests in the range of {startDate} and {endDate}", startDate, endDate);
            var requests = await adminService.GetSearchRequestsInRange(startDate, endDate);
            if (requests == null) return NotFound("No requests found");
            return Ok(requests);
        }

        /// <summary>
        /// Gets an aggregated list of number of requests per day
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRequestsPerDay")]
        public async Task<ActionResult<List<SearchRequestsPerDay>>> GetRequestsPerDay()
        {
            logger.LogInformation("Received request to get aggregated search requests per day");
            var result = await adminService.GetSearchRequestsPerDay();
            if (result == null || !result.Any())
            {
                return NotFound("No requests found");
            }
            return Ok(result);
        }

        /// <summary>
        /// Deletes request with a supplied id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteRequest/{id}")]
        public async Task<IActionResult> DeleteRequest(string id)
        {
            logger.LogInformation("Received request to delete search requests by id {id}", id);
            var success = await adminService.DeleteSearchRequestById(id);
            if (success) return NoContent();
            return NotFound("Request not found with the specified id");
        }

        /// <summary>
        /// Deletes requests before the supplied date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpDelete("DeleteRequestsBeforeDate")]
        public async Task<IActionResult> DeleteRequestsBeforeDate(DateTime date)
        {
            logger.LogInformation("Received request to delete search requests before date {beforeDate}", date);
            var deletedCount = await adminService.DeleteRequestsBefore(date);

            return Ok(new { Message = $"{deletedCount} records deleted before date {date}." });
        }
    }
}
