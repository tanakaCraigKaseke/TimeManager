using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TimeManager.WPF.ViewModels
{
    /// <summary>
    /// This class represents the window for adding a new module in the application.
    /// </summary>
    public partial class AddModule : Window
    {
        public AddModule()
        {
            InitializeComponent();

            // Create an instance of the AddModuleVM ViewModel.
            var viewModelInstance = new AddModuleVM();

            // Set the DataContext of this window to the ViewModel instance.
            this.DataContext = viewModelInstance;

            // Subscribe to the RequestClose event of the ViewModel to close the window.
            viewModelInstance.RequestClose += (sender, e) => Close();
        }
    }
}
