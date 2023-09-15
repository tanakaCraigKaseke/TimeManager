using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.MainLibrary.Helpers;

namespace TimeManager.MainLibrary.Interfaces
{
    public interface IModuleService
    {
        DataResponse CreateModule(string name, string code, int credits, double hours, int loggedInUserId = 1, int semesterId = 1);

        DataResponse ListAllUserModules(int userId, DateTime date);
    }
}
