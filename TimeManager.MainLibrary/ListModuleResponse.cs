using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.MainLibrary
{
    public class ListModuleResponse
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public double SelfSudyHours { get; set; }
        public double HoursRemaining { get; set; }
        public double HoursSpent { get; set; }
        public int UserModuleId { get; set; }

        public int Credits { get; set; }
 
    }
}
