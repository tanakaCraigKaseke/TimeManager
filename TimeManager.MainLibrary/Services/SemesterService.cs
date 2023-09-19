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
    public class SemesterService  
    {
        public static DataResponse GetAllSemesterForUser(int loggedInUserId)
        {
            var user = InMemoryDatabase.Users.FirstOrDefault(userdb=> userdb.Id == loggedInUserId);
            if(user == null)
            {
                return new DataResponse
                {
                    IsSuccsesful = false,
                    Message = "The specified user does not exists",
                };
            }

            var semester = InMemoryDatabase.Semesters.FindAll(dbSemester => dbSemester.UserId == loggedInUserId).Select(dbSemester => new SemesterDto
            {
                Id = dbSemester.Id,
                Name = dbSemester.Name,
                Weeks = dbSemester.Weeks,
                StartDate = dbSemester.StartDate,
                NumberOfModules = dbSemester.Modules.Count()
            });


            return new DataResponse
            {
                IsSuccsesful = true,
                Message = "Succesfully found the semesters",
                Data = semester
            };
        }
  
        public static DataResponse CreateOrUpdateSemester(string name, DateTime StartDate, int numberOfWeeks)
        {
            //check if a semester with the name already exists.
            var existingSemester = InMemoryDatabase.Semesters.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            //if it does not exists create a new one
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

            }else
            {
                // otherwise return the existing semester with a message.
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
