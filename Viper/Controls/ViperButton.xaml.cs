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
using Viper.Animations;

namespace Viper.Game.Controls
{
    /// <summary>
    /// Lógica de interacción para ViperButton.xaml
    /// </summary>
    public partial class ViperButton : UserControl
    {
        /// <summary>
        /// Im lazy, so heres the entire control container, do whatrever, use events, etc, idk.
        /// </summary>
        public UserControl Container
        {
            get
            {
                return ButtonContainer;
            }
        }

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

        private object _content = "ViperButton";
        private Brush _background = new SolidColorBrush(Color.FromRgb(23, 23, 23));
        private Brush _border = new SolidColorBrush(Color.FromRgb(80, 80, 80));
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

                    ButtonContent.Content = text;
                }
                else
                {
                    ButtonContent.Content = value;
                }
            }
        }

        public new Brush Background
        {
            get { return _background; }

            set 
            { 
                _background = value; 

                ButtonGrid.Background = value;
            }
        }

        public new Brush BorderBrush
        {
            get { return _border; }

            set
            { 
                _border = value;

                ButtonBorder.BorderBrush = value;
            }
        }

        public new Brush Foreground
        {
            get { return _foreground; }

            set 
            { 
                _foreground = value;

                ButtonContent.Foreground = value;
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
                    SetDefaltBrushAnimations(this);
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

        public ViperButton()
        {
            InitializeComponent();



            Loaded += ViperButton_Loaded;
            Unloaded += ViperButton_Unloaded;
            ButtonContainer.LostFocus += ViperButton_LostFocus;

            ButtonContainer.MouseEnter += ButtonContainer_MouseEnter;
            ButtonContainer.MouseLeave += ButtonContainer_MouseLeave;
            ButtonContainer.PreviewMouseLeftButtonDown += ButtonContainer_PreviewMouseLeftButtonDown;
            ButtonContainer.PreviewMouseLeftButtonUp += ButtonContainer_PreviewMouseLeftButtonUp;
        }

        private void ViperButton_LostFocus(object sender, RoutedEventArgs e)
        {
            Release?.Invoke(this, new EventArgs());
        }

        private void ViperButton_Unloaded(object sender, RoutedEventArgs e)
        {
            ButtonGrid.SizeChanged -= ButtonGrid_SizeChanged;
            ButtonContainer.MouseEnter -= ButtonContainer_MouseEnter;
            ButtonContainer.MouseLeave -= ButtonContainer_MouseLeave;
            ButtonContainer.PreviewMouseLeftButtonDown -= ButtonContainer_PreviewMouseLeftButtonDown;
            ButtonContainer.PreviewMouseLeftButtonUp -= ButtonContainer_PreviewMouseLeftButtonUp;
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

                Animate.Double(ButtonGrid.RenderTransform, ScaleTransform.ScaleXProperty, 1, TimeSpan.FromMilliseconds(1000), elastic);
                Animate.Double(ButtonGrid.RenderTransform, ScaleTransform.ScaleYProperty, 1, TimeSpan.FromMilliseconds(1000), elastic);
            }
        }

        private void ButtonContainer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _canRegisterClick = true;

            Holding?.Invoke(this, new EventArgs());

            Animate.Double(ButtonGrid.RenderTransform, ScaleTransform.ScaleXProperty, 0.9, TimeSpan.FromMilliseconds(1000), quadOut);
            Animate.Double(ButtonGrid.RenderTransform, ScaleTransform.ScaleYProperty, 0.9, TimeSpan.FromMilliseconds(1000), quadOut);

            ButtonContainer.MouseLeave += LocalMouseLeave;

            void LocalMouseLeave(object sender, MouseEventArgs e)
            {
                NoHovering?.Invoke(this, new EventArgs());

                Animate.Double(ButtonGrid.RenderTransform, ScaleTransform.ScaleXProperty, 1, TimeSpan.FromMilliseconds(1000), elastic);
                Animate.Double(ButtonGrid.RenderTransform, ScaleTransform.ScaleYProperty, 1, TimeSpan.FromMilliseconds(1000), elastic);

                ButtonContainer.MouseLeave -= LocalMouseLeave;
            }
        }

        private async void ButtonContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            NoHovering?.Invoke(this, new EventArgs());
            _canRegisterClick = false;
        }

        private async void ButtonContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            Hovering?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Sets default colors, and color animations for buttons, black when not hovered, grey when hovered, and white when clicked (red if disabled)).
        /// THIS REPLACES ANY BRUSH THAT THE BUTTON HAD.
        /// </summary>
        /// <param name="viperButton"></param>
        public static void SetDefaltBrushAnimations(ViperButton viperButton)
        {
            SolidColorBrush bdBrush = new(Color.FromRgb(80, 80, 80));
            SolidColorBrush bgBrush = new(Color.FromRgb(23, 23, 23));
            SolidColorBrush fgBrush = new(Color.FromRgb(255, 255, 255));

            viperButton.Background = bgBrush;
            viperButton.BorderBrush = bdBrush;
            viperButton.Foreground = fgBrush;

            viperButton.Holding += OnHolding;
            viperButton.Hovering += OnHover;
            viperButton.NoHovering += OnNoHover;
            viperButton.Release += OnRelease;

            void OnHolding(object sender, EventArgs e)
            {
                if (viperButton.IsEnabled)
                {
                    Animate.Color(bdBrush, SolidColorBrush.ColorProperty, Colors.White, TimeSpan.FromMilliseconds(100), new QuadraticEase());
                }
                else
                {
                    Animate.Color(bdBrush, SolidColorBrush.ColorProperty, Color.FromRgb(255, 99, 99), TimeSpan.FromMilliseconds(100), new QuadraticEase());
                }
            }

            void OnRelease(object sender, EventArgs e)
            {
                if (viperButton.IsEnabled)
                {
                    Animate.Color(bgBrush, SolidColorBrush.ColorProperty, Color.FromRgb(23, 23, 23), TimeSpan.FromMilliseconds(300), new QuadraticEase(), Colors.White);
                    Animate.Color(bdBrush, SolidColorBrush.ColorProperty, Color.FromRgb(80, 80, 80), TimeSpan.FromMilliseconds(300), new QuadraticEase());
                    Animate.Color(fgBrush, SolidColorBrush.ColorProperty, Color.FromRgb(255, 255, 255), TimeSpan.FromMilliseconds(300), new QuadraticEase(), Colors.Black);
                }
                else
                {
                    Animate.Color(bgBrush, SolidColorBrush.ColorProperty, Color.FromRgb(23, 23, 23), TimeSpan.FromMilliseconds(300), new QuadraticEase(), Color.FromRgb(255, 99, 99));
                    Animate.Color(bdBrush, SolidColorBrush.ColorProperty, Color.FromRgb(80, 80, 80), TimeSpan.FromMilliseconds(300), new QuadraticEase());
                    Animate.Color(fgBrush, SolidColorBrush.ColorProperty, Color.FromRgb(255, 255, 255), TimeSpan.FromMilliseconds(300), new QuadraticEase(), Color.FromRgb(255, 99, 99));
                }
            }

            void OnHover(object sender, EventArgs e)
            {
                Animate.Color(bgBrush, SolidColorBrush.ColorProperty, Color.FromRgb(40, 40, 40), TimeSpan.FromMilliseconds(200), new QuadraticEase());
            }

            void OnNoHover(object sender, EventArgs e)
            {
                Animate.Color(bgBrush, SolidColorBrush.ColorProperty, Color.FromRgb(23, 23, 23), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                Animate.Color(bdBrush, SolidColorBrush.ColorProperty, Color.FromRgb(80, 80, 80), TimeSpan.FromMilliseconds(300), new QuadraticEase());
            }
        }
    }
}
