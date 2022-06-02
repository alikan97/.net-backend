using System;

namespace Server.Dtos
{
    public record paginatedResponse<T>
    {
        public int itemsCount {get; set;}
        public T data {get; set;}
    }
}