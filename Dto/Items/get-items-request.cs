using System;

namespace Server.Dtos
{
    public record GetItemsFilters
    {
        public string[] Categories {get; set;}
        public int price {get; set;}
    }
}