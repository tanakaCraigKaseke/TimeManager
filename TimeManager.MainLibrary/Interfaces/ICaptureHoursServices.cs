using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.MainLibrary.Dtos;
using TimeManager.MainLibrary.Helpers;

namespace TimeManager.MainLibrary.interfaces
{
    public interface ICaptureHoursServices
    {
        Task<DataResponse> LogHours(LogHoursDto incomingHours);
    }
}
