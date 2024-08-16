using MongoDB.Bson;
using MongoDB.Driver;
using ValueBlue.Movies.Domain.Entities;
using ValueBlue.Movies.Domain.Models;

namespace ValueBlue.Movies.Application.Interfaces
{
    public interface IAdminService
    {
        Task<SearchRequest> GetSearchRequestById(string id);
        Task<List<SearchRequest>> GetLatestNRequests(int n);
        Task<List<SearchRequest>> GetAllSearchRequests();
        Task<List<SearchRequest>> GetSearchRequestsInRange(DateTime startDate, DateTime endDate);
        Task<bool> DeleteSearchRequestById(string id);
        Task<List<SearchRequestsPerDay>> GetSearchRequestsPerDay();
        Task<long> DeleteRequestsBefore(DateTime date);
    }
}
