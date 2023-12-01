using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeManager.WPF.Shared.Commands;
using TimeManager.WPF.State;

namespace TimeManager.WPF.Shared.Forms.SignIn
{
    public class SignInViewModel : BaseViewModel
    {
        private string emailOrUsername;
        private string password;
        private readonly NavigationState _navigationState;

        public string EmailOrUsername
        {
            get { return emailOrUsername; }
            set
            {
                if (emailOrUsername != value)
                {
                    emailOrUsername = value;
                    OnPropertyChanged(nameof(EmailOrUsername));
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }


        public ICommand NavigateToSignUpCommand { get; set; }

        public ICommand SignInCommand { get; set; }


        public BaseViewModel CurrentViewModel => _navigationState.CurrentViewModel;

        public SignInViewModel(NavigationState navigationState)
        {
            _navigationState = navigationState;
            NavigateToSignUpCommand = new NavigateToSignUpCommand(_navigationState);
            SignInCommand = new SignInCommand(this,_navigationState);
        }

         

    }
}
