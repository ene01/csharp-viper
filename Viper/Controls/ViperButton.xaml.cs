using System;
using System.Collections.Generic;
using System.Configuration;
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

        public static readonly DependencyProperty NoHoverBackgroundProperty = DependencyProperty.Register("NoHoverBackground", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(23, 23, 23)), OnBackgroundChanged));

        public static readonly DependencyProperty NoHoverBorderProperty = DependencyProperty.Register("NoHoverBorder", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(80, 80, 80)), OnBorderChanged));

        public static readonly DependencyProperty NoHoverForegroundProperty = DependencyProperty.Register("NoHoverForeground", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)), OnForegroundChanged));

        public static readonly DependencyProperty HoverBackgroundProperty = DependencyProperty.Register("HoverBackground", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(50, 50, 50)), OnHoverBackgroundChanged));

        public static readonly DependencyProperty HoverBorderProperty = DependencyProperty.Register("HoverBorder", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(90, 90, 90)), OnHoverBorderChanged));

        public static readonly DependencyProperty HoverForegroundProperty = DependencyProperty.Register("HoverForeground", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)), OnHoverForegroundChanged));

        public static readonly DependencyProperty ClickBackgroundProperty = DependencyProperty.Register("ClickBackground", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)), OnClickBackgroundChanged));

        public static readonly DependencyProperty ClickBorderProperty = DependencyProperty.Register("ClickBorder", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)), OnClickBorderChanged));

        public static readonly DependencyProperty ClickForegroundProperty = DependencyProperty.Register("ClickForeground", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0)), OnClickForegroundChanged));

        public static readonly DependencyProperty ContainerHeightProperty = DependencyProperty.Register("ButtonHeight", typeof(double), typeof(ViperButton), new PropertyMetadata(double.NaN, OnHeightChanged));

        public static readonly DependencyProperty ContainerWidthProperty = DependencyProperty.Register("ButtonWidth", typeof(double), typeof(ViperButton), new PropertyMetadata(double.NaN, OnWidthChanged));

        public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register("MarginPadding", typeof(Thickness), typeof(ViperButton), new PropertyMetadata(new Thickness(0, 0, 0, 0), OnMarginChanged));

        public static readonly DependencyProperty TransformsProperty = DependencyProperty.Register("Transforms", typeof(Transform), typeof(ViperButton), new PropertyMetadata(null, OnRenderTransformChanged));

        public static readonly DependencyProperty YAlignmentProperty = DependencyProperty.Register("YAlignment", typeof(System.Windows.VerticalAlignment), typeof(ViperButton), new PropertyMetadata(System.Windows.VerticalAlignment.Top, OnVerticalAlignmentChanged));

        public static readonly DependencyProperty XAlignmentProperty = DependencyProperty.Register("XAlignment", typeof(System.Windows.HorizontalAlignment), typeof(ViperButton), new PropertyMetadata(System.Windows.HorizontalAlignment.Left, OnHorizontalAlignmentChanged));

        public EventHandler Click;

        private object _composition = "ViperButton";
        private Brush _noHoverBackground = new SolidColorBrush(Color.FromRgb(23, 23, 23));
        private Brush _noHoverBorder = new SolidColorBrush(Color.FromRgb(80, 80, 80));
        private Brush _noHoverForeground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        private Brush _hoverBackground = new SolidColorBrush(Color.FromRgb(50, 50, 50));
        private Brush _hoverBorder = new SolidColorBrush(Color.FromRgb(90, 90, 90));
        private Brush _hoverForeground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        private Brush _clickBackground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        private Brush _clickBorder = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        private Brush _clickForeground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private double _containerHeight = double.NaN;
        private double _containerWidth = double.NaN;
        private Thickness _spacing = new Thickness(0, 0, 0, 0);
        private Transform _transforms = null;
        private VerticalAlignment _yAlignment = VerticalAlignment.Top;
        private HorizontalAlignment _xAlignment = HorizontalAlignment.Left;

        public object Composition
        {
            get { return _composition; }
            set { _composition = value; SetValue(CompositionProperty, value); }
        }

        public Brush NoHoverBackground
        {
            get { return _noHoverBackground; }
            set { _noHoverBackground = value; SetValue(NoHoverBackgroundProperty, value); }
        }

        public Brush NoHoverBorder
        {
            get { return _noHoverBorder; }
            set { _noHoverBorder = value; SetValue(NoHoverBorderProperty, value); }
        }

        public Brush NoHoverForeground
        {
            get { return _noHoverForeground; }
            set { _noHoverForeground = value; SetValue(NoHoverForegroundProperty, value); }
        }

        public Brush HoverBackground
        {
            get { return _hoverBackground; }
            set { _hoverBackground = value; SetValue(HoverBackgroundProperty, value); }
        }

        public Brush HoverBorder
        {
            get { return _hoverBorder; }
            set { _hoverBorder = value; SetValue(HoverBorderProperty, value); }
        }

        public Brush HoverForeground
        {
            get { return _hoverForeground; }
            set { _hoverForeground = value; SetValue(HoverForegroundProperty, value); }
        }

        public Brush ClickBackground
        {
            get { return _clickBackground; }
            set { _clickBackground = value; SetValue(ClickBackgroundProperty, value); }
        }

        public Brush ClickBorder
        {
            get { return _clickBorder; }
            set { _clickBorder = value; SetValue(ClickBorderProperty, value); }
        }

        public Brush ClickForeground
        {
            get { return _clickForeground; }
            set { _clickForeground = value; SetValue(ClickForegroundProperty, value); }
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

        private static void OnHoverBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnHoverBackgroundChanged(e.OldValue, e.NewValue);
        }

        private static void OnHoverBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnHoverBorderChanged(e.OldValue, e.NewValue);
        }

        private static void OnHoverForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnHoverForegroundChanged(e.OldValue, e.NewValue);
        }

        private static void OnClickBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnClickBackgroundChanged(e.OldValue, e.NewValue);
        }

        private static void OnClickBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnClickBorderChanged(e.OldValue, e.NewValue);
        }

        private static void OnClickForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ViperButton)d;
            control.OnClickForegroundChanged(e.OldValue, e.NewValue);
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
                ButtonContent.Content = (object)newValue;
            }
        }

        protected virtual void OnBackgroundChanged(object oldValue, object newValue)
        {
            ButtonRectangle.Fill = (Brush)newValue;
        }

        protected virtual void OnBorderChanged(object oldValue, object newValue)
        {
            ButtonRectangle.Stroke = (Brush)newValue;
        }

        protected virtual void OnForegroundChanged(object oldValue, object newValue)
        {
            ButtonContent.Foreground = (Brush)newValue;
        }

        protected virtual void OnHoverBackgroundChanged(object oldValue, object newValue)
        {
            ButtonRectangle.Fill = (Brush)newValue;
        }

        protected virtual void OnHoverBorderChanged(object oldValue, object newValue)
        {
            ButtonRectangle.Stroke = (Brush)newValue;
        }

        protected virtual void OnHoverForegroundChanged(object oldValue, object newValue)
        {
            ButtonContent.Foreground = (Brush)newValue;
        }

        protected virtual void OnClickBackgroundChanged(object oldValue, object newValue)
        {
            ButtonRectangle.Fill = (Brush)newValue;
        }

        protected virtual void OnClickBorderChanged(object oldValue, object newValue)
        {
            ButtonRectangle.Stroke = (Brush)newValue;
        }

        protected virtual void OnClickForegroundChanged(object oldValue, object newValue)
        {
            ButtonContent.Foreground = (Brush)newValue;
        }

        protected virtual void OnHeightChanged(object oldValue, object newValue)
        {
            ButtonContainer.Height = (double)newValue;
        }

        protected virtual void OnWidthChanged(object oldValue, object newValue)
        {
            ButtonContainer.Width = (double)newValue;
        }

        protected virtual void OnMarginChanged(object oldValue, object newValue)
        {
            ButtonContainer.Margin = (Thickness)newValue;
        }

        protected virtual void OnRenderTransformChanged(object oldValue, object newValue)
        {
            ButtonContainer.RenderTransform = (Transform)newValue;
        }

        protected virtual void OnVerticalAlignmentChanged(object oldValue, object newValue)
        {
            ButtonContainer.VerticalAlignment = (VerticalAlignment)newValue;
        }

        protected virtual void OnHorizontalAlignmentChanged(object oldValue, object newValue)
        {
            ButtonContainer.HorizontalAlignment = (HorizontalAlignment)newValue;
        }

        public ViperButton()
        {
            InitializeComponent();

            ButtonRectangle.RenderTransform = new ScaleTransform();

            ButtonContainer.MouseEnter += ButtonContainer_MouseEnter;
            ButtonContainer.MouseLeave += ButtonContainer_MouseLeave;
            ButtonContainer.PreviewMouseLeftButtonDown += ButtonContainer_PreviewMouseLeftButtonDown;
            ButtonContainer.PreviewMouseLeftButtonUp += ButtonContainer_PreviewMouseLeftButtonUp;
        }

        private IEasingFunction elastic = new ElasticEase() { Springiness = 12 };
        private IEasingFunction quadIn = new QuadraticEase() { EasingMode = EasingMode.EaseIn };
        private IEasingFunction quadOut = new QuadraticEase() { EasingMode = EasingMode.EaseOut };

        private void ButtonContainer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Click?.Invoke(this, new EventArgs());

            Animate.Color(ButtonRectangle.Fill, SolidColorBrush.ColorProperty, (HoverBackground as SolidColorBrush).Color, TimeSpan.FromMilliseconds(300), quadOut);
            Animate.Color(ButtonRectangle.Stroke, SolidColorBrush.ColorProperty, (HoverBorder as SolidColorBrush).Color, TimeSpan.FromMilliseconds(300), quadOut);
            Animate.Color(ButtonContent.Foreground, SolidColorBrush.ColorProperty, (HoverForeground as SolidColorBrush).Color, TimeSpan.FromMilliseconds(300), quadOut);
            Animate.Double(ButtonRectangle.RenderTransform, ScaleTransform.ScaleXProperty, 1, TimeSpan.FromMilliseconds(400), quadOut);
            Animate.Double(ButtonRectangle.RenderTransform, ScaleTransform.ScaleYProperty, 1, TimeSpan.FromMilliseconds(400), quadOut);
        }

        private void ButtonContainer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Animate.Color(ButtonRectangle.Fill, SolidColorBrush.ColorProperty, (ClickBackground as SolidColorBrush).Color, TimeSpan.FromMilliseconds(100), quadOut);
            Animate.Color(ButtonRectangle.Stroke, SolidColorBrush.ColorProperty, (ClickBorder as SolidColorBrush).Color, TimeSpan.FromMilliseconds(100), quadOut);
            Animate.Color(ButtonContent.Foreground, SolidColorBrush.ColorProperty, (ClickForeground as SolidColorBrush).Color, TimeSpan.FromMilliseconds(100), quadOut);
            Animate.Double(ButtonRectangle.RenderTransform, ScaleTransform.ScaleXProperty, 0.5, TimeSpan.FromMilliseconds(1000), quadOut);
            Animate.Double(ButtonRectangle.RenderTransform, ScaleTransform.ScaleYProperty, 0.5, TimeSpan.FromMilliseconds(1000), quadOut);
        }

        private void ButtonContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            Animate.Color(ButtonRectangle.Fill, SolidColorBrush.ColorProperty, (NoHoverBackground as SolidColorBrush).Color, TimeSpan.FromMilliseconds(500), quadIn);
            Animate.Color(ButtonRectangle.Stroke, SolidColorBrush.ColorProperty, (NoHoverBorder as SolidColorBrush).Color, TimeSpan.FromMilliseconds(500), quadIn);
            Animate.Color(ButtonContent.Foreground, SolidColorBrush.ColorProperty, (NoHoverForeground as SolidColorBrush).Color, TimeSpan.FromMilliseconds(500), quadIn);
        }

        private void ButtonContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            Animate.Color(ButtonRectangle.Fill, SolidColorBrush.ColorProperty, (HoverBackground as SolidColorBrush).Color, TimeSpan.FromMilliseconds(300), quadOut);
            Animate.Color(ButtonRectangle.Stroke, SolidColorBrush.ColorProperty, (HoverBorder as SolidColorBrush).Color, TimeSpan.FromMilliseconds(300), quadOut);
            Animate.Color(ButtonContent.Foreground, SolidColorBrush.ColorProperty, (HoverForeground as SolidColorBrush).Color, TimeSpan.FromMilliseconds(300), quadOut);
        }
    }
}
