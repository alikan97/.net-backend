using Server.Dtos;
using System.Threading.Tasks;
using Server.Entities;

namespace Server.Repositories
{
    public interface IUserRepository
    {
        Task Register(User user);
        Task<User> GetUserAsync (UserDto user);
    }
}