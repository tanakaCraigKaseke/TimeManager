using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.WPF.ViewModels;

namespace TimeManager.WPF.Validators
{
    // Validator class for validating input fields in the AddModuleVM ViewModel.
    public class AddModuleValidator
    {
        // Validates the module code.
        public static string ValidateModuleCode(AddModuleVM viewModelState)
        {
            var response = String.Empty;

            if (String.IsNullOrWhiteSpace(viewModelState.Code))
            {
                 response = "The module code is required";
             
            }

            if(!String.IsNullOrEmpty(viewModelState.Code))
            {
                if(viewModelState.Code.Length <= 4)
                {
                    for (int i = 0; i < viewModelState.Code.Length; i++)
                    {
                        if (!char.IsLetter(viewModelState.Code[i]))
                        {
                            response = "The module code must start with 4 letters e.g PROG6112";

                        }
                    }
                }


                if(viewModelState.Code.Length >= 4)
                {
                    for (int i = 4; i < viewModelState.Code.Length; i++)
                    {
                        if (!char.IsDigit(viewModelState.Code[i]))
                        {
                            response = "The module code must end with 4 number e.g PROG6112";
                        }
                    }
                }

            }

            return response;
        }

        // Validates the module credits input.
        internal static string ValidateCredits(AddModuleVM addModuleVM)
        {
            double credits;
            var response = String.Empty;
            if (!double.TryParse(addModuleVM.CreditsInput, out credits))
            {
                response = "Please enter a valid number for module credits";
                return response;
            }
            return response;
        }

        // Validates the module hours input.
        internal static string ValidateHours(AddModuleVM addModuleVM)
        {
            double hours;
            var response = String.Empty;
            if (!double.TryParse(addModuleVM.HoursInput, out hours))
            {
                response = "Please enter a valid number for module hours";
                return response;
            }
            return response;
        }

        // Validates the module name.
        internal static string ValidateModuleName(AddModuleVM addModuleVM)
        {
            if (String.IsNullOrWhiteSpace(addModuleVM.Name))
            {
                var response = "The module name is required";
                return response;
            }
            return null;
        }

        // Validates the new semester name (if adding to an existing semester).
        internal static string ValidateNewSemesterName(AddModuleVM addModuleVM)
        {
            var response = String.Empty;
            if (!addModuleVM.AddToExistingSemester)
            {
                if (String.IsNullOrWhiteSpace(addModuleVM.NewSemesterName))
                {
                    response = "The module name is required";
                    return response;
                }
                return response;
            }
            else
            {
                return null;
            }
        }

        // Validates the new semester start date (if adding to an existing semester).
        internal static string ValidateNewSemesterStartDate(AddModuleVM addModuleVM)
        {
            if (!addModuleVM.AddToExistingSemester)
            {
                return null;
            }

            return null;
        }

        // Validates the number of weeks in a new semester.
        internal static string ValidateNewSemesterWeek(AddModuleVM addModuleVM)
        {
            var response = String.Empty;
            if (!double.TryParse(addModuleVM.NewSemesterWeeksInput, out double week))
            {
                response = "Please enter a valid number for weeks";
                return response;
            }
            return response;
        }
    }
}
