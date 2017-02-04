using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTimePicker
{
    class LogDateTimePickerViewModel : ViewModelBase
    {
        public class RadioClass
        {
            public string Header { get; set; }
            public bool CheckedProperty { get; set; }
        }

        public List<RadioClass> Radio
        {
            get
            {
                List<RadioClass> list = new List<RadioClass>();
                list.Add(new RadioClass { Header = "Newest Log First", CheckedProperty = true });
                list.Add(new RadioClass { Header = "Oldest Log First", CheckedProperty = false });
                return list;
            }
        }

        public string ComboBoxText => "Jump To Date";

        private DateTime _date = DateTime.Now.AddDays(-30);

        public DateTime Date
        {
            get
            {
                return _date.Date;
            }

            set
            {
                var date = value + _date.TimeOfDay;
                SetValueAndRaise(ref _date, date);
            }
        }

        public string Time
        {
            get
            {
                return _date.ToString("t");
            }

            set
            {
                // Do nothing for now.
            }
        }
    }
}
