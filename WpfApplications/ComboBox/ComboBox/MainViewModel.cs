using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

using GoogleCloudExtension.Utils;

namespace ComboBox
{
    public class MainViewModel : ViewModelBase
    {

        public List<Class1> list1 = new List<Class1> { new Class1(), new Class1() };

        public List<Class1> ListItems => list1;

        private Class1 _selected;

        public Class1 Selected
        {
            get { return _selected; }
            set { SetValueAndRaise(ref _selected, value); }
        }
    }
}
