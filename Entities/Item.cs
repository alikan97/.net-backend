namespace Server.Entities
{
    public record Item
    {
        // Use Record types for immutable objects
        public Guid Id {get; init; } 
    }
}