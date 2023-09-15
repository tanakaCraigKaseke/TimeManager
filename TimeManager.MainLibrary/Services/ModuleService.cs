using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.Data;
using TimeManager.Data.Models;
using TimeManager.MainLibrary.Helpers;
using TimeManager.MainLibrary.Interfaces;

namespace TimeManager.MainLibrary.Services
{
    public class ModuleService : IModuleService
    {
        public ModuleService() { }


        public DataResponse CreateModule(string name, string code, int credits, double hours, int loggedInUserId = 1, int semesterId = 1)
        {
            //check if the module exists using Linq
            var existingModule = InMemoryDatabase.Modules.FirstOrDefault(m => m.Name.ToLower() == name.ToLower() 
                                                                    || m.Code.ToLower() == code.ToLower());
            //if the module exists take the existing
            if(existingModule != null)
            {
                //meaning that we have a module already
                // so we assign the module to the user
                return AssignModuleToUser(existingModule, loggedInUserId);
            }
            // otherwise create the module fist find the semester to add the module to
            var semesterToAddModule = InMemoryDatabase.Semesters.FirstOrDefault(semester=> semester.Id == semesterId);
            // generate a new id for our in memory database
            var newId = InMemoryDatabase.Modules.Count + 1;
            var newModule = new Module
            {
                Id = newId,
                Name = name,
                Code = code,
                Credits = credits,
                Hours = hours,
                Semester = semesterToAddModule,
                SemesterId = semesterId
            };

            // add the module to the modules table 

            InMemoryDatabase.Modules.Add(newModule);

            // add the module to the user

             return AssignModuleToUser(existingModule, loggedInUserId);
        }

        private DataResponse AssignModuleToUser(Module module, int loggedInUserId)
        {
            // first find the user
            var user = InMemoryDatabase.Users.FirstOrDefault(u => u.Id == loggedInUserId);
        
            // check if the module exists in their list of modules
            var existingModule = user.Modules.FirstOrDefault(m => m.ModuleId == module.Id);
            // if the module exists do nothing, but tell them that the module already exists.
            if (existingModule != null)
            {
                return new DataResponse
                {
                    IsSuccsesful = false, 
                    Message = "Module has already been added" 
                };
            }
            // otherwise new instance of user module and then we add the module to the users list

            var newId = InMemoryDatabase.UserModules.Count + 1;

            var newUserModule = new UserModule
            {
                Id = newId,
                UserId = loggedInUserId,
                User = user,
                ModuleId = module.Id,
                Module = module,    
            };

            InMemoryDatabase.UserModules.Add(newUserModule);

            user.Modules.Add(newUserModule);

            return new DataResponse { Data = newUserModule, IsSuccsesful = true, Message = "Succesfully added module" };
            

        }

  
        public DataResponse ListAllUserModules(int userId, DateTime date )
        {
            var user = InMemoryDatabase.Users.FirstOrDefault(u => u.Id == userId);
            if(user != null)
            {
                var module = InMemoryDatabase.UserModules.FindAll(m => m.UserId == userId)
                                                        .Select(m => new ListModuleResponse
                                                        {
                                                            Name = m.Module.Name,
                                                            Code = m.Module.Code,
                                                            SelfSudyHours = m.Module.SelfStudyHours,
                                                            HoursRemaining = CalculateHoursRemaining(m.Logs, m.Module.SelfStudyHours, date),
                                                            HoursSpent = CalculateHoursSpent(m.Logs, m.Module.SelfStudyHours, date),
                                                            UserModuleId = m.Id,
                                                            Credits  = m.Module.Credits
                                                        });
                return new DataResponse
                {
                    Data = module,
                    Message = "Succesfully retrieved date",
                    IsSuccsesful = true,
                };
            }

            return new DataResponse
            {
                IsSuccsesful = false,
                Message = "The selected user does not exist in the database"
            };
        }

        private double CalculateHoursRemaining(List<UserLog> logs, double selfStudyHours, DateTime date)
        {

            return selfStudyHours - CalculateHoursSpent(logs, selfStudyHours, date);

        }

        private double CalculateHoursSpent(List<UserLog> logs, double selfStudyHours, DateTime date)
        {
            // Get today's date
            DateTime currentDate = DateTime.Today;
            // Calculate the start date of the current week (Sunday)
            DateTime startDateOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
            // Calculate the end date of the current week (Saturday)
            DateTime endDateOfWeek = startDateOfWeek.AddDays(6);

            var logsInCurrentWeek = logs.Where(log => log.Date >= startDateOfWeek && log.Date <= endDateOfWeek).ToList();

            var totalHoursSpent = logsInCurrentWeek.Sum(log => (log.EndTime - log.StartTime).TotalHours);

            return totalHoursSpent;
        }


    }
}
; 