using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.MainLibrary.Dtos;
using TimeManager.MainLibrary.Helpers;
using TimeManager.Data;
using TimeManager.Data.Models;
using TimeManager.MainLibrary.interfaces;

namespace TimeManager.MainLibrary.Services
{
    //manage user operations such as login etc
    public class UserService: IUserService
    {
        private readonly string _connectionString = DbController.ConnectionString;

        public async Task<DataResponse> InsertUsersIntoDb(UserDto newUser)
        {
            try
            {
                // Check if the username exists
                if (await UsernameExists(newUser.Email))
                {
                    // Username already exists, handle this case as needed
                    return new DataResponse
                    {
                        IsSuccsesful = false,
                        Message = "The username already exists, please login to access account",
                        Data = null
                    };
                }

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = "INSERT INTO Users (FirstName, LastName, Password, Email) " +
                                              "VALUES (@Name, @Surname, @PasswordHash, @UsernameOrEmail)";

                        command.Parameters.AddWithValue("@Name", newUser.FirstName);
                        command.Parameters.AddWithValue("@Surname", newUser.LastName);
                        command.Parameters.AddWithValue("@PasswordHash", newUser.Password);
                        command.Parameters.AddWithValue("@UsernameOrEmail", newUser.Email);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                return new DataResponse
                {
                    IsSuccsesful = true,
                    Message = "Succesfully registered, please sign in to access your account",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new DataResponse
                {
                    IsSuccsesful = false,
                    Message = "Something went wrong while registering, please try again later",
                    Data = ex.Message
                };
            }
        }

        public async Task<bool> UsernameExists(string username)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = "SELECT COUNT(*) FROM Users WHERE UsernameOrEmail = @Username";
                        command.Parameters.AddWithValue("@Username", username);

                        var result = await command.ExecuteScalarAsync();
                        return (int)result > 0;
                    }
                }
            } 
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<DataResponse> UserLogin(LoginDto login)
        {
            try
            {
                UserResponseDto user = null;
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = "SELECT Id, FirstName, LastName, Password, Email FROM Users WHERE Email = @UsernameOrEmail";
                        command.Parameters.AddWithValue("@UsernameOrEmail", login.Username);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())                     
                            {
                                 user = new UserResponseDto
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Password = reader.GetString(reader.GetOrdinal("Password")),
                                    Email = reader.GetString(reader.GetOrdinal("Email"))
                                };
                            }
                            if (user != null)
                            {
                                // Verify the provided password against the stored password hash using the constant salt
                                bool passwordMatch = PasswordHashHelper.VerifyPassword(login.Password, user.Password);

                                if (passwordMatch)
                                {
                                    return  new DataResponse
                                    {
                                        IsSuccsesful = true,
                                        Message = "Successfully signed in",
                                        Data = user
                                    }; 
                                }
                            }

                            return   new DataResponse
                            {
                                IsSuccsesful = false,
                                Message = "Invalid username / password",
                                Data = null
                            };
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                return new DataResponse
                {
                    IsSuccsesful = false,
                    Message = "Something went wrong, please try again later",
                    Data = ex.Message
                };
            }
             
        }  

    }  
}
 