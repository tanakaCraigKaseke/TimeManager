using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.MainLibrary.Dtos
{
    public class AddSemesterDto
    {
        public string Name { get; set; }
        public int Weeks { get; set; }
        public DateTime StartDate { get; set; }
    }
}
 