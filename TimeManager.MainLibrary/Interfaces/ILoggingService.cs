using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.MainLibrary.Helpers;

namespace TimeManager.MainLibrary.Interfaces
{
    public interface ILoggingService
    {
        DataResponse LogHours(int userModuleId, int userId, DateTime date, TimeSpan startTime, TimeSpan endTime);
    }

}
