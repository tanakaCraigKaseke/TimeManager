using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.MainLibrary.Dtos
{
    public class FiltersDto
    {
        public int loggedInUserId { get; set; }
        public int SemesterId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
