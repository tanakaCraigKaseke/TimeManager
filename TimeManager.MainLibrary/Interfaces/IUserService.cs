using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.MainLibrary.Dtos;
using TimeManager.MainLibrary.Helpers;

namespace TimeManager.MainLibrary.interfaces
{
    public interface IUserService
    {
        Task<DataResponse> InsertUsersIntoDb(UserDto newUser);
        Task<bool> UsernameExists(string username);
        Task<DataResponse> UserLogin(LoginDto login);
    }
}
