using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeManager.MainLibrary.Dtos;
using TimeManager.MainLibrary.Helpers;
using TimeManager.MainLibrary.Services;
using TimeManager.WPF.Shared.Forms.SignUp;

namespace TimeManager.WPF.Shared.Commands
{
    internal class RegisterCommand : AsyncCommandBase
    {
        private readonly SignUpFormViewModel _viewModel;

        public RegisterCommand(SignUpFormViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        protected override async Task ExecuteAsync(object parameter)
        {
            var newUser = new UserDto
            {
                FirstName = _viewModel.Name,
                LastName = _viewModel.Surname,
                Email = _viewModel.EmailOrUsername,
                Password = PasswordHashHelper.GeneratePasswordHash(_viewModel.Password)
            };

            var userServiceInstance = new UserService();

            var response = await userServiceInstance.InsertUsersIntoDb(newUser);

            if(response.IsSuccsesful)
            {
                MessageBox.Show(response.Message, "Successfully registers", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(response.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
