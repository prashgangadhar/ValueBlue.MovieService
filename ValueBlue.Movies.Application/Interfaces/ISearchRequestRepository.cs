using MongoDB.Bson;
using MongoDB.Driver;
using ValueBlue.Movies.Domain.Entities;
using ValueBlue.Movies.Domain.Models;

namespace ValueBlue.Movies.Application.Interfaces
{
    public interface ISearchRequestRepository
    {
        Task CreateAsync(SearchRequest searchRequest);
        Task<List<SearchRequest>> GetAsync();
        Task<SearchRequest?> GetAsync(string id);
        Task<List<SearchRequest>> GetByRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<SearchRequestsPerDay>> GetRequestsPerDayAsync();
        Task<List<SearchRequest>> GetTopNRequestsAsync(int n);
        Task<bool> RemoveAsync(string id);
        Task<long> RemoveBeforeAsync(DateTime date);
        Task UpdateAsync(string id, SearchRequest updatedSearchRequest);
    }
}