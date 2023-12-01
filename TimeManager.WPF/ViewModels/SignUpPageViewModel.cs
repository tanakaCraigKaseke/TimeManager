using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.WPF.Shared;
using TimeManager.WPF.State;

namespace TimeManager.WPF.ViewModels
{
    public class SignUpPageViewModel : BaseViewModel
    {
        private readonly NavigationState _navigationState;

        public SignUpPageViewModel(NavigationState navigationState)
        {
            _navigationState = navigationState;
        }
    }
}
