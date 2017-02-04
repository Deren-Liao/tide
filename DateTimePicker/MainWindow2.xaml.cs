#region Namespaces
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
//using System.IO.Compression;
//using System.IO.IsolatedStorage;
//using System.IO.Packaging;
//using System.IO.Pipes;
//using System.IO.Ports;
using System.Linq;
//using System.Printing;
//using System.Printing.IndexedProperties;
//using System.Printing.Interop;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.AccessControl;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Composition;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Serialization.Advanced;
using System.Xml.Serialization.Configuration;
using System.Xml.XPath;
using System.Xml.Xsl;

#endregion

namespace ForumProjects
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    [TemplatePart(Name = "PART_HourEditor", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_MinuteEditor", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_SecondEditor", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_UpButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_DownButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_Time", Type = typeof(ComboBox))]
    public class TimePicker : Control
    {
        static TimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(typeof(TimePicker)));
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
        public int Hour
        {
            get { return (int)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
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
                textBox.TextChanged += TextBox_TextChanged;
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


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text.Length >= 2)
            {
                MoveToNextBox(sender);
            }
        }

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
        DependencyProperty.Register("TimeType", typeof(TimeType), typeof(TimePicker), new FrameworkPropertyMetadata(TimeType.AM));
        public static readonly DependencyProperty HourProperty =
        DependencyProperty.Register("Hour", typeof(int), typeof(TimePicker), new FrameworkPropertyMetadata(), HourValidateValue);
        public static readonly DependencyProperty MinuteProperty =
        DependencyProperty.Register("Minute", typeof(int), typeof(TimePicker), new FrameworkPropertyMetadata(), MinuteValidateValue);
        public static readonly DependencyProperty SecondProperty =
        DependencyProperty.Register("Second", typeof(int), typeof(TimePicker), new FrameworkPropertyMetadata(), SecondValidateValue);
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

    [Serializable]
    public enum TimeType { AM, PM }
}
