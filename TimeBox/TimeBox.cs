using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace TimeBox
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TimeBox"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TimeBox;assembly=TimeBox"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    //public class TimeBox : Control
    //{
    //    static TimeBox()
    //    {
    //        DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeBox), new FrameworkPropertyMetadata(typeof(TimeBox)));
    //    }
    //}

    [Serializable]
    public enum TimeType { AM, PM }

    [TemplatePart(Name = "PART_HourEditor", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_MinuteEditor", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_SecondEditor", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_UpButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_DownButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_Time", Type = typeof(ComboBox))]
    public class TimeBox : Control
    {
        static TimeBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeBox), new FrameworkPropertyMetadata(typeof(TimeBox)));
        }

        static bool HourValidateValue(object value)
        {
            int t = (int)value;
            return !(t < 0 || t > 12);
        }
        static bool MinuteValidateValue(object value)
        {
            int t = (int)value;
            return !(t < 0 || t > 60);
        }
        static bool SecondValidateValue(object value)
        {
            int t = (int)value;
            return !(t < 0 || t > 60);
        }

        public DateTime Time
        {
            get
            {
                return new DateTime(0, 0, 0, Hour, Minute, Second);
            }

            set
            {
                Hour = value.Hour;
                Minute = value.Minute;
                Second = value.Second;
            }
        }

        public int Hour
        {
            get
            {
                var hour = (int)GetValue(HourProperty);
                return TimeType == TimeType.AM ? hour : (hour % 12) + 12;
            }

            set
            {
                TimeType = TimeType.AM;
                var h = value;
                if (h >= 12)
                {
                    h = value - 12;
                    TimeType = TimeType.PM;
                }
                if (h == 0)
                {
                    h = 12;
                }

                SetValue(HourProperty, h);
            }
        }
        public int Minute
        {
            get { return (int)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); }
        }
        public int Second
        {
            get { return (int)GetValue(SecondProperty); }
            set { SetValue(SecondProperty, value); }
        }
        public TimeType TimeType
        {
            get { return (TimeType)GetValue(TimeTypeProperty); }
            set { SetValue(TimeTypeProperty, value); }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.upButton = this.Template.FindName("PART_UpButton", this) as RepeatButton;
            this.downButton = this.Template.FindName("PART_DownButton", this) as RepeatButton;
            this.hourEditor = this.Template.FindName("PART_HourEditor", this) as TextBox;
            this.minuteEditor = this.Template.FindName("PART_MinuteEditor", this) as TextBox;
            this.secondEditor = this.Template.FindName("PART_SecondEditor", this) as TextBox;
            this.upButton.Click += RepeatButton_Click;
            this.downButton.Click += RepeatButton_Click;
            foreach (var textBox in TextBoxParts)
            {
                textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                textBox.PreviewTextInput += TextBox_PreviewTextInput;
                //textBox.TextChanged += TextBox_TextChanged;
            }
        }


        private int SenderIndex(object sender)
        {
            for (int i = 0; i < TextBoxParts.Length; ++i)
            {
                if (TextBoxParts[i] == sender as TextBox)
                {
                    return i;
                }
            }

            return -1;
        }

        private void MoveToNextBox(object sender)
        {
            int idx = SenderIndex(sender);
            if (idx >= 0 && idx < 2)
            {
                TextBoxParts[idx + 1].Focus();
            }
        }

        private TextBox[] TextBoxParts => new TextBox[] { hourEditor, minuteEditor, secondEditor };

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsTextAllowed(e.Text))
            {
                e.Handled = true;
                return;
            }
        }


        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var textBox = sender as TextBox;
        //    if (textBox.Text.Length >= 2)
        //    {
        //        MoveToNextBox(sender);
        //    }
        //}

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(hourEditor.IsFocused || minuteEditor.IsFocused || secondEditor.IsFocused))
            {
                hourEditor.Focus();
            }

            DependencyProperty dp = null;
            int maxValue = 60;
            if (this.hourEditor.IsFocused)
            {
                dp = HourProperty;
                maxValue = 12;
            }
            if (this.minuteEditor.IsFocused) dp = MinuteProperty;
            if (this.secondEditor.IsFocused) dp = SecondProperty;
            if (dp == null) return;
            int value = (int)this.GetValue(dp);
            if (e.Source == this.upButton)
                ++value;
            else
                --value;
            if (value < 0 || value > maxValue) return;
            this.SetValue(dp, value);
        }
        public static readonly DependencyProperty TimeTypeProperty =
        DependencyProperty.Register("TimeType", typeof(TimeType), typeof(TimeBox), new FrameworkPropertyMetadata(TimeType.AM));
        public static readonly DependencyProperty HourProperty =
        DependencyProperty.Register("Hour", typeof(int), typeof(TimeBox), new FrameworkPropertyMetadata(), HourValidateValue);
        public static readonly DependencyProperty MinuteProperty =
        DependencyProperty.Register("Minute", typeof(int), typeof(TimeBox), new FrameworkPropertyMetadata(), MinuteValidateValue);
        public static readonly DependencyProperty SecondProperty =
        DependencyProperty.Register("Second", typeof(int), typeof(TimeBox), new FrameworkPropertyMetadata(), SecondValidateValue);
        private RepeatButton upButton, downButton;
        private TextBox hourEditor, minuteEditor, secondEditor;

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!e.IsDown)
            {
                return;
            }

            var textBox = sender as TextBox;
            int index = SenderIndex(sender);
            if (index < 0)
            {
                return;
            }

            if (e.Key == Key.Down || e.Key == Key.Enter && index < 2)
            {
                TextBoxParts[index + 1].Focus();
            }

            if (e.Key == Key.Up && index > 0)
            {
                TextBoxParts[index - 1].Focus();
            }

            if (e.Key == Key.Left && textBox.CaretIndex == 0)
            {
                if (index > 0)
                {
                    TextBoxParts[index - 1].Focus();
                }
            }

            if (e.Key == Key.Right && textBox.CaretIndex == textBox.Text.Length)
            {
                if (index < 2)
                {
                    TextBoxParts[index + 1].Focus();
                }
            }
        }
    }

}
