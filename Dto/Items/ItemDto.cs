using System;

namespace Server.Dtos
{
        public record ItemDto
    {
        // Use Record types for immutable objects
        public Guid Id {get; init; } // init used as a creation expression (only used when created)
        public string Name {get; init; }
        public decimal Price {get; init; }
        public DateTimeOffset CreatedDate {get; init; }
    }
}