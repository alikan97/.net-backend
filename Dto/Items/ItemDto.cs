using System;

namespace Server.Dtos
{
    public record ItemDto
    {
        // Use Record types for immutable objects
        public Guid Id { get; init; } // init used as a creation expression (only used when created)
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public DateTimeOffset CreatedDate { get; init; }
    }
}