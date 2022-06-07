using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Entities
{
    public record Item
    {
        // Use Record types for immutable objects
        [BsonId]
        public Guid Id { get; init; } // init used as a creation expression (only used when created)
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Price")]
        public decimal Price { get; set; }
        [BsonElement("Category")]
        public string Category { get; set; }
        [BsonElement("CreatedDate")]
        public DateTimeOffset CreatedDate { get; init; }
    }
}