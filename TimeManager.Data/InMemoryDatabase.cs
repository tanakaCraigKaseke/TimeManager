using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TimeManager.Data.Models;

namespace TimeManager.Data
{
    public static class InMemoryDatabase
    {
        public static ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>
        {
            new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe",
                Semesters = new List<Semester>(),
                Modules = new List<Module>()
            }
        };

        public static ObservableCollection<Semester> Semesters { get; set; } = new ObservableCollection<Semester>
        {
            new Semester
            {
                Id = 1,
                Name = "Semester 1",
                StartDate = DateTime.Now,
                UserId = 1,
                Weeks = 10,
                Modules = new List<Module>()
            },
            new Semester
            {
                Id = 2,
                Name = "Semester 2",
                StartDate = DateTime.Now,
                UserId = 1,
                Weeks = 10,
                Modules = new List<Module>()
            }
        };

        public static ObservableCollection<UserLog> UserLogs { get; set; } = new ObservableCollection<UserLog>();
        public static ObservableCollection<Module> Modules { get; set; } = new ObservableCollection<Module>
        {
            new Module
            {
                Id = 1,
                Name = "Programming",
                Code = "This is the code",
                UserId = 1,
                Credits = 5,
                Hours = 1.5,
                SemesterId = 1,
                Semester = new Semester
                {
                    Id = 1,
                    Name = "Semester 1",
                    StartDate = DateTime.Now,
                    UserId = 1,
                    Weeks = 10,
                    Modules = new List<Module>()
                },
                User = new User(),
                Logs = new List<UserLog>()
            }
        };
    }
}
