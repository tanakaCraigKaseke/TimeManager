using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.Data.Models
{
    public class UserLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double HoursSpent { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }  
}
