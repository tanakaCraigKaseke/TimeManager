using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.Data.Models
{
    public class UserModule
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }
        public int SemesterId { get; set; }
        public Semester Semester { get; set; }
        public List<UserLog> Logs { get; set; }

    }
}
