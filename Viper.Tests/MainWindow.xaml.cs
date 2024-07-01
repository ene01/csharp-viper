using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Viper.Game.Controls;
using Viper.Game.Animations;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Viper.Tests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextBlock _currentTestTB = new()
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            FontSize = 16,
            Opacity = 0.5,
            Foreground = new SolidColorBrush(Colors.White),
        };

        private Button _clearTestingSpace = new()
        {
            Height = 25,
            Margin = new Thickness(5, 5, 5, 5),
            Content = "Clear",
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        private Button _animationTest = new()
        {
            Height = 25,
            Margin = new Thickness(5, 5, 5, 5),
            Content = "Animation",
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        private Button _buttonTest = new()
        {
            Height = 25,
            Margin = new Thickness(5, 5, 5, 5),
            Content = "Button",
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        private Button _checkBoxTest = new()
        {
            Height = 25,
            Margin = new Thickness(5, 5, 5, 5),
            Content = "CheckBox",
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        private Button _comboBoxTest = new()
        {
            Height = 25,
            Margin = new Thickness(5, 5, 5, 5),
            Content = "ComboBox",
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        private GradientStop _testingBGGSOne = new()
        {
            Color = Colors.White,
            Offset = 0,
        };

        private GradientStop _testingBGGSTwo = new()
        {
            Color = Colors.DarkGray,
            Offset = 1,
        };

        private bool _canAnimateBG = true;

        private async void BackgroundAnimationLoop()
        {
            Random random = new();

            LinearGradientBrush backLinearGradient = new() { GradientStops = { _testingBGGSOne, _testingBGGSTwo }, StartPoint = new Point(0, 0), EndPoint = new Point(0, 1) };

            TestingMainGrid.Background = backLinearGradient;

            while (_canAnimateBG)
            {
                Animate.Color(_testingBGGSOne, GradientStop.ColorProperty, Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)), TimeSpan.FromSeconds(10), new QuadraticEase() { EasingMode = EasingMode.EaseInOut });
                Animate.Color(_testingBGGSTwo, GradientStop.ColorProperty, Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)), TimeSpan.FromSeconds(10), new QuadraticEase() { EasingMode = EasingMode.EaseInOut });
                Animate.Point(backLinearGradient, LinearGradientBrush.StartPointProperty, new Point(random.NextDouble(), random.NextDouble()), TimeSpan.FromSeconds(10), new QuadraticEase() { EasingMode = EasingMode.EaseInOut });
                Animate.Point(backLinearGradient, LinearGradientBrush.EndPointProperty, new Point(random.NextDouble(), random.NextDouble()), TimeSpan.FromSeconds(10), new QuadraticEase() { EasingMode = EasingMode.EaseInOut });

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            BackgroundAnimationLoop();

            DisposeLastTest("Nothing");

            TestingSpacesButtons.Children.Add(_clearTestingSpace);
            TestingSpacesButtons.Children.Add(_animationTest);
            TestingSpacesButtons.Children.Add(_buttonTest);
            TestingSpacesButtons.Children.Add(_checkBoxTest);
            TestingSpacesButtons.Children.Add(_comboBoxTest);

            _clearTestingSpace.Click += _clearTestingSpace_Click;
            _animationTest.Click += _animationTest_Click;
            _buttonTest.Click += _buttonTest_Click;
            _checkBoxTest.Click += _checkBoxTest_Click;
            _comboBoxTest.Click += _comboBoxTest_Click;
        }

        private void _clearTestingSpace_Click(object sender, RoutedEventArgs e)
        {
            DisposeLastTest("Nothing");
        }

        private void _animationTest_Click(object sender, RoutedEventArgs e)
        {
            DisposeLastTest("Animation");

            Random random = new();

            StackPanel rectangleColumn = new()
            {
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };

            Grid r1Box = new()
            {
                Height = 60,
                Width = 60,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };

            Rectangle r1 = new()
            {
                Height = 50,
                Width = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.White),
                Margin = new Thickness(10, 10, 0, 0),
                RenderTransform = new TranslateTransform(0, 0),
            };

            Rectangle r2 = new()
            {
                Height = 50,
                Width = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.White),
                Margin = new Thickness(10, 10, 0, 0),
                RenderTransform = new TranslateTransform(0, 0),
            };

            Rectangle r3 = new()
            {
                Height = 50,
                Width = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.White),
                Margin = new Thickness(10, 10, 0, 0),
                RenderTransform = new TranslateTransform(0, 0),
            };

            GradientStop gs1 = new()
            {
                Color = Colors.White,
                Offset = 0,
            };

            GradientStop gs2 = new()
            {
                Color = Colors.White,
                Offset = 1,
            };

            LinearGradientBrush lg = new() { GradientStops = { gs1, gs2 }, StartPoint = new Point(0, 0), EndPoint = new Point(0, 0) };

            Rectangle r4 = new()
            {
                Height = 50,
                Width = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = lg,
                Margin = new Thickness(10, 10, 0, 0),
                RenderTransform = new TranslateTransform(0, 0),
            };

            Rectangle r5 = new()
            {
                Height = 50,
                Width = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.White),
                Margin = new Thickness(10, 10, 0, 0),
                RenderTransform = new TranslateTransform(0, 0),
            };

            Grid r6Box = new()
            {
                Height = 120,
                Width = 60,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };

            Rectangle r6 = new()
            {
                Height = 50,
                Width = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.White),
                Margin = new Thickness(10, 10, 0, 0),
                RenderTransform = new TranslateTransform(0, 0),
            };

            Grid r7Box = new()
            {
                Height = 80,
                Width = 80,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };

            Rectangle r7 = new()
            {
                Height = 50,
                Width = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.White),
                Margin = new Thickness(10, 10, 0, 0),
                RenderTransform = new RotateTransform(0, 25, 25),
            };

            Grid skewBox = new()
            {
                Height = 120,
                Width = 300,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };

            Rectangle r8 = new()
            {
                Height = 50,
                Width = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.White),
                Margin = new Thickness(10, 10, 0, 0),
                RenderTransform = new SkewTransform(0, 0),
            };

            Rectangle r9 = new()
            {
                Height = 50,
                Width = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.White),
                Margin = new Thickness(100, 10, 0, 0),
                RenderTransform = new SkewTransform(0, 0),
            };

            async void Animations()
            {
                while (true)
                {
                    double timeSpan = 1000;

                    // Height and Width.
                    Animate.Double(r1, HeightProperty, 40, TimeSpan.FromMilliseconds(timeSpan), new BounceEase());
                    Animate.Double(r2, WidthProperty, 40, TimeSpan.FromMilliseconds(timeSpan), new BounceEase());

                    // Solid color.
                    Animate.Color(r3.Fill, SolidColorBrush.ColorProperty, Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)), TimeSpan.FromMilliseconds(timeSpan));

                    // GradientStops color and offsets
                    Animate.Color(gs1, GradientStop.ColorProperty, Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)), TimeSpan.FromMilliseconds(timeSpan));
                    Animate.Color(gs2, GradientStop.ColorProperty, Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)), TimeSpan.FromMilliseconds(timeSpan));
                    Animate.Double(gs1, GradientStop.OffsetProperty, random.NextDouble(), TimeSpan.FromMilliseconds(timeSpan));
                    Animate.Double(gs2, GradientStop.OffsetProperty, random.NextDouble(), TimeSpan.FromMilliseconds(timeSpan));
                    Animate.Point(lg, LinearGradientBrush.StartPointProperty, new Point(random.NextDouble(), random.NextDouble()), TimeSpan.FromMilliseconds(timeSpan));

                    // TranslateTransform
                    Animate.Double(r5.RenderTransform, TranslateTransform.XProperty, random.Next(10, 200), TimeSpan.FromMilliseconds(timeSpan), new QuadraticEase());
                    Animate.Double(r6.RenderTransform, TranslateTransform.YProperty, 50, TimeSpan.FromMilliseconds(timeSpan), new QuadraticEase());

                    // RotateTransform
                    Animate.Double(r7.RenderTransform, RotateTransform.AngleProperty, random.Next(0, 360), TimeSpan.FromMilliseconds(timeSpan), new QuadraticEase());

                    // SkewTransform
                    Animate.Double(r8.RenderTransform, SkewTransform.AngleXProperty, random.Next(-50, 50), TimeSpan.FromMilliseconds(timeSpan), new QuadraticEase());
                    Animate.Double(r9.RenderTransform, SkewTransform.AngleYProperty, random.Next(-50, 50), TimeSpan.FromMilliseconds(timeSpan), new QuadraticEase());

                    await Task.Delay(1100);

                    Animate.Double(r1, HeightProperty, 50, TimeSpan.FromMilliseconds(timeSpan), new BounceEase());
                    Animate.Double(r2, WidthProperty, 50, TimeSpan.FromMilliseconds(timeSpan), new BounceEase());
                    Animate.Color(r3.Fill, SolidColorBrush.ColorProperty, Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)), TimeSpan.FromMilliseconds(timeSpan));

                    Animate.Color(gs1, GradientStop.ColorProperty, Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)), TimeSpan.FromMilliseconds(timeSpan));
                    Animate.Color(gs2, GradientStop.ColorProperty, Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)), TimeSpan.FromMilliseconds(timeSpan));
                    Animate.Double(gs1, GradientStop.OffsetProperty, random.NextDouble(), TimeSpan.FromMilliseconds(timeSpan));
                    Animate.Double(gs2, GradientStop.OffsetProperty, random.NextDouble(), TimeSpan.FromMilliseconds(timeSpan));
                    Animate.Point(lg, LinearGradientBrush.StartPointProperty, new Point(random.NextDouble(), random.NextDouble()), TimeSpan.FromMilliseconds(timeSpan));

                    Animate.Double(r5.RenderTransform, TranslateTransform.XProperty, random.Next(10, 200), TimeSpan.FromMilliseconds(timeSpan), new QuadraticEase());
                    Animate.Double(r6.RenderTransform, TranslateTransform.YProperty, 0, TimeSpan.FromMilliseconds(timeSpan), new QuadraticEase());

                    Animate.Double(r7.RenderTransform, RotateTransform.AngleProperty, random.Next(0, 360), TimeSpan.FromMilliseconds(timeSpan), new QuadraticEase());

                    Animate.Double(r8.RenderTransform, SkewTransform.AngleXProperty, random.Next(-50, 50), TimeSpan.FromMilliseconds(timeSpan), new QuadraticEase());
                    Animate.Double(r9.RenderTransform, SkewTransform.AngleYProperty, random.Next(-50, 50), TimeSpan.FromMilliseconds(timeSpan), new QuadraticEase());

                    await Task.Delay(1100);
                }
            }

            Animations();

            rectangleColumn.Children.Add(new TextBlock() { Text = "Height", Foreground = new SolidColorBrush(Colors.White) });
            r1Box.Children.Add(r1);
            rectangleColumn.Children.Add(r1Box);

            rectangleColumn.Children.Add(new TextBlock() { Text = "Width", Foreground = new SolidColorBrush(Colors.White) });
            rectangleColumn.Children.Add(r2);

            rectangleColumn.Children.Add(new TextBlock() { Text = "Solid Color", Foreground = new SolidColorBrush(Colors.White) });
            rectangleColumn.Children.Add(r3);

            rectangleColumn.Children.Add(new TextBlock() { Text = "Gradient", Foreground = new SolidColorBrush(Colors.White) });
            rectangleColumn.Children.Add(r4);

            rectangleColumn.Children.Add(new TextBlock() { Text = "TranslateTransforms", Foreground = new SolidColorBrush(Colors.White) });
            rectangleColumn.Children.Add(r5);
            r6Box.Children.Add(r6);
            rectangleColumn.Children.Add(r6Box);

            rectangleColumn.Children.Add(new TextBlock() { Text = "RotateTransforms", Foreground = new SolidColorBrush(Colors.White) });
            r7Box.Children.Add(r7);
            rectangleColumn.Children.Add(r7Box);

            rectangleColumn.Children.Add(new TextBlock() { Text = "SkewTransforms", Foreground = new SolidColorBrush(Colors.White) });
            skewBox.Children.Add(r8);
            skewBox.Children.Add(r9);
            rectangleColumn.Children.Add(skewBox);


            TestingSpace.Children.Add(rectangleColumn);
        }

        private void _buttonTest_Click(object sender, RoutedEventArgs e)
        {
            DisposeLastTest("Button");

            StackPanel buttonColumn = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
            };

            ViperButton vButton1 = new()
            {
                Content = "Enabled",
                Margin = new Thickness(10, 10, 0, 0),
                DefaultColorAnimations = true,
            };

            ViperButton vButton2 = new()
            {
                Content = "Disabled",
                Margin = new Thickness(10, 10, 0, 0),
                DefaultColorAnimations = true,
                IsEnabled = false,
            };

            ViperButton vButton3 = new()
            {
                Content = "With click event",
                Margin = new Thickness(10, 10, 0, 0),
                DefaultColorAnimations = true,
            };

            ViperButton vButton4 = new()
            {
                Content = "Reading a long text is like trying to find a needle in a haystack, but the needle is the main point, and the haystack is endless paragraphs of rambling.",
                Margin = new Thickness(10, 10, 0, 0),
                DefaultColorAnimations = true,
            };

            ViperButton vButton5 = new()
            {
                Content = "Dynamically streched button",
                Margin = new Thickness(10, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                DefaultColorAnimations = true,
            };

            vButton2.Click += OnClick;

            vButton5.Click += OnClick;

            void OnClick(object sender, EventArgs e)
            {
                Debug.WriteLine("- An important message from a cool button: You clicked me!");

                if (!(sender as ViperButton).IsEnabled)
                {
                    Debug.WriteLine("- An important message from a disabled button: You fucked up");
                }
            }

            Debug.WriteLine($"Content: {vButton2.GetValue(ForegroundProperty)}");

            buttonColumn.Children.Add(vButton1);
            buttonColumn.Children.Add(vButton2);
            buttonColumn.Children.Add(vButton3);
            buttonColumn.Children.Add(vButton4);
            buttonColumn.Children.Add(vButton5);

            TestingSpace.Children.Add(buttonColumn);

        }

        private void _checkBoxTest_Click(object sender, RoutedEventArgs e)
        {
            DisposeLastTest("CheckBox");

            StackPanel checkColumn = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
            };

            ViperCheckBox check1 = new()
            {
                DefaultColorAnimations = true,
                Content = "Enabled check"
            };

            ViperCheckBox check2 = new()
            {
                DefaultColorAnimations = true,
                Content = "Disabled check",
                IsEnabled = false,
            };

            ViperCheckBox check3 = new()
            {
                DefaultColorAnimations = true,
                Content = "Long texts are like marathons for your eyes—by the end, you're not sure if you should get a medal or just a nap. 🏅😴",
            };

            ViperCheckBox check4 = new()
            {
                DefaultColorAnimations = true,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Content = "Dynamically stretched check box",
            };

            check1.StateChanged += (s, e) =>
            {
                Debug.WriteLine($"{e.State}");
            };

            checkColumn.Children.Add(check1);
            checkColumn.Children.Add(check2);
            checkColumn.Children.Add(check3);
            checkColumn.Children.Add(check4);

            TestingSpace.Children.Add(checkColumn);
        }

        private void _comboBoxTest_Click(object sender, RoutedEventArgs e)
        {
            DisposeLastTest("ComboBox");

            StackPanel comboColumn = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 10, 0, 0),
            };

            ViperComboBox combo1 = new()
            {
                DefaultColorAnimations = true,
                Margin = new Thickness(0, 10, 0, 0),
            };

            ViperComboBox combo2 = new()
            {
                IsEnabled = false,
                Content = "Disabled",
                DefaultColorAnimations = true,
                Margin = new Thickness(0, 10, 0, 0),
            };

            ViperComboBox combo3 = new()
            {
                DefaultColorAnimations = true,
                Content = "A very tall ComboBox",
                Height = 100,
                Margin = new Thickness(0, 10, 0, 0),
            };

            ViperComboBox combo4 = new()
            {
                DefaultColorAnimations = true,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Content = "Getting through a long text is like watching a movie with a 30-minute intro, an hour-long middle, and another 30 minutes of ending scenes—by the time it's over, you've forgotten what it was even about.",
                Margin = new Thickness(0, 10, 0, 0),
            };

            ViperComboBox combo5 = new()
            {
                DefaultColorAnimations = true,
                Content = "A tall item list.",
                Margin = new Thickness(0, 10, 0, 0),
                ItemContainerMaxHeight = 200,
            };

            // Array of number names
            string[] numberNames = { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten" };

            // Loop through numbers 1 to 10
            for (int i = 0; i < numberNames.Length; i++)
            {
                combo1.AddItem(numberNames[i]);
                combo2.AddItem(numberNames[i]);
                combo3.AddItem(numberNames[i]);
                combo4.AddItem(numberNames[i]);
                combo5.AddItem(numberNames[i]);
            }

            combo1.SetSelection(0);

            comboColumn.Children.Add(combo1);
            comboColumn.Children.Add(combo2);
            comboColumn.Children.Add(combo3);
            comboColumn.Children.Add(combo4);
            comboColumn.Children.Add(combo5);

            TestingSpace.Children.Add(comboColumn);
        }

        private void DisposeLastTest(string testingSpaceMessage)
        {
            TestingSpace.Children.Clear();
            TestingInteractions.Children.Clear();
            TestingSpace.Children.Add(_currentTestTB);
            _currentTestTB.Text = testingSpaceMessage;
        }
    }
}
