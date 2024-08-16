using Microsoft.Extensions.Logging;
using ValueBlue.Movies.Application.Exceptions;
using ValueBlue.Movies.Application.Interfaces;
using ValueBlue.Movies.Domain.Entities;
using ValueBlue.Movies.Domain.Models;

namespace ValueBlue.Movies.Application.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly ISearchRequestRepository searchRequestRepository;
        private readonly ILogger<RepositoryService> logger;

        public RepositoryService(ISearchRequestRepository searchRequestRepository, ILogger<RepositoryService> logger)
        {
            this.searchRequestRepository = searchRequestRepository;
            this.logger = logger;
        }
        public async Task CreateEntity(SearchRequest searchRequest)
        {
            try
            {
                await searchRequestRepository.CreateAsync(searchRequest);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw new SaveSearchRequestFailedException("Saving search request to db failed");
            }
        }

        public async Task<long> DeleteBefore(DateTime date)
        {
            return await searchRequestRepository.RemoveBeforeAsync(date);
        }

        public async Task<bool> DeleteById(string id)
        {
            return await searchRequestRepository.RemoveAsync(id);
        }

        public Task DeleteEntity(SearchRequest searchRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SearchRequest>> GetAll()
        {
            return await searchRequestRepository.GetAsync();
        }

        public async Task<SearchRequest> GetById(string id)
        {
            return await searchRequestRepository.GetAsync(id);
        }

        public async Task<List<SearchRequest>> GetByRange(DateTime startDate, DateTime endDate)
        {
            return await searchRequestRepository.GetByRangeAsync(startDate, endDate);
        }

        public async Task<List<SearchRequestsPerDay>> GetRequestsPerDay()
        {
            return await searchRequestRepository.GetRequestsPerDayAsync();
        }

        public async Task<List<SearchRequest>> GetTopNRequests(int n)
        {
            return await searchRequestRepository.GetTopNRequestsAsync(n);
        }
    }
}
