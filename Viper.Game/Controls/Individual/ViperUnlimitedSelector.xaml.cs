using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Viper.Game.Animations;
using Viper.Game.Events;

namespace Viper.Game.Controls.Individual
{
    /// <summary>
    /// Lógica de interacción para ViperButton.xaml
    /// </summary>
    public partial class ViperUnlimitedSelector : UserControl
    {
        /// <summary>
        /// Im lazy, so heres the entire control container, do whatrever, use events, etc, idk.
        /// </summary>
        public UserControl Container => UnlimitedSelectorControl;

        /// <summary>
        /// Events that triggers when the button is enabled and clicked.
        /// </summary>
        public EventHandler? LeftClick;

        /// <summary>
        /// Events that triggers when the button is enabled and clicked.
        /// </summary>
        public EventHandler? RightClick;

        /// <summary>
        /// Event that triggers when the button is pressed, can trigger even if the button is disabled.
        /// </summary>
        public EventHandler? Holding;

        /// <summary>
        /// Event that triggers when the button is released, can trigger even if the button is disabled.
        /// </summary>
        public EventHandler? Release;

        /// <summary>
        /// Event that triggers when the mouse is being hovered over the button.
        /// </summary>
        public EventHandler? Hovering;

        /// <summary>
        /// Event that triggers when the mouse is no longer hovering the button.
        /// </summary>
        public EventHandler? NoHovering;

        public EventHandler<ViperUnlimitedSelectorIndexChanged>? IndexChanged;

        // Define const.
        private static readonly Brush BACKGROUND_COLOR = new SolidColorBrush(Color.FromRgb(23, 23, 23));
        private static readonly Brush BORDER_COLOR = new SolidColorBrush(Color.FromRgb(80, 80, 80));
        private static readonly Brush FOREGROUND_COLOR = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        private static readonly Brush SIDE_BUTTONS_COLOR = new SolidColorBrush(Color.FromRgb(82, 82, 82));
        private static readonly object SIDE_BUTTON_LEFT_CONTENT = "🡸";
        private static readonly object SIDE_BUTTON_RIGTH_CONTENT = "🡺";
        private const double CONTAINER_HEIGHT_NAN = double.NaN;
        private const double CONTAINER_WIDTH_NAN = double.NaN;
        private static readonly Thickness SPACING_ZERO = new Thickness(0, 0, 0, 0);
        private const VerticalAlignment Y_ALIGNMENT_TOP = VerticalAlignment.Top;
        private const HorizontalAlignment X_ALIGNMENT_LEFT = HorizontalAlignment.Left;
        private const bool IS_ENABLED_TRUE = true;
        private const int DEFAULT_INDEX = 0;

        // Private properties used for the button.
        private object _leftContent = SIDE_BUTTON_LEFT_CONTENT;
        private object _rightContent = SIDE_BUTTON_RIGTH_CONTENT;
        private Brush _background = BACKGROUND_COLOR;
        private Brush _sideBackground = SIDE_BUTTONS_COLOR;
        private Brush _border = BORDER_COLOR;
        private Brush _foreground = FOREGROUND_COLOR;
        private Brush _sideButtons = SIDE_BUTTONS_COLOR;
        private double _containerHeight = CONTAINER_HEIGHT_NAN;
        private double _containerWidth = CONTAINER_WIDTH_NAN;
        private Thickness _spacing = SPACING_ZERO;
        private VerticalAlignment _yAlignment = Y_ALIGNMENT_TOP;
        private HorizontalAlignment _xAlignment = X_ALIGNMENT_LEFT;
        private bool _isEnabled = IS_ENABLED_TRUE;
        private int _currentIndex = DEFAULT_INDEX;
        private bool _defaultAnim = false;

        public object LeftContent
        {
            get { return _leftContent; }

            set
            {
                _leftContent = value;

                if (value is string)
                {
                    TextBlock text = new()
                    {
                        Text = (string)_leftContent,
                        TextWrapping = TextWrapping.WrapWithOverflow,
                    };

                    Left.Content = text;
                }
                else
                {
                    Left.Content = value;
                }
            }
        }

        public object RightContent
        {
            get { return _rightContent; }

            set
            {
                _leftContent = value;

                if (value is string)
                {
                    TextBlock text = new()
                    {
                        Text = (string)_rightContent,
                        TextWrapping = TextWrapping.WrapWithOverflow,
                    };

                    Right.Content = text;
                }
                else
                {
                    Right.Content = value;
                }
            }
        }

        /// <summary>
        /// Background brush for the button.
        /// </summary>
        public new Brush Background
        {
            get { return _background; }

            set 
            { 
                _background = value;

                UnlimitedSelectorGrid.Background = value;
            }
        }

        public Brush SideButtonsBackground
        {
            get { return _sideBackground; }

            set
            {
                _background = value;

                LeftButton.Background = value;
                RightButton.Background = value;
            }
        }

        /// <summary>
        /// Border brush for the button, a square that appears at the edges of the button
        /// </summary>
        public new Brush BorderBrush
        {
            get { return _border; }

            set
            { 
                _border = value;

                UnlimitedSelectorBorder.BorderBrush = value;
            }
        }

        /// <summary>
        /// Text brush.
        /// </summary>
        public new Brush Foreground
        {
            get { return _foreground; }

            set 
            { 
                _foreground = value;

                IndexViewer.Foreground = value;
            }
        }

        public new bool IsEnabled
        {
            get { return _isEnabled; }

            set 
            { 
                _isEnabled = value;

                EnabledLayerToggle(value);
            }
        }

        public int Index
        {
            get
            {
                return _currentIndex;   
            }

            set
            {
                _currentIndex = value;
            }
        }

        public bool DefaultColorAnimations
        {
            get => _defaultAnim;

            set
            {
                if (value)
                {
                    SetDefaultColorAnimations(this);
                }
                else
                {
                    this.RightClick = null;
                    this.LeftClick = null;

                    BorderBrush = BORDER_COLOR;
                }
            }
        }

        private bool _setDefaultColorAnimations = false;

        private IEasingFunction elastic = new ElasticEase() { Springiness = 5, Oscillations = 2 };
        private IEasingFunction quadOut = new QuadraticEase() { EasingMode = EasingMode.EaseOut };

        /// <summary>
        /// A cool button.
        /// </summary>
        public ViperUnlimitedSelector()
        {
            InitializeComponent();

            IndexViewer.RenderTransform = new TranslateTransform();

            LeftButton.SizeChanged += LeftButton_SizeChanged;
            RightButton.SizeChanged += RightButton_SizeChanged;

            LeftButton.PreviewMouseLeftButtonUp += LeftButton_PreviewMouseLeftButtonUp;
            RightButton.PreviewMouseLeftButtonUp += RightButton_PreviewMouseLeftButtonUp;
        }

        private void RightButton_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Thickness current = CenterElements.Margin;

            CenterElements.Margin = new Thickness(current.Left, current.Top, RightButton.ActualWidth, current.Bottom);
        }

        private void LeftButton_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Thickness current = CenterElements.Margin;

            CenterElements.Margin = new Thickness(LeftButton.ActualWidth, current.Top, current.Right, current.Bottom);
        }

        private async void RightButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Right");

            if (_isEnabled)
            {
                _currentIndex += 1;
                IndexViewer.Text = $"{_currentIndex + 1}";

                Animate.Color(WhiteOverlayR.Fill, SolidColorBrush.ColorProperty, Color.FromArgb(0, 128, 189, 255), TimeSpan.FromMilliseconds(300), quadOut, Color.FromArgb(255, 128, 189, 255));

                Animate.Double(IndexViewer.RenderTransform, TranslateTransform.XProperty, 3, TimeSpan.FromMilliseconds(50), quadOut);

                await Task.Delay(50);

                Animate.Double(IndexViewer.RenderTransform, TranslateTransform.XProperty, 0, TimeSpan.FromMilliseconds(300), elastic);

                RightClick?.Invoke(this, new EventArgs());
                IndexChanged?.Invoke(this, new ViperUnlimitedSelectorIndexChanged(_currentIndex));
            }
            else
            {
                Animate.Color(WhiteOverlayR.Fill, SolidColorBrush.ColorProperty, Color.FromArgb(0, 255, 128, 128), TimeSpan.FromMilliseconds(300), quadOut, Color.FromArgb(255, 255, 128, 128));
            }
        }

        private async void LeftButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Left");

            if (!(_currentIndex - 1 < 0) && _isEnabled)
            {
                _currentIndex -= 1;
                IndexViewer.Text = $"{_currentIndex + 1}";

                Animate.Color(WhiteOverlayL.Fill, SolidColorBrush.ColorProperty, Color.FromArgb(0, 128, 189, 255), TimeSpan.FromMilliseconds(300), quadOut, Color.FromArgb(255, 128, 189, 255));

                Animate.Double(IndexViewer.RenderTransform, TranslateTransform.XProperty, -3, TimeSpan.FromMilliseconds(50), quadOut);

                await Task.Delay(50);

                Animate.Double(IndexViewer.RenderTransform, TranslateTransform.XProperty, 0, TimeSpan.FromMilliseconds(300), elastic);

                LeftClick?.Invoke(this, new EventArgs());
                IndexChanged?.Invoke(this, new ViperUnlimitedSelectorIndexChanged(_currentIndex));
            }
            else
            {
                Animate.Color(WhiteOverlayL.Fill, SolidColorBrush.ColorProperty, Color.FromArgb(0, 255, 128, 128), TimeSpan.FromMilliseconds(300), quadOut, Color.FromArgb(255, 255, 128, 128));
            }
        }

        public static void SetDefaultColorAnimations(ViperUnlimitedSelector us)
        {
            GradientStop gs1 = new()
            {
                Color = Color.FromRgb(80, 80, 80),
                Offset = 0,
            };

            GradientStop gs2 = new()
            {
                Color = Color.FromRgb(80, 80, 80),
                Offset = 1,
            };

            LinearGradientBrush lg = new() { GradientStops = { gs1, gs2 }, StartPoint = new Point(1, 0), EndPoint = new Point(0, 0) };

            us.BorderBrush = lg;

            us.LeftClick += (s, e) =>
            {
                Animate.Color(gs2, GradientStop.ColorProperty, Color.FromRgb(80, 80, 80), TimeSpan.FromMilliseconds(500), new QuadraticEase(), Colors.White);
            };

            us.RightClick += (s, e) =>
            {
                Animate.Color(gs1, GradientStop.ColorProperty, Color.FromRgb(80, 80, 80), TimeSpan.FromMilliseconds(500), new QuadraticEase(), Colors.White);
            };
        }

        private void EnabledLayerToggle(bool enable)
        {
            if (enable)
            {
                Animate.Double(Blackout, OpacityProperty, 0, TimeSpan.FromMilliseconds(200));
            }
            else
            {
                Animate.Double(Blackout, OpacityProperty, 0.3, TimeSpan.FromMilliseconds(200));
            }
        }
    }
}
