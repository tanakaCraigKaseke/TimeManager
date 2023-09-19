using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.Data;
using TimeManager.Data.Models;
using TimeManager.MainLibrary.Dtos;
using TimeManager.MainLibrary.Helpers;
using TimeManager.MainLibrary.Interfaces;

namespace TimeManager.MainLibrary.Services
{
    public class ModuleService
    {
        public ModuleService() { }


        public static DataResponse CreateModule(AddModuleDto newModule)
        {
            try
            {
                //check if this module belongs to the og list
                var user = InMemoryDatabase.Users.FirstOrDefault(dbUsers => dbUsers.Id == newModule.UserId);
                //create
                if (!newModule.ShouldCreateNewSemester)
                {
                    var semester = InMemoryDatabase.Semesters.FirstOrDefault(dbSemester => dbSemester.Id == newModule.SemesterId);
                    var newModuleId = InMemoryDatabase.Modules.Count + 1;
                    var newModuleObject = new Module
                    {
                        Name = newModule.Name,
                        Code = newModule.Code,
                        Credits = newModule.Credits,
                        Hours = newModule.Hours,
                        SemesterId = newModule.SemesterId,
                        Semester = semester,
                        UserId = newModule.UserId,
                        User = user,
                        Logs = new List<UserLog>()
                    };

                    InMemoryDatabase.Modules.Add(newModuleObject);
                    semester.Modules.Add(newModuleObject);
                    user.Modules.Add(newModuleObject);

                    return new DataResponse
                    {
                        IsSuccsesful = true,
                        Message = "Successfully added user"
                    };
                }

                var newSemester = new Semester
                {
                    Name = newModule.SemesterName,
                    Weeks = newModule.Weeks,
                    StartDate = newModule.SemesterStartDate,
                    UserId = newModule.UserId,
                    User = user,
                    Modules = new List<Module>()
                };

                InMemoryDatabase.Semesters.Add(newSemester);


                var newModuleObjectModule = new Module
                {
                    Name = newModule.Name,
                    Code = newModule.Code,
                    Credits = newModule.Credits,
                    Hours = newModule.Hours,
                    SemesterId = newModule.SemesterId,
                    Semester = newSemester,
                    UserId = newModule.UserId,
                    User = user,
                    Logs = new List<UserLog>()
                };

                newSemester.Modules.Add(newModuleObjectModule);
                user.Modules.Add(newModuleObjectModule);

                return new DataResponse
                {
                    IsSuccsesful = true,
                    Message = "Successfully added user"
                };

            } catch (Exception ex)
            {
                return new DataResponse
                {
                    IsSuccsesful = false,
                    Message = $"Could not add a user exeption: {ex.Message}"
                };
            };
        }
    





  
        public static DataResponse ListAllUserModules(int userId, DateTime date )
        {
            var user = InMemoryDatabase.Users.FirstOrDefault(u => u.Id == userId);
            if(user != null)
            {
                var modules = InMemoryDatabase.Modules.ToList().FindAll(m => m.UserId == userId)
                                                        .Select(m => new UserModuleDto
                                                        {
                                                            Name = m.Name,
                                                            Code = m.Code,
                                                            SelfSudyHoursPerWeek = CalculateSelfStudyHours(m),
                                                            HoursRemaining = CalculateHoursRemaining(m, date),
                                                            HoursSpent = CalculateHoursSpent(m, date),
                                                            UserModuleId = m.Id,
                                                            Credits  = m.Credits
                                                        });

                return new DataResponse  
                {
                    Data = modules,
                    Message = "Succesfully retrieved modules for the specified date",
                    IsSuccsesful = true,
                };
            }

            return new DataResponse
            {
                IsSuccsesful = false,
                Message = "The selected user does not exist in the database"
            };
        }

        private static double CalculateSelfStudyHours(Module m)  
        {
 
                    var response = ((m.Credits * 10) / m.Semester.Weeks) - m.Hours;
                    return response;
 
        }

        private static double CalculateHoursRemaining(Module m, DateTime date)
        {

            return CalculateSelfStudyHours(m) - CalculateHoursSpent(m, date);

        }

        private static double CalculateHoursSpent(Module m, DateTime date)
        {
            // Get today's date
            DateTime currentDate = DateTime.Today;
            // Calculate the start date of the current week (Sunday)
            DateTime startDateOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
            // Calculate the end date of the current week (Saturday)
            DateTime endDateOfWeek = startDateOfWeek.AddDays(6);

            var logsInCurrentWeek = m.Logs.Where(log => log.Date >= startDateOfWeek && log.Date <= endDateOfWeek).ToList();

            var totalHoursSpent = logsInCurrentWeek.Sum(log => (log.EndTime - log.StartTime).TotalHours);

            return totalHoursSpent;
        }


    }
}
; 