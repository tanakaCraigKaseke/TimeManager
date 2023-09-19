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

namespace TimeManager.WPF.ViewModels
{
    public class AddModuleVM : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Credits { get; set; }
        public int loggedInUserId { get; set; }
        public double Hours { get; set; }
        public bool AddToExistingSemester { get; set; }
        public bool CreateNewSemester { get; set; }
        public ObservableCollection<SemesterDto> ExistingSemesters { get; set; }
        public int SelectedExistingSemester { get; set; } = 1;
        public string NewSemesterName { get; set; }
        public int NewSemesterWeeks { get; set; }
        public DateTime NewSemesterStartDate { get; set; }
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;


        public AddModuleVM()
        {
            ExistingSemesters = getAllSemeters();
            SubmitCommand = new RelayCommand(Submit, CanSubmit);
            CancelCommand = new RelayCommand(Cancel);
        }

        private ObservableCollection<SemesterDto> getAllSemeters()
        {
            var semesters = SemesterService.GetAllSemesterForUser(1);
            if (semesters.IsSuccsesful)
            {
                var data = semesters.Data as IEnumerable<SemesterDto>;
                var collection = new ObservableCollection<SemesterDto>(data);
                return collection;
            }else
            {
                MessageBox.Show(semesters.Message);
                return null;
            }
        }

        private void Submit(object parameter)
        {
            var newModule = new AddModuleDto
            {
                Name = Name,
                Code = Code,
                Credits = Credits,
                Hours = Hours,
                SemesterId = SelectedExistingSemester,
                UserId = 1,
                SemesterName = NewSemesterName,
                SemesterStartDate = NewSemesterStartDate,
                Weeks = NewSemesterWeeks,
                ShouldCreateNewSemester = CreateNewSemester
            };

           var data = ModuleService.CreateModule(newModule);
          
            MessageBox.Show(data.Message);
        }

        private bool CanSubmit(object parameter)
        {
            return true;
        }  

        private void Cancel(object parameter)
        {
            MessageBox.Show("Cancel clicked!");
        }
    }
}


 