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

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(ViperButton), new PropertyMetadata("ViperButton", OnContentChanged));

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(23, 23, 23)), OnBackgroundChanged));

        public static readonly DependencyProperty BorderProperty = DependencyProperty.Register("Border", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(80, 80, 80)), OnBorderChanged));

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)), OnForegroundChanged));

        public static readonly DependencyProperty HoverBackgroundProperty = DependencyProperty.Register("HoverBackground", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(50, 50, 50)), OnHoverBackgroundChanged));

        public static readonly DependencyProperty HoverBorderProperty = DependencyProperty.Register("HoverBorder", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(90, 90, 90)), OnHoverBorderChanged));

        public static readonly DependencyProperty HoverForegroundProperty = DependencyProperty.Register("HoverForeground", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)), OnHoverForegroundChanged));

        public static readonly DependencyProperty ClickBackgroundProperty = DependencyProperty.Register("ClickBackground", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)), OnClickBackgroundChanged));

        public static readonly DependencyProperty ClickBorderProperty = DependencyProperty.Register("ClickBorder", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)), OnClickBorderChanged));

        public static readonly DependencyProperty ClickForegroundProperty = DependencyProperty.Register("ClickForeground", typeof(Brush), typeof(ViperButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0)), OnClickForegroundChanged));

        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(double), typeof(ViperButton), new PropertyMetadata(double.NaN, OnHeightChanged));

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(ViperButton), new PropertyMetadata(double.NaN, OnWidthChanged));

        public static readonly DependencyProperty MarginProperty = DependencyProperty.Register("Margin", typeof(Thickness), typeof(ViperButton), new PropertyMetadata(new Thickness(0, 0, 0, 0), OnMarginChanged));

        public static readonly DependencyProperty RenderTransformProperty = DependencyProperty.Register("RenderTransform", typeof(Transform), typeof(ViperButton), new PropertyMetadata(null, OnRenderTransformChanged));

        public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.Register("VerticalAlignment", typeof(System.Windows.VerticalAlignment), typeof(ViperButton), new PropertyMetadata(System.Windows.VerticalAlignment.Top, OnVerticalAlignmentChanged));

        public static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.Register("HorizontalAlignment", typeof(System.Windows.HorizontalAlignment), typeof(ViperButton), new PropertyMetadata(System.Windows.HorizontalAlignment.Left, OnHorizontalAlignmentChanged));

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public Brush Border
        {
            get { return (Brush)GetValue(BorderProperty); }
            set { SetValue(BorderProperty, value); }
        }

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public Brush HoverBackground
        {
            get { return (Brush)GetValue(HoverBackgroundProperty); }
            set { SetValue(HoverBackgroundProperty, value); }
        }

        public Brush HoverBorder
        {
            get { return (Brush)GetValue(HoverBorderProperty); }
            set { SetValue(HoverBorderProperty, value); }
        }

        public Brush HoverForeground
        {
            get { return (Brush)GetValue(HoverForegroundProperty); }
            set { SetValue(HoverForegroundProperty, value); }
        }

        public Brush ClickBackground
        {
            get { return (Brush)GetValue(ClickBackgroundProperty); }
            set { SetValue(ClickBackgroundProperty, value); }
        }

        public Brush ClickBorder
        {
            get { return (Brush)GetValue(ClickBorderProperty); }
            set { SetValue(ClickBorderProperty, value); }
        }

        public Brush ClickForeground
        {
            get { return (Brush)GetValue(ClickForegroundProperty); }
            set { SetValue(ClickForegroundProperty, value); }
        }

        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }

        public Transform RenderTransform
        {
            get { return (Transform)GetValue(RenderTransformProperty); }
            set { SetValue(RenderTransformProperty, value); }
        }

        public VerticalAlignment VerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalAlignmentProperty); }
            set { SetValue(VerticalAlignmentProperty, value); }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalAlignmentProperty); }
            set { SetValue(HorizontalAlignmentProperty, value); }
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
            ButtonContent.Content = (object)newValue;
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
            ButtonRectangle.Stroke = (Brush)newValue;
        }

        protected virtual void OnWidthChanged(object oldValue, object newValue)
        {
            ButtonContent.Foreground = (Brush)newValue;
        }

        protected virtual void OnMarginChanged(object oldValue, object newValue)
        {
            ButtonRectangle.Stroke = (Brush)newValue;
        }

        protected virtual void OnRenderTransformChanged(object oldValue, object newValue)
        {
            ButtonContent.Foreground = (Brush)newValue;
        }

        protected virtual void OnVerticalAlignmentChanged(object oldValue, object newValue)
        {
            ButtonRectangle.Stroke = (Brush)newValue;
        }

        protected virtual void OnHorizontalAlignmentChanged(object oldValue, object newValue)
        {
            ButtonContent.Foreground = (Brush)newValue;
        }

        public ViperButton()
        {
            InitializeComponent();

            ButtonContainer.MouseEnter += ButtonContainer_MouseEnter;
            ButtonContainer.MouseLeave += ButtonContainer_MouseLeave;
            ButtonContainer.PreviewMouseLeftButtonDown += ButtonContainer_PreviewMouseLeftButtonDown;
            ButtonContainer.PreviewMouseLeftButtonUp += ButtonContainer_PreviewMouseLeftButtonUp;
        }

        private void ButtonContainer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ButtonRectangle.Fill = HoverBackground;
            ButtonRectangle.Stroke = HoverBorder;
            ButtonContent.Foreground = HoverForeground;
        }

        private void ButtonContainer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ButtonRectangle.Fill = ClickBackground;
            ButtonRectangle.Stroke = ClickBorder;
            ButtonContent.Foreground = ClickForeground;
        }

        private void ButtonContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonRectangle.Fill = Background;
            ButtonRectangle.Stroke = Border;
            ButtonContent.Foreground = Foreground;
        }

        private void ButtonContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            ButtonRectangle.Fill = HoverBackground;
            ButtonRectangle.Stroke = HoverBorder;
            ButtonContent.Foreground = HoverForeground;
        }
    }
}
