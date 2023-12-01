using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.Data.Models
{
    public  class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Credits { get; set; }
        public double Hours { get; set; }
        public int SemesterId { get; set; }
        public Semester Semester { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<UserLog> Logs { get; set; }

       

        public double SelfStudyHours
        {
            get 
            { 
                return CalculateSelfStudyHours(this);
            }
        
        }


        private static double CalculateSelfStudyHours(Module m)
        {
            var response = ((m.Credits * 10) / m.Semester.Weeks) - m.Hours;
            return response;
        }

    }
}
