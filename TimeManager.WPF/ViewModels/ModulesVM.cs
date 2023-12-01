using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TimeManager.MainLibrary.Dtos;
using TimeManager.MainLibrary.Services;
using TimeManager.WPF.Commands;
using TimeManager.WPF.State;

namespace TimeManager.WPF.ViewModels
{
    // The ViewModel class for managing modules in a WPF application.
    public class ModulesVM : INotifyPropertyChanged
    {
        // ObservableCollection to store user modules.
        private ObservableCollection<UserModuleDto> _userModules;
         
        public ObservableCollection<UserModuleDto> UserModules
        {
            get { return _userModules; }
            set
            {
                if (_userModules != value)
                {
                    _userModules = value;
                    OnPropertyChanged(nameof(UserModules));
                }
            }
        }  
         
        // ObservableCollection to store filtered modules for display.
        private ObservableCollection<UserModuleDto> _displayedData;
        public ObservableCollection<UserModuleDto> DisplayedData
        {
            get { return _displayedData; ; }
            set { _displayedData = value; OnPropertyChanged(); }
        }

        // ObservableCollection to store semesters.
        private ObservableCollection<SemesterDto> _semesters;

        public ObservableCollection<SemesterDto> Semesters
        {
            get { return _semesters; }
            set { _semesters = value; OnPropertyChanged(); }
        }

        // Currently selected semester.
        private SemesterDto _selectedSemester;

        public SemesterDto SelectedSemester
        {
            get { return _selectedSemester; }
            set
            {
                _selectedSemester = value;
                OnPropertyChanged();
                SemesterFilterApplied = true;
                ApplySemesterFilter();
            }
        }

        // Filter for module name.
        private string _moduleNameFilter;

        public string ModuleNameFilter
        {
            get { return _moduleNameFilter; }
            set { _moduleNameFilter = value; OnPropertyChanged(); }
        }

        // Filter for module code.
        private string _modelCodeFilter;

        public string ModuleCodeFilter
        {
            get { return _modelCodeFilter; }
            set { _modelCodeFilter = value; OnPropertyChanged(); }
        }

        // Flag to track if the semester filter is applied.
        private bool _semesterFilterApplied;

        public bool SemesterFilterApplied
        {
            get { return _semesterFilterApplied; }
            set { _semesterFilterApplied = value; OnPropertyChanged(); }
        }

        // Flag to track if a module is selected in the grid.
        private bool gridModuleSelected;

        public bool GridModuleSelected
        {
            get { return gridModuleSelected; }
            set { gridModuleSelected = value; OnPropertyChanged(); }
        }

        // Currently selected user module.
        private UserModuleDto _selectedModule;

        public UserModuleDto SelectedModule
        {
            get { return _selectedModule; }
            set { _selectedModule = value; OnPropertyChanged(); }
        }

        // Commands to trigger actions.
        public ICommand ShowAddModuleWindow { get; set; }
        public ICommand ShowCaptureHoursWindow { get; set; }
        public ICommand ClearFilters { get; set; }

        // Constructor for ModulesVM.
        public ModulesVM()
        {
            // Initialize data and commands.
         
            InitializeAsync();

            ShowAddModuleWindow = new RelayCommand(ViewModules, canViewModules);
            ShowCaptureHoursWindow = new RelayCommand(ViewCaptureHours, CanViewCaptureHours);
           
           
        }

        public async void InitializeAsync()
        {
            DisplayedData = await GetUsersModules();
            UserModules = await GetUsersModules();
            Semesters = await GetAllSemesters();
            await RefreshUserModules();
            
        }
        // Method to clear filters.
        private async Task ClearFiltersMethod(object obj)
        {
            await RefreshUserModules();
        }

        // Check if filters can be cleared.
        private bool CanClearFilter(object arg)
        {
            return true;
        }

        // Apply the semester filter to the displayed data.
        private void ApplySemesterFilter()
        {
            if (SelectedSemester != null)
            {
                var filtersDto = new FiltersDto
                {
                    Name = ModuleNameFilter,
                    Code = ModuleCodeFilter,
                    loggedInUserId = 1,
                    Date = DateTime.Now,
                    SemesterId = SelectedSemester.Id,
                };
                var dbData = ModuleService.FilterModules(filtersDto);
                if (dbData.IsSuccsesful)
                {
                    var data = dbData.Data as IEnumerable<UserModuleDto>;
                    var filteredResults = new ObservableCollection<UserModuleDto>(data);
                    DisplayedData = filteredResults;
                }
            }
        }

        // Get all semesters for the user.
        private async Task<ObservableCollection<SemesterDto>> GetAllSemesters()
        {
            var data = await SemesterService.GetAllSemesterForUser(UserState.LoggedInUser.Id);
            var response = data.Data as IEnumerable<SemesterDto>;
            var semesters = new ObservableCollection<SemesterDto>(response);

            return semesters;
        }

        // Check if the "Capture Hours" action can be performed.
        private bool CanViewCaptureHours(object arg)
        {
            return true;
        }

        // Open the "Capture Hours" window.
        private void ViewCaptureHours(object obj)
        {
            var mainWindow = obj as Window;
            CaptureHours logHoursScreen = new CaptureHours();
            logHoursScreen.Owner = mainWindow;
            logHoursScreen.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            logHoursScreen.Closed += WindowRefresher;
            logHoursScreen.Show();
        }

        // Check if the "View Modules" action can be performed.
        private bool canViewModules(object arg)
        {
            return true;
        }

        // Open the "Add Module" window.
        private void ViewModules(object obj)
        {
            var mainWindow = obj as Window;
            AddModule addModuleWindow = new AddModule();
            addModuleWindow.Owner = mainWindow;
            addModuleWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addModuleWindow.Closed += WindowRefresher;
            addModuleWindow.Show();
        }

        // Refresh the user modules when a window is closed.
        private void WindowRefresher(object sender, EventArgs e)
        {
            // Cast the sender to a Window if necessary
            var window = sender as Window;

            // Unsubscribe from the Closed event to prevent memory leaks (optional)
            window.Closed -= WindowRefresher;

            // Call GetUsersModules to refresh the user modules
            RefreshUserModules();
        }

        // Refresh user modules.
        private async Task RefreshUserModules()
        {
            var modulesFromDb = await ModuleService.ListAllUserModules(UserState.LoggedInUser.Id, DateTime.Now);
            if (modulesFromDb.IsSuccsesful)
            {
                var data = modulesFromDb.Data as IEnumerable<UserModuleDto>;
                DisplayedData = new ObservableCollection<UserModuleDto>(data);
                Semesters = await GetAllSemesters();
            }
            else
            {
                MessageBox.Show("Could not load the user's modules");
            }
        }
         
        // PropertyChanged event handler.
        public event PropertyChangedEventHandler PropertyChanged;

        // Helper method to invoke the PropertyChanged event.
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Get user modules from the database.
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
    }
}
