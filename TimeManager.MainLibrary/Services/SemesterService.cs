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
    public class SemesterService : ISemesterService
    {
 
        public DataResponse CreateOrUpdateSemester(string name, DateTime StartDate, int numberOfWeeks)
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
