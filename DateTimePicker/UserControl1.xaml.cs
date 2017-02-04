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
using System.Diagnostics;

namespace DateTimePicker
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private readonly UserControl1ViewModel _viewModel = new UserControl1ViewModel();
        public UserControl1()
        {
            InitializeComponent();
            DataContext = _viewModel;

        }


        private void _getTime_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"Time control time is  {_viewModel.CurrentTime}");
        }
    }
}
