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


using System.Windows.Controls.Primitives;

using GoogleCloudExtension.Utils;

namespace DateTimePicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Content = "156";
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate window
            var dialogBox = new Window1();

            // Show window modally
            // NOTE: Returns only when window is closed
            Nullable<bool> dialogResult = dialogBox.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // calendarBox.
            comboPickTime.IsDropDownOpen = false;
            
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            comboPickTime.IsDropDownOpen = false;
        }

        private int clicked_toogle = 0;

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            toggle.Text = (++clicked_toogle).ToString();
        }

        private int clicked = 0;
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            refreshClickText.Text = (++clicked).ToString();
        }

        private const string RefreshImagePath = "Resources/refresh.png";
        private const string RefreshMouseOverImagePath = "Resources/refresh-mouseover.png";
        private const string RefreshMouseDownImagePath = "Resources/refresh-mouse-down.png";
        private static readonly Lazy<ImageSource> s_refreshImage =
            new Lazy<ImageSource>(() => ResourceUtils.LoadImage(RefreshImagePath));
        private static readonly Lazy<ImageSource> s_refreshMouseOverImage =
            new Lazy<ImageSource>(() => ResourceUtils.LoadImage(RefreshMouseOverImagePath));
        private static readonly Lazy<ImageSource> s_refreshMouseDownImage =
            new Lazy<ImageSource>(() => ResourceUtils.LoadImage(RefreshMouseDownImagePath));

        private void btnNext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                nextImage.Source = s_refreshMouseDownImage.Value;
            }
        }

        private void btnNext_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                nextImage.Source = s_refreshMouseOverImage.Value;
            }
        }

        private void btnNext_MouseLeave(object sender, MouseEventArgs e)
        {
            nextImage.Source = s_refreshImage.Value;
        }

        private void btnNext_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                nextImage.Source = s_refreshMouseOverImage.Value;
            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                nextImage.Source = s_refreshMouseDownImage.Value;
            }

        }

        private void ComboBox_Loaded(Object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var comboBoxTemplate = comboBox.Template;
            var toggleButton = comboBoxTemplate.FindName("toggleButton", comboBox) as ToggleButton;
            var toggleButtonTemplate = toggleButton.Template;
            var border = toggleButtonTemplate.FindName("templateRoot", toggleButton) as Border;

            border.Background = new SolidColorBrush(Colors.White);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button is clicked");
            
        }
    }
}
