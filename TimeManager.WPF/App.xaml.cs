using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TimeManager.WPF.Shared.Forms.SignIn;
using TimeManager.WPF.State;
using TimeManager.WPF.ViewModels;
using TimeManager.WPF.Views;

namespace TimeManager.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var navigationState = new NavigationState();
            navigationState.CurrentViewModel = new SignInViewModel(navigationState);
            MainWindow = new welcome()
            {
                DataContext = new WelcomVM(navigationState)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
