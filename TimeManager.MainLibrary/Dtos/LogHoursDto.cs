using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.Data.Models;

namespace TimeManager.MainLibrary.Dtos
{
    public class LogHoursDto
    {
        public DateTime Date { get; set; }
        public double HoursSpent { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }
        public int LoggedInUserId { get; set; }
    }
}
