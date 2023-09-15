using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.Data.Models;

namespace TimeManager.Data
{
    public static class InMemoryDatabase
    {

        public static List<User> Users { get; set; } = new List<User> { new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "johndoe" } };

        public static List<Semester> Semesters { get; set; } = new List<Semester> { new Semester { Id = 1, Name = "Semester 1", } };
        public static List<UserModule> UserModules { get; set; }

        public static List<UserLog> UserLogs { get; set; }

        public static List<Module> Modules { get; set; }

    }
}
