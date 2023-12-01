using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.Data.Models;

namespace TimeManager.MainLibrary.Dtos
{

 
    public class SemesterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weeks { get; set; }
        public DateTime StartDate { get; set; }
        public int NumberOfModules { get; set; } = 0;
    }
}
