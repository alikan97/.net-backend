using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Entities;

namespace Server.Repositories
{
    public interface IUserRepository
    {
        Task Register(User user);
        Task Login (string email, string Password);
        Task RecoverAccount(string email);
    }
}