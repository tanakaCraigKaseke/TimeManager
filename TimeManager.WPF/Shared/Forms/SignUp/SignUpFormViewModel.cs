using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeManager.WPF.Shared.Commands;
using TimeManager.WPF.State;

namespace TimeManager.WPF.Shared.Forms.SignUp
{
    public class SignUpFormViewModel : BaseViewModel
    {
        private string name;
        private string surname;
        private string emailOrUsername;
        private string password;
        private readonly NavigationState _navigationState;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Surname
        {
            get { return surname; }
            set
            {
                if (surname != value)
                {
                    surname = value;
                    OnPropertyChanged(nameof(Surname));
                }
            }
        }

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


        public ICommand SignUpCommand { get; set; }
        public ICommand NavigateToLoginCommand { get; set; }


        public SignUpFormViewModel(NavigationState navigationState)
        {
            _navigationState = navigationState;
            SignUpCommand = new RegisterCommand(this);
            NavigateToLoginCommand = new NavigateToLoginCommand(_navigationState);
        }
 

    }
}


