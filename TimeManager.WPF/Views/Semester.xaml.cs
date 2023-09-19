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
using TimeManager.Data.Models;
using TimeManager.WPF.ViewModels;

namespace TimeManager.WPF.ViewModels
{
    /// <summary>
    /// Interaction logic for Semester.xaml
    /// </summary>
    public partial class Semester : Window
    {
        public Semester()
        {
            InitializeComponent();

            DisplayVM displayVM = new DisplayVM();
            this.DataContext = displayVM;

            var SemesterModelInstance = new SemesterVM();
            this.DataContext = SemesterModelInstance;
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserList.Items.Filter = FilterMethod();

        }

        private Predicate<object> FilterMethod()
        {
           // var semester = (Semester)obj;
           // semester.Name.Contains(FilterTextBox.Text);
            throw new NotImplementedException();
        }

       
           
 

        public void ModuleView()
        {
           
                DisplayVM displayVM = new DisplayVM();
                this.DataContext = displayVM;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddModule mod = new AddModule();
            mod.Show();
        }
    }
}
