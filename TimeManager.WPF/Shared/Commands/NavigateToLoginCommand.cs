using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.WPF.Shared.Forms.SignIn;
using TimeManager.WPF.State;

namespace TimeManager.WPF.Shared.Commands
{
    internal class NavigateToLoginCommand : AsyncCommandBase
    {

        private readonly NavigationState _navigationState;

        public NavigateToLoginCommand(NavigationState navigationState)
        {
            _navigationState = navigationState;
        }
        protected override async Task ExecuteAsync(object parameter)
        {
   
             _navigationState.CurrentViewModel = new SignInViewModel(_navigationState);
      
        }
    }
}
 