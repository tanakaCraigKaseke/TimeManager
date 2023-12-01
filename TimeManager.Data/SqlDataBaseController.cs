using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using TimeManager.Data.Models;

namespace TimeManager.Data
{
    public static class SqlDataBaseController
    {
        public const string ConnectionString = "Server=tcp:time-manager-server.database.windows.net,1433;Initial Catalog=timeManager;Persist Security Info=False;User ID=TimeManager;Password=formula.1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

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

        public static async Task<int> CreateModule(Module newModuleObjectModule)
        {
            int newId = 0;

            using (var connection = new SqlConnection(ConnectionString))
            {
               await connection.OpenAsync();

                var insertQuery = @"INSERT INTO [dbo].[Modules] (Name, Code, Credits, Hours, SemesterId, UserId)
                                    VALUES (@Name, @Code, @Credits, @Hours, @SemesterId, @UserId); 
                                    SELECT CAST(SCOPE_IDENTITY() AS INT)";

                newId = await connection.QuerySingleAsync<int>(insertQuery, newModuleObjectModule);

                return newId;
            }
        }

        public static async Task<int> CreateNewSemester(int userId, Semester newSemester)
        {
            int newId = 0;
            using(var connection = new SqlConnection(ConnectionString))
            {
                var insertQuery = @"
                      INSERT INTO [dbo].[Semesters] (Name,Weeks, StartDate,UserId)
                                        VALUES
                                        (
                                         @Name,
                                         @Weeks,
                                         @StartDate,
                                         @UserId
                                        ); SELECT CAST(SCOPE_IDENTITY() AS INT)
                                    ";
                await connection.OpenAsync();

                newId = await connection.QuerySingleAsync<int>(insertQuery, newSemester);

                return newId;
            }
        }

        public static async Task<User> FindUserById(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    var sqlQuery = @"
                                        SELECT Id, FirstName, LastName, PasswordHash, Email
                                        FROM Users WHERE Id = @UserId
                                    ";

                    var newUser = await connection.QueryFirstOrDefaultAsync<User>(sqlQuery, new { UserId = userId });

                    return newUser;
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL Server-specific exception, log the error, or take appropriate action.
                Console.WriteLine($"SQL Exception: {ex.Message}");
                throw; // Rethrow the exception if necessary.
            }
            catch (Exception ex)
            {
                // Handle other exceptions or log them.
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Rethrow the exception if necessary.
            }
        }

        public static async Task<Semester> GetSemesterById(int semesterId)
        {
            Semester semester = null;
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var sqlQuery = @"
                                SELECT
                                Id, Name, Weeks, StartDate, UserId
                                FROM [dbo].[Semesters]
                                WHERE Id = @SemesterId
                            ";
                 semester = await connection.QuerySingleAsync<Semester>(sqlQuery, semesterId);

                return semester;
            }
        }

        public static async Task< IEnumerable<Semester>> GetSemestersForLoggedInUser(int loggedInUserId)
        {
            IEnumerable<Semester> semesters = null;
            using(var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                var sqlQuery = @"
                                SELECT
                                Id, Name, Weeks, StartDate, UserId
                                FROM [dbo].[Semesters]
                                WHERE UserId = @LoggedInUserId
                                ";
                semesters = await connection.QueryAsync<Semester>(sqlQuery, new { LoggedInUserId = loggedInUserId});

                return semesters;
            }
        }
    }
}
