using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ValueBlue.Movies.Domain.Entities
{
    /// <summary>
    /// Entity class. It represents the structure of data stored in MongoDB
    /// </summary>
    public class SearchRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("search_token")]
        public string SearchToken { get; set; } = null!;

        [BsonElement("imdbID")]
        public string ImdbId { get; set; }

        [BsonElement("processing_time_ms")]
        public double ProcessingTime { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("ip_address")]
        public string IpAddress { get; set; }
    }

}
