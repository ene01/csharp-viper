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

namespace Viper.Game.Controls
{
    /// <summary>
    /// Lógica de interacción para ViperButton.xaml
    /// </summary>
    public partial class ViperCheckBox : UserControl
    {
        /// <summary>
        /// Im lazy, so heres the entire control container, do whatrever, use events, etc, idk.
        /// </summary>
        public UserControl Container => CheckBoxContainer;

        /// <summary>
        /// Events that triggers when the button is enabled and clicked.
        /// </summary>
        public EventHandler? Click;

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

        public EventHandler<ViperCheckBoxStateChanged> StateChanged;

        private object _content = "ViperCheckBox";
        private Brush _background = new SolidColorBrush(Color.FromRgb(23, 23, 23));
        private Brush _border = new SolidColorBrush(Color.FromRgb(80, 80, 80));
        private Brush _fill = new SolidColorBrush(Color.FromRgb(35, 35, 35));
        private Brush _stroke = new SolidColorBrush(Color.FromArgb(86, 255, 255, 255));
        private Brush _foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        private double _containerHeight = double.NaN;
        private double _containerWidth = double.NaN;
        private Thickness _spacing = new Thickness(0, 0, 0, 0);
        private Transform _transforms = null;
        private VerticalAlignment _yAlignment = VerticalAlignment.Top;
        private HorizontalAlignment _xAlignment = HorizontalAlignment.Left;
        private bool _isEnabled = true;

        public new object Content
        {
            get { return _content; }

            set
            {
                _content = value;

                if (value is string)
                {
                    TextBlock text = new()
                    {
                        Text = (string)_content,
                        TextWrapping = TextWrapping.WrapWithOverflow,
                    };

                    CheckBoxContent.Content = text;
                }
                else
                {
                    CheckBoxContent.Content = value;
                }
            }
        }

        public new Brush Background
        {
            get { return _background; }

            set
            {
                _background = value;

                CheckBoxGrid.Background = value;
            }
        }

        public Brush BorderBrush
        {
            get { return _border; }

            set
            {
                _border = value;

                CheckBoxBorder.BorderBrush = value;
            }
        }

        public new Brush CheckFill
        {
            get { return _background; }

            set
            {
                _fill= value;

                Check.Fill = value;
            }
        }

        public Brush CheckStroke
        {
            get { return _border; }

            set
            {
                _stroke = value;

                Check.Stroke = value;
            }
        }

        public new Brush Foreground
        {
            get { return _foreground; }

            set 
            { 
                _foreground = value;

                CheckBoxContent.Foreground = value;
            }
        }

        public new double Height
        {
            get { return _containerHeight; }

            set 
            { 
                _containerHeight = value;

                Container.Height = value;
            }
        }

        public new double Width
        {
            get { return _containerWidth; }

            set 
            { 
                _containerWidth = value;

                Container.Width = value;
            }
        }

        public new Thickness Margin
        {
            get { return _spacing; }

            set 
            { 
                _spacing = value;

                Container.Margin = value;
            }
        }

        public new Transform RenderTransform
        {
            get { return _transforms; }

            set 
            { 
                _transforms = value;

                Container.RenderTransform = value;
            }
        }

        public new VerticalAlignment VerticalAlignment
        {
            get { return _yAlignment; }

            set 
            { 
                _yAlignment = value;

                Container.VerticalAlignment = value;
            }
        }

        public new HorizontalAlignment HorizontalAlignment
        {
            get { return _xAlignment; }

            set 
            { 
                _xAlignment = value;

                Container.HorizontalAlignment = value;
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

        private bool _isChecked = false;

        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }

            set
            {
                _isChecked = value;

                StateChanged?.Invoke(this, new ViperCheckBoxStateChanged(value));
            }
        }

        private bool _setDefaultColorAnimations = false;

        /// <summary>
        /// Sets default colors, and color animations for buttons, black when not hovered, white when clicked (red if disabled), and grey when hovered).
        /// THIS REPLACES ANY BRUSH THAT THE BUTTON HAD.
        /// </summary>
        public bool DefaultColorAnimations
        {
            get { return _setDefaultColorAnimations; }

            set
            {
                _setDefaultColorAnimations = value;

                if (_setDefaultColorAnimations)
                {
                    SetDefaltCheckAnimations(this);
                }
                else
                {
                    this.Release = null;
                    this.Holding = null;
                    this.Hovering = null;
                    this.NoHovering = null;
                }
            }
        }

        private IEasingFunction elastic = new ElasticEase() { Springiness = 5 };
        private IEasingFunction quadOut = new QuadraticEase() { EasingMode = EasingMode.EaseOut };

        public ViperCheckBox()
        {
            InitializeComponent();

            Loaded += ViperCheckBox_Loaded;
            Unloaded += ViperCheckBox_Unloaded;
            CheckBoxContainer.LostFocus += ViperCheckBox_LostFocus;

            CheckBoxContainer.MouseEnter += CheckBoxContainer_MouseEnter;
            CheckBoxContainer.MouseLeave += CheckBoxContainer_MouseLeave;
            CheckBoxContainer.PreviewMouseLeftButtonDown += CheckBoxContainer_PreviewMouseLeftButtonDown;
            CheckBoxContainer.PreviewMouseLeftButtonUp += CheckBoxContainer_PreviewMouseLeftButtonUp;
        }

        private void ViperCheckBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Release?.Invoke(this, new EventArgs());
        }

        private void ViperCheckBox_Unloaded(object sender, RoutedEventArgs e)
        {
            Check.MouseEnter -= CheckBoxContainer_MouseEnter;
            Check.MouseLeave -= CheckBoxContainer_MouseLeave;
            Check.PreviewMouseLeftButtonDown -= CheckBoxContainer_PreviewMouseLeftButtonDown;
            Check.PreviewMouseLeftButtonUp -= CheckBoxContainer_PreviewMouseLeftButtonUp;
            Loaded -= ViperCheckBox_Loaded;
            Unloaded -= ViperCheckBox_Unloaded;
        }

        private void EnabledLayerToggle(bool enable)
        {
            if (enable)
            {
                Animate.Double(Check, OpacityProperty, 1, TimeSpan.FromMilliseconds(200));
                Animate.Double(CheckBoxContent, OpacityProperty, 1, TimeSpan.FromMilliseconds(200));
            }
            else
            {
                Animate.Double(Check, OpacityProperty, 0.6, TimeSpan.FromMilliseconds(200));
                Animate.Double(CheckBoxContent, OpacityProperty, 0.6, TimeSpan.FromMilliseconds(200));
            }
        }

        private void ViperCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            Check.RenderTransform = new ScaleTransform(1, 1) { CenterX = Check.ActualWidth / 2, CenterY = Check.ActualHeight / 2 };
            EnabledLayerToggle(_isEnabled);
        }

        private bool _canRegisterClick = false;

        private void CheckBoxContainer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isEnabled && _canRegisterClick)
            {
                Click?.Invoke(this, new EventArgs());

                _isChecked = !_isChecked;

                StateChanged?.Invoke(this, new ViperCheckBoxStateChanged(_isChecked));
            }

            if (_canRegisterClick)
            {
                Release?.Invoke(this, new EventArgs());

                Animate.Double(Check.RenderTransform, ScaleTransform.ScaleXProperty, 1, TimeSpan.FromMilliseconds(1000), elastic);
                Animate.Double(Check.RenderTransform, ScaleTransform.ScaleYProperty, 1, TimeSpan.FromMilliseconds(1000), elastic);
            }
        }

        private void CheckBoxContainer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _canRegisterClick = true;

            Holding?.Invoke(this, new EventArgs());

            Animate.Double(Check.RenderTransform, ScaleTransform.ScaleXProperty, 0.7, TimeSpan.FromMilliseconds(1000), quadOut);
            Animate.Double(Check.RenderTransform, ScaleTransform.ScaleYProperty, 0.7, TimeSpan.FromMilliseconds(1000), quadOut);

            CheckBoxContainer.MouseLeave += LocalMouseLeave;

            void LocalMouseLeave(object sender, MouseEventArgs e)
            {
                NoHovering?.Invoke(this, new EventArgs());

                Animate.Double(Check.RenderTransform, ScaleTransform.ScaleXProperty, 1, TimeSpan.FromMilliseconds(1000), elastic);
                Animate.Double(Check.RenderTransform, ScaleTransform.ScaleYProperty, 1, TimeSpan.FromMilliseconds(1000), elastic);

                CheckBoxContainer.MouseLeave -= LocalMouseLeave;
            }
        }

        private async void CheckBoxContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            NoHovering?.Invoke(this, new EventArgs());
            _canRegisterClick = false;
        }

        private async void CheckBoxContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            Hovering?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Sets default colors, and color animations for buttons, black when not hovered, grey when hovered, and white when clicked (red if disabled)).
        /// THIS REPLACES ANY BRUSH THAT THE BUTTON HAD.
        /// </summary>
        /// <param name="viperButton"></param>
        public static void SetDefaltCheckAnimations(ViperCheckBox viperCheckBox)
        {
            SolidColorBrush bgBrush = new(Color.FromRgb(35, 35, 35));
            SolidColorBrush bdBrush = new(Color.FromArgb(86, 80, 80, 80));

            viperCheckBox.CheckFill = bgBrush;
            viperCheckBox.CheckStroke = bdBrush;

            viperCheckBox.StateChanged+= OnStateChanged;
            viperCheckBox.Hovering += OnHover;
            viperCheckBox.NoHovering += OnNoHover;

            void OnStateChanged(object sender, EventArgs e)
            {
                if (viperCheckBox.IsChecked)
                {
                    Animate.Color(bgBrush, SolidColorBrush.ColorProperty, Color.FromRgb(56, 255, 102), TimeSpan.FromMilliseconds(100), new QuadraticEase(), Colors.White);
                }
                else
                {
                    Animate.Color(bgBrush, SolidColorBrush.ColorProperty, Color.FromRgb(35, 35, 35), TimeSpan.FromMilliseconds(100), new QuadraticEase(), Colors.White);
                }
            }

            void OnHover(object sender, EventArgs e)
            {
                if (viperCheckBox.IsEnabled)
                {
                    Animate.Color(bdBrush, SolidColorBrush.ColorProperty, Color.FromArgb(255, 255, 255, 255), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                }
                else
                {
                    Animate.Color(bdBrush, SolidColorBrush.ColorProperty, Color.FromArgb(255, 255, 56, 56), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                }
            }

            void OnNoHover(object sender, EventArgs e)
            {
                Animate.Color(bdBrush, SolidColorBrush.ColorProperty, Color.FromArgb(86, 255, 255, 255), TimeSpan.FromMilliseconds(300), new QuadraticEase());
            }
        }
    }
}
