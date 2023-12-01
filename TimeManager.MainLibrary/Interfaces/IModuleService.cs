using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.MainLibrary.Dtos;
using TimeManager.MainLibrary.Helpers;

namespace TimeManager.MainLibrary.interfaces
{
    public interface IModuleService
    {
        Task<DataResponse> CreateModule(AddModuleDto newModule);
        Task<DataResponse> ListAllUserModules(int userId, DateTime date);
        Task<DataResponse> SetModuleReminder(AddModuleReminder moduleReminder);
        Task<DataResponse> GetDailyReminders(int userId);
    }
}
