using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

using GoogleCloudExtension.Utils;

namespace SelectControlTemplate
{
    public class MainViewModel : ViewModelBase
    {
        public bool InValidData { get; } = true;

        public MainViewModel()
        {
            string fmt = "https://console.cloud.google.com/code/develop/browse/{0}?project={1}";
            string url = String.Format(fmt, "name", "id");

        }
    }
}
