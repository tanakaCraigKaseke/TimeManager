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
    /// This class represents the window for managing modules in the application.
    /// </summary>
    public partial class Modules : Window
    {
        public Modules()
        {
            InitializeComponent();

            // Create an instance of the ModulesVM ViewModel.
            var viewModelInstance = new ModulesVM();

            // Set the DataContext of this window to the ViewModel instance.
            this.DataContext = viewModelInstance;
        }

        // Event handler for the selection changed event in the user list.
        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Add custom logic as needed.
        }

        // Event handler for text box text changed event.
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Add custom logic as needed.
        }

        // Event handler for the selection changed event in the user list (duplicate method).
        private void UserList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            // Add custom logic as needed.
        }
    }
}
