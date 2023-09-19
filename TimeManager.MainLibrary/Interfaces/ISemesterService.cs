using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.MainLibrary.Helpers;

namespace TimeManager.MainLibrary.Interfaces
{
    public interface ISemesterService
    {
        DataResponse  CreateOrUpdateSemester(string name, DateTime startDate, int numberOfWeeks);
    }
}
