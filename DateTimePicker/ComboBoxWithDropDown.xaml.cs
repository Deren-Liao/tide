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
    /// Interaction logic for ComboBoxWithDropDown.xaml
    /// </summary>
    public partial class ComboBoxWithDropDown : UserControl
    {
        public ComboBoxWithDropDown()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets additional content for the UserControl
        /// </summary>
        public object AdditionalContent
        {
            get { return (object)GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }

        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register("AdditionalContent", typeof(object), typeof(ComboBoxWithDropDown),
              new PropertyMetadata(null));
    }
}
