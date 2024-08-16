using ValueBlue.Movies.Application.Interfaces;
using ValueBlue.Movies.Domain.Entities;
using ValueBlue.Movies.Domain.Models;

namespace ValueBlue.Movies.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepositoryService repositoryService;

        public AdminService(IRepositoryService repositoryService)
        {
            this.repositoryService = repositoryService;
        }

        public async Task<List<SearchRequest>> GetAllSearchRequests()
        {
            return await repositoryService.GetAll();
        }

        public async Task<SearchRequest> GetSearchRequestById(string id)
        {
            return await repositoryService.GetById(id);
        }

        public async Task<List<SearchRequest>> GetSearchRequestsInRange(DateTime startDate, DateTime endDate)
        {
            return await repositoryService.GetByRange(startDate, endDate);
        }

        public async Task<List<SearchRequestsPerDay>> GetSearchRequestsPerDay()
        {
            return await repositoryService.GetRequestsPerDay();
        }

        public async Task<bool> DeleteSearchRequestById(string id)
        {
            return await repositoryService.DeleteById(id);
        }

        public async Task<long> DeleteRequestsBefore(DateTime date)
        {
            return await repositoryService.DeleteBefore(date);
        }

        public async Task<List<SearchRequest>> GetLatestNRequests(int n)
        {
            return await repositoryService.GetTopNRequests(n);
        }
    }
}
