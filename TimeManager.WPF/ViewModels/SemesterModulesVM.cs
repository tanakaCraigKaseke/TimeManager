using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TimeManager.MainLibrary.Dtos;
using TimeManager.MainLibrary.Services;
using TimeManager.WPF.Commands;

namespace TimeManager.WPF.ViewModels
{
    public class SemesterModulesVM
    {
        public ObservableCollection<UserModuleDto> UserModules { get; set; }

        public ICommand ShowAddModuleWindow { get; set; }

        public SemesterModulesVM()
        {
            UserModules = GetUsersModules();
            ShowAddModuleWindow = new RelayCommand(ViewModules, canViewModules);
        }

        private bool canViewModules(object arg)
        {
            return true;
        }

        private void ViewModules(object obj)
        {
            var mainWindow = obj as Window;
            AddModule addModuleWindow = new AddModule();
            addModuleWindow.Owner = mainWindow;
            addModuleWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addModuleWindow.Show();
        }

        private ObservableCollection<UserModuleDto> GetUsersModules()
        {
            var modulesFromDb = ModuleService.ListAllUserModules(1, DateTime.Now);
            if (modulesFromDb.IsSuccsesful)
            {
                var data = modulesFromDb.Data as IEnumerable<UserModuleDto>;
                var dataToCollection = new ObservableCollection<UserModuleDto>(data);
                return dataToCollection;
            }else
            {
                MessageBox.Show("Could not load the users modules");
                return null;
            }
        }
    }
}
