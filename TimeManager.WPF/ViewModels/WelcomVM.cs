using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.WPF.Shared;
using TimeManager.WPF.State;

namespace TimeManager.WPF.ViewModels
{
     public class WelcomVM : BaseViewModel
    {
        private readonly NavigationState _navigationState;

        public BaseViewModel CurrentViewModel => _navigationState.CurrentViewModel;

        public WelcomVM(NavigationState navigationState)
        {
            _navigationState = navigationState;
            _navigationState.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
