using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleCloudExtension.Utils;
using System.Diagnostics;

namespace DateTimePicker
{
    public class UserControl1ViewModel : Model
    {
        private int _intTest;
        public int IntTest
        {
            get { return _intTest; }
            set {
                SetValueAndRaise(ref _intTest, value);
                Debug.WriteLine($"set IntTest {value}");
            }
        }

        private TimeSpan _time;
        public TimeSpan CurrentTime
        {
            get { return _time; }
            set { SetValueAndRaise(ref _time, value); }
        }

        public UserControl1ViewModel()
        {
            _time = default(TimeSpan);
            //_time = DateTime.Now - DateTime.MinValue;
            _time = DateTime.Now.TimeOfDay - TimeSpan.FromHours(1);
            _intTest = 6;
        }

        private int _theSecond = 30;
        public int TheSecond
        {
            get { return _theSecond; }
            set { SetValueAndRaise(ref _theSecond, value); }
        }
    }
}
