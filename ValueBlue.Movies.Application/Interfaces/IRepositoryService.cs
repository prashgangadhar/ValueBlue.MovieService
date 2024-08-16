using MongoDB.Bson;
using MongoDB.Driver;
using ValueBlue.Movies.Domain.Entities;
using ValueBlue.Movies.Domain.Models;

namespace ValueBlue.Movies.Application.Interfaces
{
    public interface IRepositoryService
    {
        Task CreateEntity(SearchRequest searchRequest);
        Task<List<SearchRequest>> GetAll();
        Task<SearchRequest> GetById(string id);
        Task<bool> DeleteById(string id);
        Task DeleteEntity(SearchRequest searchRequest);
        Task<List<SearchRequest>> GetByRange(DateTime startDate, DateTime endDate);
        Task<List<SearchRequestsPerDay>> GetRequestsPerDay();
        Task<List<SearchRequest>> GetTopNRequests(int n);
        Task<long> DeleteBefore(DateTime date);
    }
}
