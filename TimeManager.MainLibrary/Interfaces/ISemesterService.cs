using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.MainLibrary.Helpers;

namespace TimeManager.MainLibrary.interfaces
{
    public interface ISemesterService
    {
        Task<DataResponse> GetAllSemesterForUser(int loggedInUserId);

        DataResponse CreateOrUpdateSemester(string name, DateTime StartDate, int numberOfWeeks);
    }
}
