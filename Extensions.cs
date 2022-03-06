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
                CreatedDate = item.CreatedDate,
            };
        }
        public static UserDto AsDto (this User user)
        {
            return new UserDto{
                Email = user.Email,
            };
        }
    }
}