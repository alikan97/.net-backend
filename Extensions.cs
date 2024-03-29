using Server.Dtos;
using Server.Entities;
namespace Server
{
    public static class Extensions
    {
        public static ItemDto AsDto (this Item item)
        {
            return new ItemDto{
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Category = item.Category,
                CreatedDate = item.CreatedDate,
            };
        }
    }
}