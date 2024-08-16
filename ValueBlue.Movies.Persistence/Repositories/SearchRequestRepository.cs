using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Movies.Application.Interfaces;
using ValueBlue.Movies.Domain.Entities;
using ValueBlue.Movies.Domain.Models;

namespace ValueBlue.Movies.Persistence.Repositories
{
    public class SearchRequestRepository : ISearchRequestRepository
    {
        private readonly IMongoCollection<SearchRequest> searchRequestCollection;

        public SearchRequestRepository(IOptions<MovieSearchHistoryDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            searchRequestCollection = mongoDatabase.GetCollection<SearchRequest>(
                databaseSettings.Value.CollectionName);
        }

        public async Task<List<SearchRequest>> GetAsync() =>
            await searchRequestCollection.Find(_ => true).ToListAsync();

        public async Task<SearchRequest?> GetAsync(string id) =>
            await searchRequestCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(SearchRequest searchRequest) =>
            await searchRequestCollection.InsertOneAsync(searchRequest);

        public async Task UpdateAsync(string id, SearchRequest updatedSearchRequest) =>
            await searchRequestCollection.ReplaceOneAsync(x => x.Id == id, updatedSearchRequest);

        public async Task<bool> RemoveAsync(string id)
        {
            var result = await searchRequestCollection.DeleteOneAsync(x => x.Id == id);
            return result?.DeletedCount > 0;
        }
        public async Task<List<SearchRequest>> GetByRangeAsync(DateTime startDate, DateTime endDate)
        {
            var filter = Builders<SearchRequest>.Filter.And(
                Builders<SearchRequest>.Filter.Gte(r => r.Timestamp, startDate),
                Builders<SearchRequest>.Filter.Lte(r => r.Timestamp, endDate)
            );
            return await searchRequestCollection.Find(filter).ToListAsync();
        }

        public async Task<List<SearchRequestsPerDay>> GetRequestsPerDayAsync()
        {
            var result = await searchRequestCollection.Aggregate()
                .Group(
                    r => new DateTime(r.Timestamp.Year, r.Timestamp.Month, r.Timestamp.Day),
                    g => new SearchRequestsPerDay { Date = g.Key, Count = g.Count() }
                )
                .SortBy(g => g.Date)
                .ToListAsync();

            return result;
        }

        public async Task<List<SearchRequest>> GetTopNRequestsAsync(int n)
        {
            var topRequests = await searchRequestCollection.Aggregate()
            .SortByDescending(sr => sr.Timestamp)
            .Limit(n)
            .ToListAsync();

            return topRequests;
        }

        public async Task<long> RemoveBeforeAsync(DateTime date)
        {
            var filter = Builders<SearchRequest>.Filter.Lt(sr => sr.Timestamp, date);
            var result = await searchRequestCollection.DeleteManyAsync(filter);

            return result.DeletedCount;
        }
    }
}
