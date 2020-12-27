using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;

namespace toDoList.Data
{
    public interface IAuthRepository
    {
        Task<User> Login(string userName, string password);
        Task<bool> UserExists(string userName);
        Task<User> Register(User user, string password);

    }
}
