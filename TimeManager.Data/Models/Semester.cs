using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.Data.Models
{
    public class Semester
    {
        public int Id { get; set; }
        public int Weeks { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
    }
}
