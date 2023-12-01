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
using Dapper;
using TimeManager.MainLibrary.interfaces;

namespace TimeManager.MainLibrary.Services
{
    // Service class responsible for managing semesters and related operations.
    public class SemesterService : ISemesterService
    {
        public const string ConnectionString = "Server=tcp:time-manager-server.database.windows.net,1433;Initial Catalog=timeManager;Persist Security Info=False;User ID=TimeManager;Password=formula.1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // Retrieves all semesters associated with a specific user.
        public  async Task<DataResponse> GetAllSemesterForUser(int loggedInUserId)
        {
            var user = await SqlDataBaseController.FindUserById(loggedInUserId);

            if (user == null)
            {
                return new DataResponse
                {
                    IsSuccsesful = false,
                    Message = "The specified user does not exist",
                };
            }  
 
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                var sqlQuery = @"
                                SELECT
                                Id, Name, Weeks, StartDate, UserId
                                FROM [dbo].[Semesters]
                                WHERE UserId = @LoggedInUserId
                                ";
               var  semesters = await connection.QueryAsync<SemesterDto>(sqlQuery, new { LoggedInUserId = loggedInUserId });

                return new DataResponse
                {
                    IsSuccsesful = true,
                    Message = "Successfully found the semesters",
                    Data = semesters
                };
            }




        }

        // Creates or updates a semester based on the provided parameters.
        public   DataResponse CreateOrUpdateSemester(string name, DateTime StartDate, int numberOfWeeks)
        {
            // Check if a semester with the same name already exists.
            var existingSemester = InMemoryDatabase.Semesters.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            // If it does not exist, create a new one.
            if (existingSemester == null)
            {
                var newId = InMemoryDatabase.Semesters.Count + 1;
                var newSemester = new Semester
                {
                    Id = newId,
                    Name = name,
                    StartDate = StartDate,
                    Weeks = numberOfWeeks,
                };

                InMemoryDatabase.Semesters.Add(newSemester);

                return new DataResponse
                {
                    Data = newSemester,
                    Message = "Successfully created a semester",
                    IsSuccsesful = true
                };
            }
            else
            {  
                // Otherwise, update the existing semester with the provided values.
                existingSemester.Weeks = numberOfWeeks;
                existingSemester.StartDate = StartDate;
                existingSemester.Name = name;

                return new DataResponse
                {
                    Data = existingSemester,
                    IsSuccsesful = true,
                    Message = "A semester with the same name already exists"
                };
            }
        }
    }
}
