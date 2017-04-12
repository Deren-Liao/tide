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
using System.Windows.Media.Animation;

namespace DotsProgress
{
    public partial class MainWindow : Window
    {
        private const int DotsCount = 5;
        private const int Duration = 1000;     // in milliseconds

        public MainWindow()
        {
            InitializeComponent();
            _static.Columns = DotsCount;
            _progress.Columns = DotsCount;
            for (int i = 0; i < DotsCount; ++i)
            {
                var dot = new Ellipse { Style = (Style)TryFindResource("BlueDot"), Visibility = Visibility.Hidden };
                _progress.Children.Add(dot);
                _dots.Add(dot);

                var smallerDot = new Ellipse { Style = (Style)TryFindResource("SmallerDot") };
                _static.Children.Add(smallerDot);

            }

            this.DataContext = this;
            //_slider.
        }

        private List<Ellipse> _dots = new List<Ellipse>();

        public static DependencyProperty StepProperty =
            DependencyProperty.Register(
                nameof(Step),
                typeof(int),
                typeof(MainWindow),
                new FrameworkPropertyMetadata(-1, OnIntervalSecondsChanged));

        /// <summary>
        /// Gets or sets the command that responds to auto reload timer event.
        /// </summary>
        public int Step
        {
            get { return (int)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        private static void OnIntervalSecondsChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            // WPF framework makes the call and value type is int. 
            // Does not need to check the type.
            int step = (int)e.NewValue;
            MainWindow window = source as MainWindow;
            foreach (var dot in window._dots)
            {
                dot.Visibility = Visibility.Hidden;
            }
            if (step >= 0 && step < window._dots.Count)
            {
                window._dots[step].Visibility = Visibility.Visible;
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Storyboard s = (Storyboard)TryFindResource("mystoryboard");
            //s.Begin();

            ObjectAnimationUsingKeyFrames animation = new ObjectAnimationUsingKeyFrames();

            animation.Duration = TimeSpan.FromMilliseconds(Duration);
            animation.RepeatBehavior = RepeatBehavior.Forever;

            var frameDuration = Duration / _dots.Count;
            int framePoint = 0;

            for (int i = 0; i < DotsCount; ++i)
            {
                DiscreteObjectKeyFrame kf1 = new DiscreteObjectKeyFrame(
                    i,
                    new TimeSpan(0, 0, 0, 0, framePoint));
                animation.KeyFrames.Add(kf1);
                framePoint += frameDuration;
            }

            var _storyboard = new Storyboard();
            _storyboard.Children.Add(animation);

            //Storyboard.SetTargetName(animation, "_dotProgress4");
            //Storyboard.SetTargetProperty(animation, new PropertyPath("Visibility"));
            //_storyboard.Begin();
            this.BeginAnimation(MainWindow.StepProperty, animation);
        }
    }
}
