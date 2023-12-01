using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeManager.MainLibrary.Dtos;
using TimeManager.MainLibrary.Services;
using TimeManager.WPF.Shared.Forms.SignIn;
using TimeManager.WPF.State;
using TimeManager.WPF.ViewModels;

namespace TimeManager.WPF.Shared.Commands
{
    public class SignInCommand: AsyncCommandBase
    {
        private SignInViewModel signInViewModel;
        private NavigationState navigationState;

        public SignInCommand(SignInViewModel signInViewModel, NavigationState navigationState)
        {
            this.signInViewModel = signInViewModel;
            this.navigationState = navigationState;
        }

        protected override async Task ExecuteAsync(object parameter)
        {

            if(String.IsNullOrEmpty(this.signInViewModel.Password))
            {
                MessageBox.Show("Please enter a paswword to continue", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrEmpty(this.signInViewModel.EmailOrUsername))
            {
                MessageBox.Show("Please enter a username or email to continue", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newloginResponse = new LoginDto
            {
                Password = this.signInViewModel.Password,
                Username = this.signInViewModel.EmailOrUsername
            };
            var userServiceInstance= new UserService();
            var response = await userServiceInstance.UserLogin(newloginResponse);
            if(response.IsSuccsesful)
            {
                var result =   MessageBox.Show(response.Message,"Success", MessageBoxButton.OK, MessageBoxImage.Information);

                if (result == MessageBoxResult.OK)
                {
                    UserState.IsLoggedIn = true;
                    var data = response.Data as UserResponseDto;
                    if (data != null)
                    {
                        UserState.LoggedInUser = data;
                        Modules modules = new Modules();
                        modules.Show();
                    }
                } 
            }
            else
            {
                MessageBox.Show(response.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
