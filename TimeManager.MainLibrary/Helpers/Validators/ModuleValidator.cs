using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.MainLibrary.Helpers.Validators
{
    public class ModuleValidator : IDataErrorInfo, INotifyPropertyChanged
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Credits { get; set; }
        public double Hours { get; set; }
        public string this[string columnName]
        {
            get
            {
                string errorMsg = string.Empty;
                if(columnName.Equals("Code"))
                {
 
                    if(String.IsNullOrEmpty(Code))
                    {
                        errorMsg = "Module code is a mandatory field";
                    }else if(!IsModuleCodeValid(Code))
                    {
                        errorMsg = "The first 4 characters of the module code must be letter and last 4 must be a digit";
                    }
                }

                if(String.IsNullOrEmpty(this.Name))
                {
                    errorMsg = "Name is a mandatory field";
                }


                // make sure Credits is a digit
                if (!int.TryParse(this.Credits, out _))
                {
                    errorMsg = "Credits must always be number";
                    return errorMsg; // Successfully parsed as a number
                }

                return errorMsg;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        public bool IsModuleCodeValid(string moduleCode)
        {

            // Check the first 4 characters are letters
            for (int i = 0; i < 4; i++)
            {
                if (!char.IsLetter(moduleCode[i]))
                {
                    return false;
                }
            }

            // Check the last 4 characters are digits
            for (int i = 4; i < 8; i++)
            {
                if (!char.IsDigit(moduleCode[i]))
                {
                    return false;
                }
            }

            // If all checks pass, the module code is valid
            return true;
        }

        public string Error => throw new NotImplementedException();
    }
}
