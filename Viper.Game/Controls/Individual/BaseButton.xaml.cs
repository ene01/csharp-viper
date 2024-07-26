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

namespace Viper.Game.Controls.Individual
{
    /// <summary>
    /// Lógica de interacción para ViperButton.xaml
    /// </summary>
    public partial class BaseButton : UserControl
    {
        /// <summary>
        /// Im lazy, so heres the entire control container, do whatrever, use events, etc, idk.
        /// </summary>
        public UserControl Container => ButtonControl;

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

        // Define const.
        private const string CONTENT_VIPER_BUTTON = "ViperButton";
        private static readonly Brush BACKGROUND_COLOR = new SolidColorBrush(Color.FromRgb(23, 23, 23));
        private static readonly Brush BORDER_COLOR = new SolidColorBrush(Color.FromRgb(80, 80, 80));
        private static readonly Brush FOREGROUND_COLOR = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        private const bool IS_ENABLED_TRUE = true;

        // Private properties used for the button.
        private object _content = CONTENT_VIPER_BUTTON;
        private Brush _background = BACKGROUND_COLOR;
        private Brush _border = BORDER_COLOR;
        private Brush _foreground = FOREGROUND_COLOR;
        private bool _isEnabled = IS_ENABLED_TRUE;

        /// <summary>
        /// Define whats inside the button.
        /// </summary>
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

                    ButtonContent.Content = text;
                }
                else
                {
                    ButtonContent.Content = value;
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

                ButtonGrid.Background = value;
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

                ButtonBorder.BorderBrush = value;
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

                ButtonContent.Foreground = value;
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

        private bool _setDefaultColorAnimations = false;

        private IEasingFunction elastic = new ElasticEase() { Springiness = 5 };
        private IEasingFunction quadOut = new QuadraticEase() { EasingMode = EasingMode.EaseOut };

        /// <summary>
        /// A cool button.
        /// </summary>
        public BaseButton()
        {
            InitializeComponent();

            Loaded += ViperButton_Loaded;
            Unloaded += ViperButton_Unloaded;
            ButtonControl.LostFocus += ViperButton_LostFocus;

            ButtonControl.MouseEnter += ButtonContainer_MouseEnter;
            ButtonControl.MouseLeave += ButtonContainer_MouseLeave;
            ButtonControl.PreviewMouseLeftButtonDown += ButtonContainer_PreviewMouseLeftButtonDown;
            ButtonControl.PreviewMouseLeftButtonUp += ButtonContainer_PreviewMouseLeftButtonUp;
        }

        private void ViperButton_LostFocus(object sender, RoutedEventArgs e)
        {
            Release?.Invoke(this, new EventArgs());
        }

        private void ViperButton_Unloaded(object sender, RoutedEventArgs e)
        {
            ButtonGrid.SizeChanged -= ButtonGrid_SizeChanged;
            ButtonControl.MouseEnter -= ButtonContainer_MouseEnter;
            ButtonControl.MouseLeave -= ButtonContainer_MouseLeave;
            ButtonControl.PreviewMouseLeftButtonDown -= ButtonContainer_PreviewMouseLeftButtonDown;
            ButtonControl.PreviewMouseLeftButtonUp -= ButtonContainer_PreviewMouseLeftButtonUp;
            Loaded -= ViperButton_Loaded;
            Unloaded -= ViperButton_Unloaded;
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

        private void ViperButton_Loaded(object sender, RoutedEventArgs e)
        {
            ButtonGrid.SizeChanged += ButtonGrid_SizeChanged;

            ButtonGrid.RenderTransform = new ScaleTransform(1, 1) { CenterX = ButtonGrid.ActualWidth / 2, CenterY = ButtonGrid.ActualHeight / 2 };
            EnabledLayerToggle(_isEnabled);
        }

        private void ButtonGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ButtonGrid.RenderTransform = new ScaleTransform(1, 1) { CenterX = ButtonGrid.ActualWidth / 2, CenterY = ButtonGrid.ActualHeight / 2 };
        }

        private bool _canRegisterClick = false;

        private void ButtonContainer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isEnabled && _canRegisterClick)
            {
                Click?.Invoke(this, new EventArgs());
            }

            if (_canRegisterClick)
            {
                Release?.Invoke(this, new EventArgs());
            }
        }

        private void ButtonContainer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _canRegisterClick = true;

            Holding?.Invoke(this, new EventArgs());

            ButtonControl.MouseLeave += LocalMouseLeave;

            void LocalMouseLeave(object sender, MouseEventArgs e)
            {
                NoHovering?.Invoke(this, new EventArgs());

                ButtonControl.MouseLeave -= LocalMouseLeave;
            }
        }

        private void ButtonContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            NoHovering?.Invoke(this, new EventArgs());
            _canRegisterClick = false;
        }

        private void ButtonContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            Hovering?.Invoke(this, new EventArgs());
        }
    }
}
