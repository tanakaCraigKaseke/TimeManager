using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TimeManager.MainLibrary.Dtos;
using TimeManager.MainLibrary.Services;
using TimeManager.WPF.Commands;
using TimeManager.WPF.State;

namespace TimeManager.WPF.ViewModels
{
    // ViewModel class for capturing hours worked on user modules in a WPF application.
    public class CaptureHoursVM : INotifyPropertyChanged, IDataErrorInfo
    {
        // Event to request the closing of the Capture Hours window.
        public event EventHandler RequestClose;

        // Command to log hours worked.
        public ICommand LogHoursCommand { get; set; }

        // Command to cancel the operation.
        public ICommand CancelCommand { get; set; }

        // ObservableCollection to store user modules.
        private ObservableCollection<UserModuleDto> _userModules;
        public ObservableCollection<UserModuleDto> UserModules
        {
            get { return _userModules; }
            set
            {
                _userModules = value;
                OnPropertyChanged();
            }
        }

        // Currently selected user module.
        private UserModuleDto _selectedModule;
        public UserModuleDto SelectedModule
        {
            get { return _selectedModule; }
            set
            {
                _selectedModule = value;
                OnPropertyChanged(nameof(SelectedModule));
                OnPropertyChanged(nameof(HoursRemainingText));
                OnPropertyChanged(nameof(SelectedModuleErrorMessage));
                CalculateRemainingHours();
            }
        }

        // Selected date for logging hours (default is current date).
        public DateTime SelectedDate { get; set; } = DateTime.Now;

        // Hours spent on the selected module.
        private double _hoursSpent = 0;
        public double HoursSpent
        {
            get { return _hoursSpent; }
            set
            {
                _hoursSpent = value;
                CalculateRemainingHours();
                OnPropertyChanged();
            }
        }

        // String property to hold user input for hours spent.
        private string _hoursSpentInput;
        public string HoursSpentInput
        {
            get { return _hoursSpentInput; }
            set { _hoursSpentInput = value; OnPropertyChanged(); }
        }

        // Error message for hours spent input validation.
        private string _hoursSpentInputError;
        public string HoursSpentInputError
        {
            get { return _hoursSpentInputError; }
            set { _hoursSpentInputError = value; OnPropertyChanged(); }
        }

        // Text displaying remaining hours for the selected module.
        private string _hoursRemainingText;
        public string HoursRemainingText
        {
            get { return _hoursRemainingText; }
            set { _hoursRemainingText = value; OnPropertyChanged(); }
        }

        // Error message for selected module validation.
        private string _selectedModuleErrorMessage;
        public string SelectedModuleErrorMessage
        {
            get { return _selectedModuleErrorMessage; }
            set { _selectedModuleErrorMessage = value; OnPropertyChanged(); }
        }

        // Implementing IDataErrorInfo interface for validation.
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(HoursSpentInput))
                {
                    // Validation logic for the HoursSpentInput property.
                    string errorMessage = String.Empty;
                    double hoursSpent;

                    if (SelectedModule == null)
                    {
                        errorMessage = "Please select a module first from the list";
                    }

                    if (!double.TryParse(HoursSpentInput, out hoursSpent))
                    {
                        errorMessage = "Please enter a valid number";
                        hoursSpent = 0;
                    }

                    if (hoursSpent < 0)
                    {
                        errorMessage = "Hours spent cannot be negative";
                    }

                    if (SelectedModule != null)
                    {
                        if (hoursSpent > SelectedModule.HoursRemaining)
                        {
                            errorMessage = "Hours spent cannot be more than the remaining hours";
                        }
                    }
                    HoursSpentInputError = errorMessage;
                    if (String.IsNullOrEmpty(errorMessage))
                    {
                        HoursSpent = hoursSpent;
                    }
                    else
                    {
                        HoursSpent = 0;
                    }

                    return errorMessage;
                }

                if (columnName == nameof(SelectedModule))
                {
                    // Validation logic for the SelectedModule property.
                    string errorMessage = String.Empty;
                    if (SelectedModule == null)
                    {
                        errorMessage = "Please select a module to record time";
                    }

                    SelectedModuleErrorMessage = errorMessage;
                    return errorMessage;
                }

                return null;
            }
        }


        public CaptureHoursVM()
        { 
            CaptureHoursVMS();
            LogHoursCommand = new RelayCommand(Submit, CanSubmit);
            CancelCommand = new RelayCommand(Cancel, CanCancel);
        }

        // Constructor for CaptureHoursVM.
        public async void CaptureHoursVMS()
        {
            // Initialize data and commands.
            UserModules =  await GetUsersModules();
        }  

        

        // Check if the Cancel command can be executed.
        private bool CanCancel(object arg)
        {
            return true;
        }

        // Execute the Cancel command.
        private void Cancel(object obj)
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        // Check if the LogHours command can be executed.
        private bool CanSubmit(object arg)
        {
            return true;
        }

        // Execute the LogHours command.
        private async void Submit(object obj)
        {
            // Validation and submission logic for logging hours.

            if (SelectedModule == null)
            {
                MessageBox.Show("Please select a module to continue", "Validation error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (SelectedDate == null)
            {
                MessageBox.Show("Please select a date", "Validation error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (HoursSpent <= 0)
            {
                MessageBox.Show("Please enter a valid value for the hours to continue", "Validation error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Extract necessary data for logging hours.
            var selectedModuleId = SelectedModule.ModuleId;
            var selectedDate = SelectedDate;
            var hoursSpent = HoursSpent;

            // Create a LogHoursDto object for the service call.
            var newHours = new LogHoursDto
            {
                ModuleId = selectedModuleId,
                Date = selectedDate,
                HoursSpent = hoursSpent,
                LoggedInUserId = 1
            };

            // Call the service to log hours and handle the response.
            var data = await CaptureHoursService.LogHours(newHours);

            if (data.IsSuccsesful)
            {
                MessageBox.Show(data.Message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                MessageBox.Show(data.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Retrieve user modules from the database.
        private async Task<ObservableCollection<UserModuleDto>> GetUsersModules()
        {
            var modulesFromDb = await ModuleService.ListAllUserModules(UserState.LoggedInUser.Id, DateTime.Now);
            if (modulesFromDb.IsSuccsesful)
            {
                var data = modulesFromDb.Data as IEnumerable<UserModuleDto>;
                var dataToCollection = new ObservableCollection<UserModuleDto>(data);
                return dataToCollection;
            }
            else
            {
                MessageBox.Show("Could not load the users modules");
                return null;
            }
        }

        // PropertyChanged event handler.
        public event PropertyChangedEventHandler PropertyChanged;

        // Helper method to invoke the PropertyChanged event.
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Calculate remaining hours based on selected module and hours spent.
        private void CalculateRemainingHours()
        {
            if (SelectedModule != null)
            {
                double remainingHours = SelectedModule.HoursRemaining - HoursSpent;
                HoursRemainingText = remainingHours.ToString();
            }
        }
    }
}  
