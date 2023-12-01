using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TimeManager.MainLibrary.Dtos;
using TimeManager.MainLibrary.Services;
using TimeManager.WPF.Commands;
using System.Reflection;
using TimeManager.WPF.ViewModels;
using System.ComponentModel;
using TimeManager.Data.Models;
using System.Runtime.CompilerServices;
using TimeManager.WPF.Validators;
using System.Runtime.InteropServices.WindowsRuntime;
using TimeManager.WPF.State;

namespace TimeManager.WPF.ViewModels
{
    // ViewModel class for adding modules and optionally creating new semesters in a WPF application.
    public class AddModuleVM : INotifyPropertyChanged, IDataErrorInfo
    {
        // Event to request the closing of the Add Module window.
        public event EventHandler RequestClose;

        // Properties for module information.
        public string Name { get; set; }
        public string Code { get; set; }
        public int Credits { get; set; }
        public int loggedInUserId { get; set; }
        public double Hours { get; set; }

        // Property to indicate whether to add the module to an existing semester.
        private bool _addToExistingSemester;

        // Property for credits input.
        private string _creditsInput;
        public string CreditsInput
        {
            get { return _creditsInput; }
            set { _creditsInput = value; OnPropertyChanged(); }
        }

        // Property for hours input.
        private string _hoursInput;
        public string HoursInput
        {
            get { return _hoursInput; }
            set { _hoursInput = value; }
        }

        // Property to specify whether to add the module to an existing semester or create a new one.
        public bool AddToExistingSemester
        {
            get { return _addToExistingSemester; }
            set
            {
                _addToExistingSemester = value;
                OnPropertyChanged();
            }
        }

        // Property to indicate whether to create a new semester (default is true).
        private bool _createNewSemester = true;
        public bool CreateNewSemester
        {
            get { return _createNewSemester; }
            set
            {
                _createNewSemester = value;
                OnPropertyChanged();
                UpdateFieldsWithValues(clear: false);
            }
        }

        // Property for the selected semester when adding the module to an existing semester.
        private SemesterDto _selectedSemester;
        public SemesterDto SelectedSemester
        {
            get { return _selectedSemester; }
            set
            {
                _selectedSemester = value;
                OnPropertyChanged();
                UpdateFieldsWithValues(clear: false);
            }
        }

        // Properties for creating a new semester.
        private string _newSemesterName;
        public string NewSemesterName
        {
            get { return _newSemesterName; }
            set { _newSemesterName = value; OnPropertyChanged(); }
        }

        private int _newSemesterWeeks;
        public int NewSemesterWeeks
        {
            get { return _newSemesterWeeks; }
            set { _newSemesterWeeks = value; OnPropertyChanged(); }
        }

        private DateTime _newSemesterDate = DateTime.Now;
        public DateTime NewSemesterStartDate
        {
            get { return _newSemesterDate; }
            set { _newSemesterDate = value; OnPropertyChanged(); }
        }

        // Error messages for validation.
        private string _codeErrorMessage;
        public string CodeErrorMessage
        {
            get { return _codeErrorMessage; }
            set { _codeErrorMessage = value; OnPropertyChanged(); }
        }

        private string _nameErrorMessage;
        public string NameErrorMessage
        {
            get { return _nameErrorMessage; }
            set { _nameErrorMessage = value; OnPropertyChanged(); }
        }

        private string _creditsErrorMessage;
        public string CreditsErrorMessage
        {
            get { return _creditsErrorMessage; }
            set { _creditsErrorMessage = value; OnPropertyChanged(); }
        }

        private string _newSemesterNameErrorMessage;
        public string NewSemesterNameErrorMessage
        {
            get { return _newSemesterNameErrorMessage; }
            set { _newSemesterNameErrorMessage = value; OnPropertyChanged(); }
        }

        private string _hoursErrorMessage;
        public string HoursErrorMessage
        {
            get { return _hoursErrorMessage; }
            set { _hoursErrorMessage = value; OnPropertyChanged(); }
        }

        private string _newSemesterStartDateErrorMessage;
        public string NewSemesterStartDateErrorMessage
        {
            get { return _newSemesterStartDateErrorMessage; }
            set { _newSemesterStartDateErrorMessage = value; OnPropertyChanged(); }
        }

        private string _newSemesterNumberOfWeeksErrorMessage;
        public string NewSemesterNumberOfWeeksErrorMessage
        {
            get { return _newSemesterNumberOfWeeksErrorMessage; }
            set { _newSemesterNumberOfWeeksErrorMessage = value; OnPropertyChanged(); }
        }

        // Property for input of the number of weeks in a new semester.
        private string _newSemesterWeeksInput;
        public string NewSemesterWeeksInput
        {
            get { return _newSemesterWeeksInput; }
            set { _newSemesterWeeksInput = value; OnPropertyChanged(); }
        }

        // Collection of existing semesters.
        public ObservableCollection<SemesterDto> ExistingSemesters { get; set; }

        // Command for submitting the module.
        public ICommand SubmitCommand { get; }

        // Command for canceling the operation.
        public ICommand CancelCommand { get; }

        // Implementing IDataErrorInfo interface for validation.
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Code))
                {
                    // Validate module code.
                    CodeErrorMessage = AddModuleValidator.ValidateModuleCode(this) ?? null;
                    return CodeErrorMessage;
                }

                if (columnName == nameof(Name))
                {
                    // Validate module name.
                    NameErrorMessage = AddModuleValidator.ValidateModuleName(this) ?? null;
                    return NameErrorMessage;
                }

                if (columnName == nameof(CreditsInput))
                {
                    // Validate credits input.
                    CreditsErrorMessage = AddModuleValidator.ValidateCredits(this) ?? null;
                    if (String.IsNullOrEmpty(CreditsErrorMessage))
                    {
                        Credits = int.Parse(CreditsInput);
                    }
                    return CreditsErrorMessage;
                }

                if (columnName == nameof(HoursInput))
                {
                    // Validate hours input.
                    HoursErrorMessage = AddModuleValidator.ValidateHours(this) ?? null;
                    if (String.IsNullOrEmpty(HoursErrorMessage))
                    {
                        Hours = double.Parse(HoursInput);
                    }
                }

                if (columnName == nameof(NewSemesterName))
                {
                    // Validate new semester name.
                    NewSemesterNameErrorMessage = AddModuleValidator.ValidateNewSemesterName(this) ?? null;
                }

                if (columnName == nameof(NewSemesterStartDate))
                {
                    // Validate new semester start date.
                    NewSemesterStartDateErrorMessage = AddModuleValidator.ValidateNewSemesterStartDate(this) ?? null;
                }

                if (columnName == nameof(NewSemesterWeeksInput))
                {
                    // Validate new semester number of weeks.
                    NewSemesterNumberOfWeeksErrorMessage = AddModuleValidator.ValidateNewSemesterWeek(this) ?? null;
                }

                return null;
            }
        }

        // Constructor.
        public AddModuleVM()
        {
            InitializeAsync();
            SubmitCommand = new RelayCommand(Submit, CanSubmit);
            CancelCommand = new RelayCommand(Cancel, CanCancel);
        }

        // Asynchronous initialization method.
        private async void InitializeAsync()
        {
            ExistingSemesters =  await getAllSemeters();

            // You can add any other initialization logic here.
        }
        // Retrieves all semesters for the user.
        private async Task< ObservableCollection<SemesterDto>> getAllSemeters()
        {
            var semesters = await  SemesterService.GetAllSemesterForUser(UserState.LoggedInUser.Id);
            if (semesters.IsSuccsesful)
            {
                var data = semesters.Data as IEnumerable<SemesterDto>;
                var collection = new ObservableCollection<SemesterDto>(data);
                return collection;
            }
            else
            {
                MessageBox.Show(semesters.Message);
                return null;
            }
        }

        // Submits the module and handles validation.
        private async void Submit(object parameter)
        {
            if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Code) || Credits < 0 || Hours < 0)
            {
                MessageBox.Show("You have some required fields missing for module information", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CreateNewSemester && (String.IsNullOrEmpty(NewSemesterName) || NewSemesterWeeks < 0))
            {
                MessageBox.Show("You have some required fields missing for semester information", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
              
            var newModule = new AddModuleDto
            {
                Name = Name,
                Code = Code,
                Credits = Credits,
                Hours = Hours,
                SemesterId = SelectedSemester?.Id ?? 0,
                UserId = UserState.LoggedInUser.Id,
                SemesterName = NewSemesterName,
                SemesterStartDate = NewSemesterStartDate,
                Weeks = NewSemesterWeeks,
                ShouldCreateNewSemester = CreateNewSemester
            };

            var data = await ModuleService.CreateModule(newModule);

            MessageBox.Show(data.Message);

            Cancel(null);
        }

        // Checks if the Submit command can be executed.
        private bool CanSubmit(object parameter)
        {
            return true;
        }

        // Checks if the Cancel command can be executed.
        private bool CanCancel(object parameter)
        {
            return true;
        }

        // Cancels the operation and requests to close the window.
        private void Cancel(object obj)
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        // Event for property change.
        public event PropertyChangedEventHandler PropertyChanged;

        // Helper method to invoke the PropertyChanged event.
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Updates fields with values based on the selected semester or clears them.
        private void UpdateFieldsWithValues(bool clear)
        {
            if (SelectedSemester != null && !clear)
            {
                NewSemesterName = SelectedSemester.Name;
                NewSemesterStartDate = SelectedSemester.StartDate;
                NewSemesterWeeks = SelectedSemester.Weeks;
            }

            if (clear)
            {
                NewSemesterName = String.Empty;
                NewSemesterStartDate = DateTime.Now;
                NewSemesterWeeks = 0;
            }
        }
    }
}
