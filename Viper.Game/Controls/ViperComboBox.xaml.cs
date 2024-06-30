using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

        public EventHandler<ViperComboBoxSelectionChanged> SelectionChanged;

        private const string CONTENT_VIPER_CHECK_BOX = "Nothing selected";
        private const byte BACKGROUND_COLOR_R = 23;
        private const byte BACKGROUND_COLOR_G = 23;
        private const byte BACKGROUND_COLOR_B = 23;
        private const byte BORDER_COLOR_R = 80;
        private const byte BORDER_COLOR_G = 80;
        private const byte BORDER_COLOR_B = 80;
        private const byte FOREGROUND_COLOR_R = 255;
        private const byte FOREGROUND_COLOR_G = 255;
        private const byte FOREGROUND_COLOR_B = 255;
        private const byte ITEMS_BACKGROUND_COLOR_R = 12;
        private const byte ITEMS_BACKGROUND_COLOR_G = 12;
        private const byte ITEMS_BACKGROUND_COLOR_B = 12;
        private const byte ITEMS_DISPLAYERS_COLOR_R = 30;
        private const byte ITEMS_DISPLAYERS_COLOR_G = 30;
        private const byte ITEMS_DISPLAYERS_COLOR_B = 30;
        private const double DEFAULT_GRID_HEIGHT = double.NaN;
        private const double DEFAULT_GRID_WIDTH = double.NaN;
        private const double ITEM_CONT_MAX_HEIGHT = 100;
        private const double THICKNESS_LEFT = 0;
        private const double THICKNESS_TOP = 0;
        private const double THICKNESS_RIGHT = 0;
        private const double THICKNESS_BOTTOM = 0;
        private const VerticalAlignment DEFAULT_VERTICAL_ALIGNMENT = VerticalAlignment.Top;
        private const HorizontalAlignment DEFAULT_HORIZONTAL_ALIGNMENT = HorizontalAlignment.Left;
        private const bool DEFAULT_IS_ENABLED = true;
        private const bool DEFAULT_IS_OPEN = false;

        private object _content = CONTENT_VIPER_CHECK_BOX;
        private Brush _background = new SolidColorBrush(Color.FromRgb(BACKGROUND_COLOR_R, BACKGROUND_COLOR_G, BACKGROUND_COLOR_B));
        private Brush _border = new SolidColorBrush(Color.FromRgb(BORDER_COLOR_R, BORDER_COLOR_G, BORDER_COLOR_B));
        private Brush _foreground = new SolidColorBrush(Color.FromRgb(FOREGROUND_COLOR_R, FOREGROUND_COLOR_G, FOREGROUND_COLOR_B));
        private Brush _itemsBackground = new SolidColorBrush(Color.FromRgb(ITEMS_BACKGROUND_COLOR_R, ITEMS_BACKGROUND_COLOR_G, ITEMS_BACKGROUND_COLOR_B));
        private double _stuffGridHeight = DEFAULT_GRID_HEIGHT;
        private double _stuffGridWidth = DEFAULT_GRID_WIDTH;
        private double _itemContMaxHeight = ITEM_CONT_MAX_HEIGHT;
        private Thickness _spacing = new Thickness(THICKNESS_LEFT, THICKNESS_TOP, THICKNESS_RIGHT, THICKNESS_BOTTOM);
        private Transform _transforms = null;
        private VerticalAlignment _yAlignment = DEFAULT_VERTICAL_ALIGNMENT;
        private HorizontalAlignment _xAlignment = DEFAULT_HORIZONTAL_ALIGNMENT;
        private bool _isEnabled = DEFAULT_IS_ENABLED;
        private bool _isOpen = DEFAULT_IS_OPEN;
        private Brush _itemDisplayersBrush = new SolidColorBrush(Color.FromRgb(ITEMS_DISPLAYERS_COLOR_R, ITEMS_DISPLAYERS_COLOR_G, ITEMS_DISPLAYERS_COLOR_B));

        public new object Content
        {
            get { return _content; }

            set
            {
                _content = value;

                if (value is string)
                {
                    ComboBoxContent.Content = WrapWithOverflowTextBlock(value as string, _foreground);
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

        public Brush ItemsDisplayersBackground
        {
            get { return _itemDisplayersBrush; }

            set
            {
                SetDisplayerBrushes(value);
            }
        }

        public new double ItemContainerMaxHeight
        {
            get { return _itemContMaxHeight; }

            set
            {
                _itemContMaxHeight = value;
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
                RootGrid.VerticalAlignment = value;
                ComboBoxContainerStackPanel.VerticalAlignment = value;
                ComboBoxStuffGrid.VerticalAlignment = value;
            }
        }

        public new HorizontalAlignment HorizontalAlignment
        {
            get { return _xAlignment; }

            set 
            { 
                _xAlignment = value;

                Container.HorizontalAlignment = value;
                RootGrid.HorizontalAlignment = value;
                ComboBoxContainerStackPanel.HorizontalAlignment = value;
                ComboBoxStuffGrid.HorizontalAlignment = value;
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

                    Background = new SolidColorBrush(Color.FromRgb(BACKGROUND_COLOR_R, BACKGROUND_COLOR_G, BACKGROUND_COLOR_B));
                    BorderBrush = new SolidColorBrush(Color.FromRgb(BORDER_COLOR_R, BORDER_COLOR_G, BORDER_COLOR_B));
                    Foreground = new SolidColorBrush(Color.FromRgb(FOREGROUND_COLOR_R, FOREGROUND_COLOR_G, FOREGROUND_COLOR_B));
                    ItemsBackground = new SolidColorBrush(Color.FromRgb(ITEMS_BACKGROUND_COLOR_R, ITEMS_BACKGROUND_COLOR_G, ITEMS_BACKGROUND_COLOR_B));
                }
            }
        }

        private ScrollViewer _itemScroll = new()
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
            Height = 0,
        };

        private StackPanel _itemStackPanelList = new()
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        private List<object> _items = new();
        private List<Grid> _itemDisplayers = new();

        private int ItemAmount
        {
            get { return _items.Count; }
        }

        private IEasingFunction elastic = new ElasticEase() { Springiness = 8, Oscillations = 2 };
        private IEasingFunction quadOut = new QuadraticEase() { EasingMode = EasingMode.EaseOut };

        private void SetDisplayerBrushes(Brush value)
        {
            foreach (Grid itemDisplayer in _itemDisplayers)
            {
                itemDisplayer.Background = value;
            }
        }

        public void AddItem(object item)
        {
            Grid itemGrid = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 1),
                Background = _itemDisplayersBrush,
            };

            Border border = new()
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new SolidColorBrush(),
                BorderBrush = new SolidColorBrush(),
                BorderThickness = new Thickness(1, 1, 1, 1)
            };

            ContentControl itemContent = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 5, 5, 5),
            };

            itemGrid.MouseEnter += (s, e) =>
            {
                Animate.Color(border.BorderBrush, SolidColorBrush.ColorProperty, Colors.Gray, TimeSpan.FromMilliseconds(150), quadOut);
                Animate.Color(border.Background, SolidColorBrush.ColorProperty, Color.FromArgb(50, 255, 255, 255), TimeSpan.FromMilliseconds(150), quadOut);
            };

            itemGrid.MouseLeave += (s, e) =>
            {
                Animate.Color(border.BorderBrush, SolidColorBrush.ColorProperty, Color.FromArgb(0, 255, 255, 255), TimeSpan.FromMilliseconds(150), quadOut);
                Animate.Color(border.Background, SolidColorBrush.ColorProperty, Color.FromArgb(0, 255, 255, 255), TimeSpan.FromMilliseconds(150), quadOut);
            };

            if (item is string)
            {
                itemContent.Content = WrapWithOverflowTextBlock(item as string, new SolidColorBrush(Colors.White));
            }
            else
            {
                itemContent.Content = item;
            };

            itemGrid.Children.Add(border);
            itemGrid.Children.Add(itemContent);

            _items.Add(item);
            _itemDisplayers.Add(itemGrid);

            int currentIndex = _itemDisplayers.Count - 1;

            itemGrid.PreviewMouseLeftButtonUp += (s, e) =>
            {
                SelectionChanged?.Invoke(this, new ViperComboBoxSelectionChanged(currentIndex));

                Animate.Color(border.BorderBrush, SolidColorBrush.ColorProperty, Color.FromArgb(0, 255, 255, 255), TimeSpan.FromMilliseconds(150), quadOut, Colors.White);

                ItemDisplayToggle(false);

                ComboBoxContent.Content = item;
            };

            _itemStackPanelList.Children.Add(itemGrid);
        }

        public void RemoveItem(int index)
        {
            _itemStackPanelList.Children.RemoveAt(index);

            _items.RemoveAt(index);
            _itemDisplayers.RemoveAt(index);
        }

        public void SetSelection(int index)
        {
            SelectionChanged?.Invoke(this, new ViperComboBoxSelectionChanged(index));

            ComboBoxContent.Content = _items[index];
        }

        private TextBlock WrapWithOverflowTextBlock(string stringToConvert, Brush brush)
        {
            TextBlock text = new()
            {
                Text = stringToConvert,
                TextWrapping = TextWrapping.WrapWithOverflow,
                Foreground = brush,
            };

            return text;
        }

        public ViperComboBox()
        {
            InitializeComponent();

            RootGrid.RenderTransform = new ScaleTransform();
            SillyThing.RenderTransform = new RotateTransform(132);

            ItemStackPanel.Children.Add(_itemScroll);

            _itemScroll.Content = _itemStackPanelList;

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

            RootGrid.RenderTransform = new ScaleTransform(1, 1) { CenterX = ComboBoxStuffGrid.ActualWidth / 2, CenterY = ComboBoxStuffGrid.ActualHeight / 2 };

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
            RootGrid.RenderTransform = new ScaleTransform(1, 1) { CenterX = ComboBoxStuffGrid.ActualWidth / 2, CenterY = ComboBoxStuffGrid.ActualHeight / 2 };

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

        private async void ItemDisplayToggle(bool newState)
        {
            StateChanged?.Invoke(this, new ViperComboBoxStateChanged(newState));

            if (newState)
            {
                Animate.Double(ItemStackPanel, FrameworkElement.HeightProperty, _itemContMaxHeight, TimeSpan.FromMilliseconds(600), elastic);
                Animate.Double(_itemScroll, ScrollViewer.HeightProperty, _itemContMaxHeight, TimeSpan.FromMilliseconds(600), elastic);
                Animate.Double(SillyThing.RenderTransform, RotateTransform.AngleProperty, 313, TimeSpan.FromMilliseconds(200), quadOut);
                Animate.Color(SillyThing.Stroke, SolidColorBrush.ColorProperty, Color.FromArgb(120, 0, 0, 0), TimeSpan.FromMilliseconds(200), quadOut);
            }
            else
            {
                Animate.Double(ItemStackPanel, FrameworkElement.HeightProperty, 0, TimeSpan.FromMilliseconds(200), quadOut);
                Animate.Double(_itemScroll, ScrollViewer.HeightProperty, 0, TimeSpan.FromMilliseconds(200), elastic);
                Animate.Double(SillyThing.RenderTransform, RotateTransform.AngleProperty, 132, TimeSpan.FromMilliseconds(200), quadOut);
                Animate.Color(SillyThing.Stroke, SolidColorBrush.ColorProperty, Color.FromArgb(40, 255, 255, 255), TimeSpan.FromMilliseconds(200), quadOut);

                await Task.Delay(200);

                _itemScroll.ScrollToTop();
            }

            if (_isOpen != newState)
            {
                _isOpen = newState;
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
