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
    public class SemesterVM
    {
        public ObservableCollection<SemesterDto> Semesters { get; set; }
        public ICommand ShowAddSemesterWindow { get; set; }



        public SemesterVM()
        {
            Semesters = getUsersSemesters(1);
            ShowAddSemesterWindow = new AddNewSemesterFormCommand();
        }

        private ObservableCollection<SemesterDto> getUsersSemesters(int loggedInUser)
        {
            var semesters = SemesterService.GetAllSemesterForUser(loggedInUser);
            if(semesters.IsSuccsesful)
            {
                var data = semesters.Data as IEnumerable<SemesterDto>;
                var response = new ObservableCollection<SemesterDto>(data);
                return response;
            }else
            {
                var response = new ObservableCollection<SemesterDto>();
                MessageBox.Show(semesters.Message);
                return response;
            }


        }
    }
}
