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
using Viper.Game.Controls.Individual;
using Viper.Game.Controls.Panels;
using Viper.Game.Animations;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;
using Viper.Game.Events;
using Viper.Game.Controls.Gameplay;

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

        private Button _sliderTest = new()
        {
            Height = 25,
            Margin = new Thickness(5, 5, 5, 5),
            Content = "Slider",
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        private Button _unlimitedSelectorTest = new()
        {
            Height = 25,
            Margin = new Thickness(5, 5, 5, 5),
            Content = "UnlimitedSelector",
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        private Button _searchSPTest = new()
        {
            Height = 25,
            Margin = new Thickness(5, 5, 5, 5),
            Content = "SearchableContainer",
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        private Button _playerTest = new()
        {
            Height = 25,
            Margin = new Thickness(5, 5, 5, 5),
            Content = "Player",
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
            TestingSpacesButtons.Children.Add(_sliderTest);
            TestingSpacesButtons.Children.Add(_unlimitedSelectorTest);
            TestingSpacesButtons.Children.Add(_searchSPTest);
            TestingSpacesButtons.Children.Add(_playerTest);

            _clearTestingSpace.Click += _clearTestingSpace_Click;
            _animationTest.Click += _animationTest_Click;
            _buttonTest.Click += _buttonTest_Click;
            _checkBoxTest.Click += _checkBoxTest_Click;
            _comboBoxTest.Click += _comboBoxTest_Click;
            _sliderTest.Click += _sliderTest_Click;
            _unlimitedSelectorTest.Click += _unlimitedSelectorTest_Click;
            _searchSPTest.Click += _searchSPTest_Click;
            _playerTest.Click += _playerTest_Click;
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

            ScrollViewer sv = new();

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


            sv.Content = rectangleColumn;

            TestingSpace.Children.Add(sv);
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
                Content = "Tall button.",
                Height = 60,
                Margin = new Thickness(10, 10, 0, 0),
                DefaultColorAnimations = true,
            };

            ViperButton vButton5 = new()
            {
                Content = "Reading a long text is like trying to find a needle in a haystack, but the needle is the main point, and the haystack is endless paragraphs of rambling.",
                Margin = new Thickness(10, 10, 0, 0),
                DefaultColorAnimations = true,
            };

            ViperButton vButton6 = new()
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
            buttonColumn.Children.Add(vButton6);

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

            List<ViperComboBox> combos = new();

            StackPanel comboColumn = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 10, 0, 0),
            };

            TextBlock debugText = new()
            {
                Text = "Last selection: ",
                Foreground = new SolidColorBrush(Colors.White),
            };

            TextBlock desc1 = new()
            {
                Text = "The first ComboBox has InstaSelection (select first item as soon as is availible) and UsePreviousItem (select previous item if the selected is removed) enabled",
                Foreground = new SolidColorBrush(Colors.White),
            };

            Button debugAdd = new()
            {
                Content = "Add random items"
            };

            Button debugRemove = new()
            {
                Content = "Remove an item"
            };

            Button debugClear = new()
            {
                Content = "Clear items"
            };

            ViperComboBox combo1 = new()
            {
                DefaultColorAnimations = true,
                Margin = new Thickness(0, 10, 0, 0),
                UsePreviousItem = true,
                InstaSelection = true,
            };

            ViperComboBox combo2 = new()
            {
                IsEnabled = false,
                DefaultColorAnimations = true,
                Margin = new Thickness(0, 10, 0, 0),
            };

            ViperComboBox combo3 = new()
            {
                DefaultColorAnimations = true,
                Height = 100,
                Margin = new Thickness(0, 10, 0, 0),
            };

            ViperComboBox combo4 = new()
            {
                DefaultColorAnimations = true,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                FallbackContent = "This is a really long messaage to tell you that this custom ViperComboBox is currently holding no items, add a few with the buttons on the side!",
                Margin = new Thickness(0, 10, 0, 0),
            };

            ViperComboBox combo5 = new()
            {
                DefaultColorAnimations = true,
                Margin = new Thickness(0, 10, 0, 0),
                ItemContainerMaxHeight = 200,
            };

            combos.Add(combo1);
            combos.Add(combo2);
            combos.Add(combo3);
            combos.Add(combo4);
            combos.Add(combo5);

            combo1.SelectionChanged += UpdateDebugText;
            combo2.SelectionChanged += UpdateDebugText;
            combo3.SelectionChanged += UpdateDebugText;
            combo4.SelectionChanged += UpdateDebugText;
            combo5.SelectionChanged += UpdateDebugText;

            void UpdateDebugText(object sender, ViperComboBoxSelectionChanged e)
            {
                debugText.Text = $"Last selection: {(sender as ViperComboBox).GetItemFromIndex(e.Index)}";
            }

            debugAdd.Click += (s, e) =>
            {
                Random rnd = new();

                foreach (ViperComboBox combo in combos)
                {
                    combo.AddItem($"{rnd.Next(0, 1000)}");
                }
            };

            debugRemove.Click += (s, e) =>
            {
                foreach (ViperComboBox combo in combos)
                {
                    combo.RemoveItem(combo.ItemAmount - 1);
                }
            };

            debugClear.Click += (s, e) =>
            {
                foreach (ViperComboBox combo in combos)
                {
                    combo.ClearAllItems();
                }
            };

            comboColumn.Children.Add(desc1);
            comboColumn.Children.Add(combo1);
            comboColumn.Children.Add(combo2);
            comboColumn.Children.Add(combo3);
            comboColumn.Children.Add(combo4);
            comboColumn.Children.Add(combo5);

            TestingSpace.Children.Add(comboColumn);
            TestingInteractions.Children.Add(debugText);
            TestingInteractions.Children.Add(debugAdd);
            TestingInteractions.Children.Add(debugRemove);
            TestingInteractions.Children.Add(debugClear);
        }

        private void _sliderTest_Click(object sender, RoutedEventArgs e)
        {
            DisposeLastTest("Slider");

            StackPanel sliderColumn = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 10, 10, 0),
            };

            TextBlock debugText = new()
            {
                Text = "Value: 0",
                Foreground = new SolidColorBrush(Colors.White),
            };

            TextBlock desc1 = new()
            {
                Text = "This one is scrollable, others are not",
                Foreground = new SolidColorBrush(Colors.White),
            };

            ViperSlider slider1 = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 40,
                MaxValue = 200,
                MinValue = 0,
                Scrollable = true,
            };

            ViperSlider slider2 = new()
            {
                Height = 40,
                Width = 300,
                MaxValue = 100,
                MinValue = 0,
                ProgressBarBrush = new SolidColorBrush(Colors.Gold),
            };

            ViperSlider slider3 = new()
            {
                Height = 40,
                Width = 200,
                MaxValue = 10,
                MinValue = 0,
                SliderBrush = new SolidColorBrush(Colors.Gold),
            };

            ViperSlider slider4 = new()
            {
                Height = 40,
                Width = 200,
                MaxValue = 200,
                MinValue = 0,
                SliderBrush = new SolidColorBrush(Colors.Gold),
                ProgressBarBrush = new SolidColorBrush(Colors.Gold),
            };

            ViperSlider slider5 = new()
            {
                Height = 40,
                Width = 200,
                IsEnabled = false,
            };

            sliderColumn.Children.Add(desc1);
            sliderColumn.Children.Add(slider1);
            sliderColumn.Children.Add(slider2);
            sliderColumn.Children.Add(slider3);
            sliderColumn.Children.Add(slider4);
            sliderColumn.Children.Add(slider5);

            slider1.ValueChanged += (s, e) =>
            {
                debugText.Text = "Value: " + e.Value.ToString();
            };

            slider2.ValueChanged += (s, e) =>
            {
                debugText.Text = "Value: " + e.Value.ToString();
            };

            slider3.ValueChanged += (s, e) =>
            {
                debugText.Text = "Value: " + e.Value.ToString();
            };

            slider4.ValueChanged += (s, e) =>
            {
                debugText.Text = "Value: " + e.Value.ToString();
            };

            TestingSpace.Children.Add(sliderColumn);
            TestingInteractions.Children.Add(debugText);
        }

        private void _unlimitedSelectorTest_Click(object sender, RoutedEventArgs e)
        {
            DisposeLastTest("UnlimitedSelector");

            StackPanel unlimtedSelectorColumn = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 10, 10, 0),
                
            };

            TextBlock desc1 = new()
            {
                Text = "Not a lot to see here",
                Foreground = new SolidColorBrush(Colors.White),
            };

            TextBlock debugText = new()
            {
                Text = "Value: 0",
                Foreground = new SolidColorBrush(Colors.White),
            };

            ViperUnlimitedSelector us1 = new()
            {
                Margin = new Thickness(0, 0, 0, 10),
                DefaultColorAnimations = true,
            };

            ViperUnlimitedSelector us2 = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 10),
                DefaultColorAnimations = true,
            };

            us1.IndexChanged += (s, e) =>
            {
                debugText.Text = "Value: " + e.Index.ToString();
            };

            us2.IndexChanged += (s, e) =>
            {
                debugText.Text = "Value: " + e.Index.ToString();
            };

            unlimtedSelectorColumn.Children.Add(desc1);
            unlimtedSelectorColumn.Children.Add(us1);
            unlimtedSelectorColumn.Children.Add(us2);

            TestingSpace.Children.Add(unlimtedSelectorColumn);
            TestingInteractions.Children.Add(debugText);
        }

        private void _searchSPTest_Click(object sender, RoutedEventArgs e)
        {
            DisposeLastTest("SearchableGroupBoxStackPanel");

            SearchableGroupBoxStackPanel thing = new()
            {
                Background = new SolidColorBrush(Color.FromRgb(23, 23, 23)),
            };

            TextBox searchBox = new()
            {
                Height = 30,
                FontSize = 19,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            thing.AddElement(new Label() { Foreground = new SolidColorBrush(Colors.White), Content = "SearchTest", FontSize = 21}, "", false);
            thing.AddElement(new Label() { Foreground = new SolidColorBrush(Colors.White), Content = "Coso" }, "coso");
            thing.AddElement(new Label() { Foreground = new SolidColorBrush(Colors.White), Content = "Cosa" }, "cosa");
            thing.AddElement(new Label() { Foreground = new SolidColorBrush(Colors.White), Content = "Algo" }, "algo");
            thing.AddElement(new Label() { Foreground = new SolidColorBrush(Colors.White), Content = "LoL" }, "lol league of legends");
            thing.AddElement(new Label() { Foreground = new SolidColorBrush(Colors.White), Content = "Wawa" }, "wawa");

            searchBox.PreviewKeyUp += (s, e) =>
            {
                thing.Search(searchBox.Text);

                if (searchBox.Text == "")
                {
                    thing.ShowAllElements();
                }
            };

            searchBox.LostFocus += (s, e) =>
            {
                searchBox.Text = "";
            };

            TestingSpace.Children.Add(thing);
            TestingInteractions.Children.Add(searchBox);
        }

        private void _playerTest_Click(object sender, RoutedEventArgs e)
        {
            DisposeLastTest("Player");

            Player player = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            player.IncreasePlayerSize(5);

            TestingSpace.Children.Add(player);
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
