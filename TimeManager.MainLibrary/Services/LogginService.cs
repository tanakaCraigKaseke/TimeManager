using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.Data;
using TimeManager.Data.Models;
using TimeManager.MainLibrary.Helpers;
using TimeManager.MainLibrary.Interfaces;

namespace TimeManager.MainLibrary.Services
{
    public class LogginService  
    {
        //public DataResponse LogHours(int userModuleId, int userId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        //{
        //    //find the usermodule to log hours for.
        //    var userModule = InMemoryDatabase.UserModules.FirstOrDefault(m=> m.Id == userModuleId);
        //    if (userModule == null)
        //    {
        //        return new DataResponse
        //        {
        //            IsSuccsesful = false,
        //            Message = "Could not find the specified user module"
        //        };
        //    }
        //    // create a new log
        //    var newLogId = InMemoryDatabase.UserLogs.Count + 1;
        //    var newLog = new UserLog
        //    {
        //        Id = newLogId,
        //        Date = date,
        //        StartTime = startTime,
        //        EndTime = endTime,
        //        UserModule = userModule,
        //        UserModuleId = newLogId,
        //    }; 

        //    // add a new log
        //    InMemoryDatabase.UserLogs.Add(newLog);
        //    // ascosciate log with module
        //    userModule.Logs.Add(newLog);

        //    return new DataResponse
        //    {
        //        Message = "Successfully logged data",
        //        IsSuccsesful = true
        //    };

        //}
    }
}
