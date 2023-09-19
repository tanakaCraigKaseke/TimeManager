using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeManager.Data.Models;
using TimeManager.MainLibrary.Dtos;
using TimeManager.MainLibrary.Services;
using TimeManager.WPF.Commands;

namespace TimeManager.WPF.ViewModels
{
    public class DisplayVM
    {
        public ObservableCollection<Module> Modules { get; set; }
        public ICommand ViewModulesCommand { get; set; }

        public DisplayVM()
        {
            //Modules = new ObservableCollection<Module>();

            ViewModulesCommand = new RelayCommand(ViewModules, canViewModules);
        }

        private bool canViewModules(object arg)
        {
            throw new NotImplementedException();
        }



        // Method to open the Modules view
        private void ViewModules(object obj)
        {
           // var modulesView = new ModuleView(selectedSemester);
            //modulesView.Show();

            var display = new Display();
            display.Show();
            
        }
    }
}

