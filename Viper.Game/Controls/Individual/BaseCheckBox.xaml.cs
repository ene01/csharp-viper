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
    public partial class BaseCheckBox : UserControl
    {
        /// <summary>
        /// Im lazy, so heres the entire control container, do whatrever, use events, etc, idk.
        /// </summary>
        public UserControl Container => CheckBoxControl;

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

        public EventHandler<ViperCheckBoxStateChanged>? StateChanged;

        // Const.
        private const string CONTENT_VIPER_CHECKBOX = "ViperCheckBox";
        private static readonly Brush BACKGROUND_COLOR = new SolidColorBrush(Color.FromRgb(23, 23, 23));
        private static readonly Brush BORDER_COLOR = new SolidColorBrush(Color.FromRgb(80, 80, 80));
        private static readonly Brush FILL_COLOR = new SolidColorBrush(Color.FromRgb(35, 35, 35));
        private static readonly Brush STROKE_COLOR = new SolidColorBrush(Color.FromArgb(86, 255, 255, 255));
        private static readonly Brush FOREGROUND_COLOR = new SolidColorBrush(Color.FromRgb(255, 255, 255));

        private object _content = CONTENT_VIPER_CHECKBOX;
        private Brush _background = BACKGROUND_COLOR;
        private Brush _border = BORDER_COLOR;
        private Brush _fill = FILL_COLOR;
        private Brush _stroke = STROKE_COLOR;
        private Brush _foreground = FOREGROUND_COLOR;


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

        public new Brush BorderBrush
        {
            get { return _border; }

            set
            {
                _border = value;

                CheckBoxBorder.BorderBrush = value;
            }
        }

        public Brush CheckFill
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

        private IEasingFunction elastic = new ElasticEase() { Springiness = 5 };
        private IEasingFunction quadOut = new QuadraticEase() { EasingMode = EasingMode.EaseOut };

        public BaseCheckBox()
        {
            InitializeComponent();

            Loaded += ViperCheckBox_Loaded;
            Unloaded += ViperCheckBox_Unloaded;
            CheckBoxControl.LostFocus += ViperCheckBox_LostFocus;

            CheckBoxControl.MouseEnter += CheckBoxContainer_MouseEnter;
            CheckBoxControl.MouseLeave += CheckBoxContainer_MouseLeave;
            CheckBoxControl.PreviewMouseLeftButtonDown += CheckBoxContainer_PreviewMouseLeftButtonDown;
            CheckBoxControl.PreviewMouseLeftButtonUp += CheckBoxContainer_PreviewMouseLeftButtonUp;
        }

        // In case the user loses focus (focus to other window, keybind to change focus, etc.), trigger a release event.
        private void ViperCheckBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Release?.Invoke(this, new EventArgs());
        }

        // Remove events when unloaded just in case.
        private void ViperCheckBox_Unloaded(object sender, RoutedEventArgs e)
        {
            Check.MouseEnter -= CheckBoxContainer_MouseEnter;
            Check.MouseLeave -= CheckBoxContainer_MouseLeave;
            Check.PreviewMouseLeftButtonDown -= CheckBoxContainer_PreviewMouseLeftButtonDown;
            Check.PreviewMouseLeftButtonUp -= CheckBoxContainer_PreviewMouseLeftButtonUp;
            Loaded -= ViperCheckBox_Loaded;
            Unloaded -= ViperCheckBox_Unloaded;
        }

        // Enables or disables the "black overlay" that appears on top of the button in case its enabled/disabled.
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

        // Set the "middle" point when loaded.
        private void ViperCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            Check.RenderTransform = new ScaleTransform(1, 1) { CenterX = Check.ActualWidth / 2, CenterY = Check.ActualHeight / 2 };
            EnabledLayerToggle(IsEnabled);
        }

        private bool _canRegisterClick = false;

        private void CheckBoxContainer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsEnabled && _canRegisterClick)
            {
                Click?.Invoke(this, new EventArgs());

                _isChecked = !_isChecked;

                StateChanged?.Invoke(this, new ViperCheckBoxStateChanged(_isChecked));
            }

            if (_canRegisterClick)
            {
                Release?.Invoke(this, new EventArgs());
            }
        }

        private void CheckBoxContainer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _canRegisterClick = true;

            Holding?.Invoke(this, new EventArgs());

            CheckBoxControl.MouseLeave += LocalMouseLeave;

            void LocalMouseLeave(object sender, MouseEventArgs e)
            {
                NoHovering?.Invoke(this, new EventArgs());

                CheckBoxControl.MouseLeave -= LocalMouseLeave;
            }
        }

        private void CheckBoxContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            NoHovering?.Invoke(this, new EventArgs());

            // An attempt at avoiding weird interactions when the user presses, moves the cursor away, hover the check again and releases the click button.
            _canRegisterClick = false;
        }

        private void CheckBoxContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            Hovering?.Invoke(this, new EventArgs());
        }
    }
}
