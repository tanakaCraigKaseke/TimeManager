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

        public static ObservableCollection<Semester> Semesters { get; set; } = new ObservableCollection<Semester>();
        public static ObservableCollection<UserLog> UserLogs { get; set; } = new ObservableCollection<UserLog>();
        public static ObservableCollection<Module> Modules { get; set; } = new ObservableCollection<Module>();
    }
}
