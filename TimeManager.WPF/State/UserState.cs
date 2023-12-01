using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.MainLibrary.Dtos;

namespace TimeManager.WPF.State
{
    public static class UserState
    {
        public static bool IsLoggedIn { get; set; } = false;

        public static UserResponseDto LoggedInUser { get; set; } 

    }
}
