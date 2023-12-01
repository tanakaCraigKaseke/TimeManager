using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.WPF.Shared.Forms.SignIn;
using TimeManager.WPF.Shared.Forms.SignUp;
using TimeManager.WPF.State;
using TimeManager.WPF.ViewModels;

namespace TimeManager.WPF.Shared.Commands
{
    public class NavigateToSignUpCommand : AsyncCommandBase
    {
        private readonly NavigationState _navigationState;

        public NavigateToSignUpCommand(NavigationState navigationState)
        {
            _navigationState = navigationState;
        }
        protected override async Task ExecuteAsync(object parameter)
        {
            Console.WriteLine(_navigationState.CurrentViewModel);
   
            _navigationState.CurrentViewModel = new SignUpFormViewModel(_navigationState);


        }
    }
}  
