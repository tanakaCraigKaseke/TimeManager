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
    /// This class represents the window for capturing hours worked on a module in the application.
    /// </summary>
    public partial class CaptureHours : Window
    {
        public CaptureHours()
        {
            InitializeComponent();

            // Create an instance of the CaptureHoursVM ViewModel.
            var VMInstance = new CaptureHoursVM();

            // Set the DataContext of this window to the ViewModel instance.
            DataContext = VMInstance;

            // Subscribe to the RequestClose event of the ViewModel to close the window.
            VMInstance.RequestClose += (sender, e) => Close();
        }
    }
}
