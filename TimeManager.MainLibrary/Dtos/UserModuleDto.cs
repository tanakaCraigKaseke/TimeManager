using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.Data.Models;

namespace TimeManager.MainLibrary.Dtos
{
    public class UserModuleDto { 

            public int ModuleId { get; set; } 
            public string Name { get; set; }
            public string Code { get; set; }
            public double HoursRemaining { get; set; }
            public double HoursSpent { get; set; }
            public int UserModuleId { get; set; }
            public int Credits { get; set; }

            public int Weeks { get; set; }

            public DateTime StartDate { get; set; }

            public string SemesterName { get; set; } 

            public int TotalHoursSpent { get; set; }

            public int Hours { get; set; }

            public string HoursToString { get { return HoursRemaining.ToString() ?? "0"; } }

            public double SelfStudyHoursPerWeek { get; set; }


    }
}
