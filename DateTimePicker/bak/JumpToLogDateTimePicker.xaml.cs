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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DateTimePicker
{
    /// <summary>
    /// Interaction logic for JumpToLogDateTimePicker.xaml
    /// </summary>
    public partial class JumpToLogDateTimePicker : UserControl
    {
        public JumpToLogDateTimePicker()
        {
            InitializeComponent();
            comboPickTime.SelectedIndex = 1;
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            comboPickTime.IsDropDownOpen = false;
        }
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            comboPickTime.IsDropDownOpen = false;
        }

        private void UniformGrid_Click(object sender, RoutedEventArgs e)
        {
            comboPickTime.IsDropDownOpen = false;
        }
    }
}
