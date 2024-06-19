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

        public static readonly DependencyProperty CompositionProperty = DependencyProperty.Register("Composition", typeof(object), typeof(ViperButton), new PropertyMetadata("ViperButton", OnContentChanged));

        public static readonly DependencyProperty ButtonBackgroundProperty = DependencyProperty.Register("NoHoverBackground", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(23, 23, 23)), OnBackgroundChanged));

        public static readonly DependencyProperty ButtonBorderProperty = DependencyProperty.Register("NoHoverBorder", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(80, 80, 80)), OnBorderChanged));

        public static readonly DependencyProperty ButtonForegroundProperty = DependencyProperty.Register("NoHoverForeground", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)), OnForegroundChanged));

        public static readonly DependencyProperty ContainerHeightProperty = DependencyProperty.Register("ButtonHeight", typeof(double), typeof(ViperButton), new PropertyMetadata(double.NaN, OnHeightChanged));

        public static readonly DependencyProperty ContainerWidthProperty = DependencyProperty.Register("ButtonWidth", typeof(double), typeof(ViperButton), new PropertyMetadata(double.NaN, OnWidthChanged));

        public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register("MarginPadding", typeof(Thickness), typeof(ViperButton), new PropertyMetadata(new Thickness(0, 0, 0, 0), OnMarginChanged));

        public static readonly DependencyProperty TransformsProperty = DependencyProperty.Register("Transforms", typeof(Transform), typeof(ViperButton), new PropertyMetadata(null, OnRenderTransformChanged));

        public static readonly DependencyProperty YAlignmentProperty = DependencyProperty.Register("YAlignment", typeof(System.Windows.VerticalAlignment), typeof(ViperButton), new PropertyMetadata(System.Windows.VerticalAlignment.Top, OnVerticalAlignmentChanged));

        public static readonly DependencyProperty XAlignmentProperty = DependencyProperty.Register("XAlignment", typeof(System.Windows.HorizontalAlignment), typeof(ViperButton), new PropertyMetadata(System.Windows.HorizontalAlignment.Left, OnHorizontalAlignmentChanged));

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(ViperButton), new PropertyMetadata(true, OnEnabledChanged));

        /// <summary>
        /// Events that triggers when the button is enabled and clicked.
        /// </summary>
        public EventHandler Click;

        /// <summary>
        /// Event that triggers when the button is pressed, can trigger even if the button is disabled.
        /// </summary>
        public EventHandler Holding;

        /// <summary>
        /// Event that triggers when the button is released, can trigger even if the button is disabled.
        /// </summary>
        public EventHandler Release;

        /// <summary>
        /// Event that triggers when the mouse is being hovered over the button.
        /// </summary>
        public EventHandler Hovering;

        /// <summary>
        /// Event that triggers when the mouse is no longer hovering the button.
        /// </summary>
        public EventHandler NoHovering;

        private object _composition = "ViperButton";
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

        public object Composition
        {
            get { return _composition; }
            set { _composition = value; SetValue(CompositionProperty, value); }
        }

        public Brush ButtonBackground
        {
            get { return _background; }
            set { _background = value; SetValue(ButtonBackgroundProperty, value); }
        }

        public Brush ButtonBorder
        {
            get { return _border; }
            set { _border = value; SetValue(ButtonBorderProperty, value); }
        }

        public Brush ButtonForeground
        {
            get { return _foreground; }
            set { _foreground = value; SetValue(ButtonForegroundProperty, value); }
        }

        public double ContainerHeight
        {
            get { return _containerHeight; }
            set { _containerHeight = value; SetValue(ContainerHeightProperty, value); }
        }

        public double ContainerWidth
        {
            get { return _containerWidth; }
            set { _containerWidth = value; SetValue(ContainerWidthProperty, value); }
        }

        public Thickness Spacing
        {
            get { return _spacing; }
            set { _spacing = value; SetValue(SpacingProperty, value); }
        }

        public Transform Transforms
        {
            get { return _transforms; }
            set { _transforms = value; SetValue(TransformsProperty, value); }
        }

        public VerticalAlignment YAlignment
        {
            get { return _yAlignment; }
            set { _yAlignment = value; SetValue(YAlignmentProperty, value); }
        }

        public HorizontalAlignment XAlignment
        {
            get { return _xAlignment; }
            set { _xAlignment = value; SetValue(XAlignmentProperty, value); }
        }

        public bool Enabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; SetValue(IsEnabledProperty, value); }
        }

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnContentChanged(e.OldValue, e.NewValue);
        }

        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnBackgroundChanged(e.OldValue, e.NewValue);
        }

        private static void OnBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnBorderChanged(e.OldValue, e.NewValue);
        }

        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnForegroundChanged(e.OldValue, e.NewValue);
        }

        private static void OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnHeightChanged(e.OldValue, e.NewValue);
        }

        private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnWidthChanged(e.OldValue, e.NewValue);
        }

        private static void OnMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnMarginChanged(e.OldValue, e.NewValue);
        }

        private static void OnRenderTransformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnRenderTransformChanged(e.OldValue, e.NewValue);
        }

        private static void OnVerticalAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnVerticalAlignmentChanged(e.OldValue, e.NewValue);
        }

        private static void OnHorizontalAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnHorizontalAlignmentChanged(e.OldValue, e.NewValue);
        }

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnHorizontalAlignmentChanged(e.OldValue, e.NewValue);
        }

        protected virtual void OnContentChanged(object oldValue, object newValue)
        {
            if (newValue is string)
            {
                TextBlock text = new()
                {
                    Text = (string)newValue,
                    TextWrapping = TextWrapping.WrapWithOverflow,
                };

                ButtonContent.Content = text;
            }
            else
            {
                ButtonContent.Content = _contentLoaded;
            }
        }

        protected virtual void OnBackgroundChanged(object oldValue, object newValue)
        {
            ButtonRectangle.Fill = _background;
        }

        protected virtual void OnBorderChanged(object oldValue, object newValue)
        {
            ButtonRectangle.Stroke = _border;
        }

        protected virtual void OnForegroundChanged(object oldValue, object newValue)
        {
            ButtonContent.Foreground = _foreground;
        }

        protected virtual void OnHeightChanged(object oldValue, object newValue)
        {
            ButtonContainer.Height = _containerHeight;
        }

        protected virtual void OnWidthChanged(object oldValue, object newValue)
        {
            ButtonContainer.Width = _containerWidth;
        }

        protected virtual void OnMarginChanged(object oldValue, object newValue)
        {
            ButtonContainer.Margin = _spacing;
        }

        protected virtual void OnRenderTransformChanged(object oldValue, object newValue)
        {
            ButtonContainer.RenderTransform = _transforms;
        }

        protected virtual void OnVerticalAlignmentChanged(object oldValue, object newValue)
        {
            ButtonContainer.VerticalAlignment =_yAlignment;
        }

        protected virtual void OnHorizontalAlignmentChanged(object oldValue, object newValue)
        {
            ButtonContainer.HorizontalAlignment = _xAlignment;
        }

        protected virtual void OnEnabledChanged(object oldValue, object newValue)
        {
            EnabledLayerToggle();
        }

        private IEasingFunction elastic = new ElasticEase() { Springiness = 5 };
        private IEasingFunction quadOut = new QuadraticEase() { EasingMode = EasingMode.EaseOut };

        private double _actualHeight = 0, _actualWidth = 0;

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

        private void EnabledLayerToggle()
        {
            if (_isEnabled)
            {
                EnabledLayer.IsHitTestVisible = false;
                Animate.Double(EnabledLayer, OpacityProperty, 0, TimeSpan.FromMilliseconds(200));
            }
            else
            {
                EnabledLayer.IsHitTestVisible = true;
                Animate.Double(EnabledLayer, OpacityProperty, 0.3, TimeSpan.FromMilliseconds(200));
            }
        }

        private void ViperButton_Loaded(object sender, RoutedEventArgs e)
        {
            ButtonGrid.SizeChanged += ButtonGrid_SizeChanged;

            ButtonGrid.RenderTransform = new ScaleTransform(1, 1) { CenterX = ButtonGrid.ActualWidth / 2, CenterY = ButtonGrid.ActualHeight / 2 };
            EnabledLayerToggle();
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
    }
}
