using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.Data;
using TimeManager.Data.Models;
using TimeManager.MainLibrary.Dtos;
using TimeManager.MainLibrary.Helpers;
using TimeManager.MainLibrary.interfaces;

namespace TimeManager.MainLibrary.Services
{
    // Service class responsible for managing modules and related operations.
    public class ModuleService : IModuleService
    {
        public const string ConnectionString = "Server=tcp:time-manager-server.database.windows.net,1433;Initial Catalog=timeManager;Persist Security Info=False;User ID=TimeManager;Password=formula.1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public ModuleService() { } 

        // Creates a new module based on the provided module data.
        public async  Task<DataResponse> CreateModule(AddModuleDto newModule)
        {
            try
            {
                // Check if this module belongs to an existing semester or a new one.
                //  var user = InMemoryDatabase.Users.FirstOrDefault(dbUsers => dbUsers.Id == newModule.UserId);
                var user = await SqlDataBaseController.FindUserById(newModule.UserId);

                if (!newModule.ShouldCreateNewSemester)
                {
                    // Create a module associated with an existing semester.
                    var semester = SqlDataBaseController.GetSemesterById(newModule.SemesterId);
                    

                    var newModuleObject = new Module
                    {
                        Name = newModule.Name,
                        Code = newModule.Code,
                        Credits = newModule.Credits,
                        Hours = newModule.Hours,
                        SemesterId = newModule.SemesterId,
                        UserId = newModule.UserId,
                        User = user,
                    };

                    // Add the new module and semester to the database.
                      await SqlDataBaseController.CreateModule(newModuleObject);

                    return new DataResponse
                    {
                        IsSuccsesful = true,
                        Message = "Successfully added module"
                    };
                }

                // Create a new semester for the module.
              

                var newSemester = new Semester
                {
                    Name = newModule.SemesterName,
                    Weeks = newModule.Weeks,
                    StartDate = newModule.SemesterStartDate,
                    UserId = newModule.UserId,
                    User = user,
                    Modules = new List<Module>(),
                    
                };

                int newSemesterId = await SqlDataBaseController.CreateNewSemester(newModule.UserId, newSemester);

                var newModuleObjectModule = new Module
                {
                    Name = newModule.Name,
                    Code = newModule.Code,
                    Credits = newModule.Credits,
                    Hours = newModule.Hours,
                    SemesterId = newSemesterId,
                    Semester = newSemester,
                    UserId = newModule.UserId,
                    User = user,
                    Logs = new List<UserLog>()
                };
                 
                // Add the new module and semester to the database.
                int newModuleId = await SqlDataBaseController.CreateModule(newModuleObjectModule);
                return new DataResponse
                {
                    IsSuccsesful = true,
                    Message = "Successfully created new module"
                };
            }
            catch (Exception ex)
            {
                return new DataResponse
                {
                    IsSuccsesful = false,
                    Message = $"Could not add a user exception",
                    Data = ex.Message
                };
            };
        }

        // Lists all modules associated with a user for a specific date.
        public   async Task<DataResponse> ListAllUserModules(int userId, DateTime date)
        {
            var user = await SqlDataBaseController.FindUserById(userId);

            DateTime currentDate = date;
            DayOfWeek currentDayOfWeek = currentDate.DayOfWeek;
            // Calculate the start of the week (Sunday as the first day)
            DateTime startOfWeek = currentDate.AddDays(-(int)currentDayOfWeek);

            // Calculate the end of the week (Saturday as the last day)
            DateTime endOfWeek = startOfWeek.AddDays(6);

            string startOfWeekFormatted = startOfWeek.ToString("yyyy-MM-dd");
            string endOfWeekFormatted = endOfWeek.ToString("yyyy-MM-dd");

            if (user != null)
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    var query = @"                             SELECT
                               M.Id as ModuleId,
                               M.Name,
                               M.Code,
                               M.Credits,
                               M.Hours,
                               S.Name as SemesterName,
                               S.Weeks,
                               S.StartDate,
                               COALESCE(SUM(ML.HoursSpent), 0) AS TotalHoursSpent,
                               ((M.Credits * 10) / S.Weeks) - M.Hours - COALESCE(SUM(ML.HoursSpent), 0) AS HoursRemaining,
                               ((M.Credits * 10) / S.Weeks) - M.Hours AS SelfStudyHoursPerWeek
                           FROM
                               [dbo].[Modules] AS M
                           LEFT JOIN [dbo].[Semesters] AS S ON M.Id = S.Id
                           LEFT JOIN
                               [dbo].[ModuleLogs] AS ML ON M.Id = ML.Id
                               AND ML.Date >= @WeekStartDate -- Replace @StartDate with the start date of your week
                               AND ML.Date <= @WeekEndDate  -- Replace @EndDate with the end date of your week
                           WHERE M.UserId = @UserId
                           GROUP BY
                               M.Id,
                               M.Name,
                               M.Code,
                               M.Credits,
                               M.Hours,
                               S.Name,
                               S.Weeks,
                               S.StartDate;";  

                    var response = await connection.QueryAsync<UserModuleDto>(query, new {WeekStartDate = startOfWeekFormatted,  WeekEndDate = endOfWeekFormatted, UserId = userId});

                    return new DataResponse
                    {
                        Data = response,
                        Message = "Successfully retrieved modules for the specified date",
                        IsSuccsesful = true,
                    };
                }


            }

            return new DataResponse
            {
                IsSuccsesful = false,
                Message = "The selected user does not exist in the database"
            };
        }

        // Calculates the self-study hours per week for a module.
        private   double CalculateSelfStudyHours(Module m)
        {
            var response = ((m.Credits * 10) / m.Semester.Weeks) - m.Hours;
            return response;
        }

        // Calculates the remaining hours for a module based on a date.
        private   double CalculateHoursRemaining(Module m, DateTime date)
        {
            return CalculateSelfStudyHours(m) - CalculateHoursSpent(m, date);
        }

        // Calculates the hours spent on a module based on a date.
        private   double CalculateHoursSpent(Module m, DateTime date)
        {
            // Get today's date
            DateTime currentDate = DateTime.Today;
            // Calculate the start date of the current week (Sunday)
            DateTime startDateOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
            // Calculate the end date of the current week (Saturday)
            DateTime endDateOfWeek = startDateOfWeek.AddDays(6);

            var logsInCurrentWeek = m.Logs.Where(log => log.Date >= startDateOfWeek && log.Date <= endDateOfWeek).ToList();

            var totalHoursSpent = logsInCurrentWeek.Sum(log => log.HoursSpent);

            return totalHoursSpent;
        }



        public async Task<DataResponse> SetModuleReminder(AddModuleReminder moduleReminder)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    var query = @"
                                INSERT INTO [dbo].[ModuleReminder]
                                (ModuleId, CreatedAt, UpdatedAt, DayOfTheWeek)
                                VALUES
                                (
                                @ModuleId, 
                                GETDATE(),
                                GETDATE(),
                                @DayOfTheWeek
                                )
                                SELECT SCOPE_IDENTITY() AS Id;
                                ";
                    var parameters = new
                    {
                        ModuleId = moduleReminder.ModuleId,
                        DayOfTheWeek = moduleReminder.DayOfTheWeek,
                    };

                    var response = await connection.QueryAsync<int>(query, parameters);

                    return new DataResponse
                    {
                        IsSuccsesful = true,
                        Message = "Succesfully added reminder",
                        Data = response
                    };
                }
            }
            catch (Exception ex)
            {
                return new DataResponse
                {
                    IsSuccsesful = false,
                    Message = "Failed to update. Please try again later",
                    Data = ex.Message
                };
            }
        }

        public async Task<DataResponse> GetDailyReminders(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    var todaysDate = DateTime.Now;
                    DayOfWeek dayOfWeek = todaysDate.DayOfWeek;
                    var dayName = dayOfWeek.ToString();
                    var query = @"
                            SELECT 
	                            mr.ModuleReminderId,
	                            mr.DayOfTheWeek,
	                            md.Name
                             FROM [dbo].[ModuleReminder] mr
                             LEFT JOIN [dbo].[Modules] md on mr.ModuleId = md.ModuleId
                             WHERE mr.DayOfTheWeek = @Day AND md.UserId = @UserId
                            ";
                    var parameters = new
                    {
                        Day = dayName,
                        UserId = userId
                    };


                    var response = await connection.QueryAsync<ModuleReminderDto>(query, parameters);
                    return new DataResponse
                    {
                        IsSuccsesful = true,
                        Message = "Succesfully retrieved module data",
                        Data = response
                    };

                }
            }
            catch (Exception ex)
            {
                return new DataResponse
                {
                    IsSuccsesful = false,
                    Message = "Failed to update. Please try again later",
                    Data = ex.Message
                };
            }
        }


    }
}
