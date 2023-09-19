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

        public static List<Semester> Semesters { get; set; } = new List<Semester> { 
            new Semester { 
                Id = 1, 
                Name = "Semester 1", 
                StartDate = DateTime.Now,
                UserId = 1,
                Weeks = 10,
                Modules = new List<UserModule>()
                
            },
            new Semester {
                Id = 2,
                Name = "Semester 2",
                StartDate = DateTime.Now,
                UserId = 1,
                Weeks = 10,
                Modules = new List<UserModule>()
            },
            new Semester {
                Id = 3,
                Name = "Semester 3",
                StartDate = DateTime.Now,
                UserId = 1,
                Weeks = 10,
                Modules = new List<UserModule>()
            },
            new Semester {
                Id = 4,
                Name = "Semester 4",
                StartDate = DateTime.Now,
                UserId = 1,
                Weeks = 10,
                                Modules = new List<UserModule>()
            }, 
            new Semester {
                Id = 5,
                Name = "Semester 5",
                StartDate = DateTime.Now,
                UserId = 1,
                Weeks = 10,
                                Modules = new List<UserModule>()
            },

        };
        public static List<UserModule> UserModules { get; set; }

        public static List<UserLog> UserLogs { get; set; }

        public static List<Module> Modules { get; set; }

    }
}
