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
    public partial class ViperComboBox : UserControl
    {
        /// <summary>
        /// Im lazy, so heres the entire control container, do whatrever, use events, etc, idk.
        /// </summary>
        public UserControl Container => ComboBoxContainer;

        /// <summary>
        /// Event that triggers when the combo box is pressed, can trigger even if the combo box is disabled.
        /// </summary>
        public EventHandler? Holding;

        /// <summary>
        /// Event that triggers when the combo box is released, can trigger even if the combo box is disabled.
        /// </summary>
        public EventHandler? Release;

        /// <summary>aaaaaaa
        /// Event that triggers when the mouse is being hovered over the combo box.
        /// </summary>
        public EventHandler? Hovering;

        /// <summary>
        /// Event that triggers when the mouse is no longer hovering the combo box.
        /// </summary>
        public EventHandler? NoHovering;

        public EventHandler<ViperComboBoxStateChanged> StateChanged;

        private object _content = "ViperCheckBox";
        private Brush _background = new SolidColorBrush(Color.FromRgb(23, 23, 23));
        private Brush _border = new SolidColorBrush(Color.FromRgb(80, 80, 80));
        private Brush _foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        private Brush _itemsBackground = new SolidColorBrush(Color.FromRgb(12, 12, 12));
        private double _stuffGridHeight = double.NaN;
        private double _stuffGridWidth = double.NaN;
        private Thickness _spacing = new Thickness(0, 0, 0, 0);
        private Transform _transforms = null;
        private VerticalAlignment _yAlignment = VerticalAlignment.Top;
        private HorizontalAlignment _xAlignment = HorizontalAlignment.Left;
        private bool _isEnabled = true;
        private bool _isOpen = false;

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

                    ComboBoxContent.Content = text;
                }
                else
                {
                    ComboBoxContent.Content = value;
                }
            }
        }

        public new Brush Background
        {
            get { return _background; }

            set
            {
                _background = value;

                ComboBoxStuffGrid.Background = value;
            }
        }

        public Brush BorderBrush
        {
            get { return _border; }

            set
            {
                _border = value;

                ComboBoxBorder.BorderBrush = value;
            }
        }

        public new Brush Foreground
        {
            get { return _foreground; }

            set 
            { 
                _foreground = value;

                ComboBoxContent.Foreground = value;
            }
        }

        public Brush ItemsBackground
        {
            get { return _itemsBackground; }

            set
            {
                _itemsBackground = value;

                ItemStackPanel.Background = value;
            }
        }

        public new double Height
        {
            get { return _stuffGridHeight; }

            set 
            {
                _stuffGridHeight = value;

                ComboBoxStuffGrid.Height = value;
            }
        }

        public new double Width
        {
            get { return _stuffGridWidth; }

            set 
            {
                _stuffGridWidth = value;

                ComboBoxStuffGrid.Width = value;
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

        public new bool IsOpen
        {
            get { return _isOpen; }

            set
            {
                _isOpen = value;

                ItemDisplayToggle(value);
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

        private IEasingFunction elastic = new ElasticEase() { Springiness = 4, Oscillations = 2 };
        private IEasingFunction quadOut = new QuadraticEase() { EasingMode = EasingMode.EaseOut };

        public ViperComboBox()
        {
            InitializeComponent();

            RootGrid.RenderTransform = new ScaleTransform();
            SillyThing.RenderTransform = new RotateTransform(132);

            Loaded += ViperCheckBox_Loaded;
            Unloaded += ViperCheckBox_Unloaded;
            ComboBoxContainer.LostFocus += ViperCheckBox_LostFocus;

            ComboBoxStuffGrid.PreviewMouseLeftButtonDown += ComboBoxContainer_PreviewMouseLeftButtonDown;
            ComboBoxStuffGrid.PreviewMouseLeftButtonUp += CheckBoxContainer_PreviewMouseLeftButtonUp;
            ComboBoxContainer.MouseEnter += ComboBoxContainer_MouseEnter;
            ComboBoxContainer.MouseLeave += ComboBoxContainer_MouseLeave;
        }

        private void ComboBoxContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            NoHovering?.Invoke(this, new EventArgs());
        }

        private void ComboBoxContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            Hovering?.Invoke(this, new EventArgs());
        }

        private void ComboBoxContainer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Holding?.Invoke(this, new EventArgs());

            ComboBoxContainer.MouseLeave += LocalMouseLeave;

            Animate.Double(RootGrid.RenderTransform, ScaleTransform.ScaleXProperty, 0.9, TimeSpan.FromMilliseconds(1000), quadOut);
            Animate.Double(RootGrid.RenderTransform, ScaleTransform.ScaleYProperty, 0.9, TimeSpan.FromMilliseconds(1000), quadOut);

            void LocalMouseLeave(object sender, MouseEventArgs e)
            {
                ComboBoxContainer.MouseLeave -= LocalMouseLeave;

                Animate.Double(RootGrid.RenderTransform, ScaleTransform.ScaleXProperty, 1, TimeSpan.FromMilliseconds(1000), elastic);
                Animate.Double(RootGrid.RenderTransform, ScaleTransform.ScaleYProperty, 1, TimeSpan.FromMilliseconds(1000), elastic);

                Release?.Invoke(this, new EventArgs());
            }
        }

        private void ViperCheckBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Release?.Invoke(this, new EventArgs());
        }

        private void ViperCheckBox_Unloaded(object sender, RoutedEventArgs e)
        {
            ComboBoxContainer.LostFocus -= ViperCheckBox_LostFocus;
            ComboBoxContainer.PreviewMouseLeftButtonDown -= ComboBoxContainer_PreviewMouseLeftButtonDown;
            ComboBoxContainer.PreviewMouseLeftButtonUp -= CheckBoxContainer_PreviewMouseLeftButtonUp;
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

        private void ViperCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            RootGrid.SizeChanged += RootGrid_SizeChanged;

            RootGrid.RenderTransform = new ScaleTransform(1, 1) { CenterX = RootGrid.ActualWidth / 2, CenterY = RootGrid.ActualHeight / 2 };

            void RootGrid_SizeChanged(object sender, SizeChangedEventArgs e)
            {
                if (!_isOpen)
                {
                    RootGrid.RenderTransform = new ScaleTransform(1, 1) { CenterX = RootGrid.ActualWidth / 2, CenterY = RootGrid.ActualHeight / 2 };
                }
            }

            EnabledLayerToggle(_isEnabled);
        }

        private void CheckBoxContainer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Animate.Double(RootGrid.RenderTransform, ScaleTransform.ScaleXProperty, 1, TimeSpan.FromMilliseconds(1000), elastic);
            Animate.Double(RootGrid.RenderTransform, ScaleTransform.ScaleYProperty, 1, TimeSpan.FromMilliseconds(1000), elastic);

            if (_isEnabled)
            {
                _isOpen = !_isOpen;

                ItemDisplayToggle(_isOpen);
            }

            Release?.Invoke(this, new EventArgs());
        }

        private void ItemDisplayToggle(bool openOrClose)
        {
            StateChanged?.Invoke(this, new ViperComboBoxStateChanged(openOrClose));

            if (openOrClose)
            {
                Animate.Double(ItemStackPanel, FrameworkElement.HeightProperty, 50, TimeSpan.FromMilliseconds(600), elastic);
                Animate.Double(SillyThing.RenderTransform, RotateTransform.AngleProperty, 313, TimeSpan.FromMilliseconds(200), quadOut);
                Animate.Color(SillyThing.Stroke, SolidColorBrush.ColorProperty, Color.FromArgb(120, 0, 0, 0), TimeSpan.FromMilliseconds(200), quadOut);
            }
            else
            {
                Animate.Double(ItemStackPanel, FrameworkElement.HeightProperty, 0, TimeSpan.FromMilliseconds(200), quadOut);
                Animate.Double(SillyThing.RenderTransform, RotateTransform.AngleProperty, 132, TimeSpan.FromMilliseconds(200), quadOut);
                Animate.Color(SillyThing.Stroke, SolidColorBrush.ColorProperty, Color.FromArgb(40, 255, 255, 255), TimeSpan.FromMilliseconds(200), quadOut);
            }
        }

        /// <summary>
        /// Sets default colors, and color animations for buttons, black when not hovered, grey when hovered, and white when clicked (red if disabled)).
        /// THIS REPLACES ANY BRUSH THAT THE BUTTON HAD.
        /// </summary>
        /// <param name="viperButton"></param>
        public static void SetDefaltCheckAnimations(ViperComboBox viperComboBox)
        {
            SolidColorBrush bgBrush = new(Color.FromRgb(23, 23, 23));
            SolidColorBrush bdBrush = new(Color.FromRgb(80, 80, 80));
            SolidColorBrush fgBrush = new(Color.FromRgb(255, 255, 255));

            viperComboBox.BorderBrush = bdBrush;
            viperComboBox.Background = bgBrush;
            viperComboBox.Foreground = fgBrush;

            viperComboBox.StateChanged+= OnStateChanged;
            viperComboBox.Hovering += OnHover;
            viperComboBox.NoHovering += OnNoHover;

            void OnStateChanged(object sender, ViperComboBoxStateChanged e)
            {
                if (e.IsOpen)
                {
                    Animate.Color(bgBrush, SolidColorBrush.ColorProperty, Color.FromRgb(255, 255, 255), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                    Animate.Color(fgBrush, SolidColorBrush.ColorProperty, Color.FromRgb(0, 0, 0), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                    Animate.Color(bdBrush, SolidColorBrush.ColorProperty, Color.FromRgb(255, 255, 255), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                }
                else
                {
                    Animate.Color(bgBrush, SolidColorBrush.ColorProperty, Color.FromRgb(40, 40, 40), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                    Animate.Color(fgBrush, SolidColorBrush.ColorProperty, Color.FromRgb(255, 255, 255), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                    Animate.Color(bdBrush, SolidColorBrush.ColorProperty, Color.FromRgb(80, 80, 80), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                }
            }

            void OnHover(object sender, EventArgs e)
            {
                if (!viperComboBox.IsOpen)
                {
                    Animate.Color(bgBrush, SolidColorBrush.ColorProperty, Color.FromRgb(40, 40, 40), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                }
            }

            void OnNoHover(object sender, EventArgs e)
            {
                if (!viperComboBox.IsOpen)
                {
                    Animate.Color(bgBrush, SolidColorBrush.ColorProperty, Color.FromRgb(23, 23, 23), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                }
            }
        }
    }
}
