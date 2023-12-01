using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.Data.Models;

namespace TimeManager.MainLibrary.Dtos
{
    public class AddModuleDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Credits { get; set; }
        public double Hours { get; set; }
        public int SemesterId { get; set; }
        public int UserId { get; set; }
        public List<UserLog> Logs { get; set; }
        public string SemesterName { get; set; }
        public DateTime SemesterStartDate { get; set; }
        public int Weeks { get; set; }
        public bool ShouldCreateNewSemester { get; set; } = true;


    }
}
