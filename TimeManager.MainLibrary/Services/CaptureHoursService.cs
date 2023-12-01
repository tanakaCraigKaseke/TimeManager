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
    // Service class responsible for logging hours spent on user modules.
    public class CaptureHoursService: ICaptureHoursServices
    {
 
        public const string ConnectionString = "Server=tcp:time-manager-server.database.windows.net,1433;Initial Catalog=timeManager;Persist Security Info=False;User ID=TimeManager;Password=formula.1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // Logs hours spent on a user module.
        public   async Task<DataResponse> LogHours(LogHoursDto incomingHours)
        {
            try
            {
                // Find the user associated with the provided logged-in user ID.
 

                // Create a new user log entry.
                var newLog = new UserLog
                {
                   
                    Date = incomingHours.Date,
                    HoursSpent = incomingHours.HoursSpent,
                    ModuleId = incomingHours.ModuleId,
               
                };

                using(var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    var query = @"Insert into [dbo].[ModuleLogs]
                                (Date, HoursSpent, ModuleId)
                                VALUES
                                (
	                                @Date, @HoursSpent, @ModuleId
                                )";     

                    await connection.ExecuteAsync(query, newLog);
                }
 

                return new DataResponse
                {
                    IsSuccsesful = true,
                    Message = "Successfully logged hours"
                };
            }
            catch (Exception ex)
            {
                return new DataResponse
                {
                    IsSuccsesful = false,
                    Message = ex.Message
                };
            }
        }
    }
}
 