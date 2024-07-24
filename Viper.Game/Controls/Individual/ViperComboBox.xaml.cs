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

namespace Viper.Game.Controls.Individual
{
    /// <summary>
    /// Lógica de interacción para ViperButton.xaml
    /// </summary>
    public partial class ViperComboBox : UserControl
    {
        /// <summary>
        /// Im lazy, so heres the entire control container, do whatrever, use events, etc, idk.
        /// </summary>
        public UserControl Container => ComboBoxControl;

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

        public EventHandler<ViperComboBoxStateChanged>? StateChanged;

        public EventHandler<ViperComboBoxSelectionChanged>? SelectionChanged;

        private const string DEFAULT_CONTENT_VIPER_CHECK_BOX = "Nothing selected";
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
        private const bool DEFAULT_USE_PREVIOUS_ITEM = false;
        private const bool DEFAULT_INSTA_SELECT = false;

        private object _dContent = DEFAULT_CONTENT_VIPER_CHECK_BOX;
        private Brush _background = new SolidColorBrush(Color.FromRgb(BACKGROUND_COLOR_R, BACKGROUND_COLOR_G, BACKGROUND_COLOR_B));
        private Brush _border = new SolidColorBrush(Color.FromRgb(BORDER_COLOR_R, BORDER_COLOR_G, BORDER_COLOR_B));
        private Brush _foreground = new SolidColorBrush(Color.FromRgb(FOREGROUND_COLOR_R, FOREGROUND_COLOR_G, FOREGROUND_COLOR_B));
        private Brush _itemsBackground = new SolidColorBrush(Color.FromRgb(ITEMS_BACKGROUND_COLOR_R, ITEMS_BACKGROUND_COLOR_G, ITEMS_BACKGROUND_COLOR_B));
        private double _stuffGridHeight = DEFAULT_GRID_HEIGHT;
        private double _stuffGridWidth = DEFAULT_GRID_WIDTH;
        private double _itemContMaxHeight = ITEM_CONT_MAX_HEIGHT;
        private Thickness _spacing = new Thickness(THICKNESS_LEFT, THICKNESS_TOP, THICKNESS_RIGHT, THICKNESS_BOTTOM);
        private VerticalAlignment _yAlignment = DEFAULT_VERTICAL_ALIGNMENT;
        private HorizontalAlignment _xAlignment = DEFAULT_HORIZONTAL_ALIGNMENT;
        private bool _isEnabled = DEFAULT_IS_ENABLED;
        private bool _isOpen = DEFAULT_IS_OPEN;
        private bool _usePrevious = DEFAULT_USE_PREVIOUS_ITEM;
        private bool _instaSelect = DEFAULT_INSTA_SELECT;
        private Brush _itemDisplayersBrush = new SolidColorBrush(Color.FromRgb(ITEMS_DISPLAYERS_COLOR_R, ITEMS_DISPLAYERS_COLOR_G, ITEMS_DISPLAYERS_COLOR_B));
        private int? _selected = null;

        public new object? Content = null; // :tf:

        /// <summary>
        /// Content showed when theres no item selected
        /// </summary>
        public object FallbackContent
        {
            get { return _dContent; }

            set
            {
                if (value is string)
                {
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                    _dContent = WrapWithOverflowTextBlock(value as string, _foreground);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
                }
                else
                {
                    _dContent = value;
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

        public new Brush BorderBrush
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

        public Brush ItemsBackground
        {
            get { return _itemsBackground; }

            set
            {
                _itemsBackground = value;

                ItemContainer.Background = value;
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

        public double ItemContainerMaxHeight
        {
            get { return _itemContMaxHeight; }

            set
            {
                _itemContMaxHeight = value;
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

        public bool IsOpen
        {
            get { return _isOpen; }

            set
            {
                _isOpen = value;

                ItemDisplayToggle(value);
            }
        }

        /// <summary>
        /// Handles cases when the current item selected is removed by changing the selection to the previous item.
        /// </summary>
        public bool UsePreviousItem
        {
            get { return _usePrevious; }

            set
            {
                _usePrevious = value;
            }
        }

        /// <summary>
        /// Selects the first item as soon as theres one availible.
        /// </summary>
        public bool InstaSelection
        {
            get { return _instaSelect; }

            set
            {
                _instaSelect = value;
            }
        }

        public int? SelectedIndex
        {
            get => _selected;
        }

        public int ItemAmount
        {
            get { return _items.Count; }
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

        private IEasingFunction elastic = new ElasticEase() { Springiness = 8, Oscillations = 2 };
        private IEasingFunction quadOut = new QuadraticEase() { EasingMode = EasingMode.EaseOut };

        private void SetDisplayerBrushes(Brush value)
        {
            foreach (Grid itemDisplayer in _itemDisplayers)
            {
                itemDisplayer.Background = value;
            }
        }

        public object GetItemFromIndex(int index)
        {
            return _items[index];
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
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                itemContent.Content = WrapWithOverflowTextBlock(item as string, new SolidColorBrush(Colors.White));
#pragma warning restore CS8604 // Posible argumento de referencia nulo
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

                _selected = currentIndex;

                Animate.Color(border.BorderBrush, SolidColorBrush.ColorProperty, Color.FromArgb(0, 255, 255, 255), TimeSpan.FromMilliseconds(150), quadOut, Colors.White);

                ItemDisplayToggle(false);

                ComboBoxContent.Content = item;
            };

            _itemStackPanelList.Children.Add(itemGrid);

            if (_selected == null && _items.Count == 1 && _instaSelect)
            {
                SetSelection(0);
            }
        }

        public void RemoveItem(int index)
        {
            if (!(index < 0) && index < _items.Count)
            {
                if (index == 0)
                {
                    CheckSelectedContent(true);
                }

                if (_usePrevious && index == _selected)
                {
                    SetSelection(index - 1);
                }
                else if (index == _selected)
                {
                    CheckSelectedContent(true);
                }

                _itemStackPanelList.Children.RemoveAt(index);
                _items.RemoveAt(index);
                _itemDisplayers.RemoveAt(index);
            }
        }

        public void ClearAllItems()
        {
            _itemStackPanelList.Children.Clear();
            _items.Clear();
            _itemDisplayers.Clear();
            CheckSelectedContent(true);
        }

        public void SetSelection(int index)
        {
            if (index < _items.Count && !(index < 0))
            {
                SelectionChanged?.Invoke(this, new ViperComboBoxSelectionChanged(index));

                _selected = index;
                Debug.WriteLine($"-- Selection updated = {_selected}");

                ComboBoxContent.Content = _items[index];
            }
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

            ItemContainer.Children.Add(_itemScroll);

            _itemScroll.Content = _itemStackPanelList;

            Loaded += ViperCheckBox_Loaded;
            Unloaded += ViperCheckBox_Unloaded;
            ComboBoxControl .LostFocus += ViperCheckBox_LostFocus;

            ComboBoxStuffGrid.PreviewMouseLeftButtonDown += ComboBoxContainer_PreviewMouseLeftButtonDown;
            ComboBoxStuffGrid.PreviewMouseLeftButtonUp += CheckBoxContainer_PreviewMouseLeftButtonUp;
            ComboBoxControl .MouseEnter += ComboBoxContainer_MouseEnter;
            ComboBoxControl.MouseLeave += ComboBoxContainer_MouseLeave;
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

            ComboBoxControl.MouseLeave += LocalMouseLeave;

            Animate.Double(RootGrid.RenderTransform, ScaleTransform.ScaleXProperty, 0.9, TimeSpan.FromMilliseconds(1000), quadOut);
            Animate.Double(RootGrid.RenderTransform, ScaleTransform.ScaleYProperty, 0.9, TimeSpan.FromMilliseconds(1000), quadOut);

            void LocalMouseLeave(object sender, MouseEventArgs e)
            {
                ComboBoxControl.MouseLeave -= LocalMouseLeave;

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
            ComboBoxControl.LostFocus -= ViperCheckBox_LostFocus;
            ComboBoxControl.PreviewMouseLeftButtonDown -= ComboBoxContainer_PreviewMouseLeftButtonDown;
            ComboBoxControl.PreviewMouseLeftButtonUp -= CheckBoxContainer_PreviewMouseLeftButtonUp;
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

            CheckSelectedContent();

            EnabledLayerToggle(_isEnabled);
        }

        private void CheckSelectedContent(bool forceClear = false)
        {
            if (forceClear || _selected == null)
            {
                if (forceClear)
                {
                    _selected = null;
                }

                ComboBoxContent.Content = _dContent;
            };
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
            else
            {
                Animate.Color(BorderOverlay.BorderBrush, SolidColorBrush.ColorProperty, Color.FromArgb(0, 255, 51, 51), TimeSpan.FromMilliseconds(400), quadOut, Color.FromArgb(255, 255, 51, 51));
            }

            Release?.Invoke(this, new EventArgs());
        }

        private async void ItemDisplayToggle(bool newState)
        {
            StateChanged?.Invoke(this, new ViperComboBoxStateChanged(newState));

            if (newState)
            {
                Animate.Double(ItemContainer, FrameworkElement.HeightProperty, _itemContMaxHeight, TimeSpan.FromMilliseconds(600), elastic);
                Animate.Double(_itemScroll, ScrollViewer.HeightProperty, _itemContMaxHeight, TimeSpan.FromMilliseconds(600), elastic);
                Animate.Double(SillyThing.RenderTransform, RotateTransform.AngleProperty, 313, TimeSpan.FromMilliseconds(200), quadOut);
                Animate.Color(SillyThing.Stroke, SolidColorBrush.ColorProperty, Color.FromArgb(120, 0, 0, 0), TimeSpan.FromMilliseconds(200), quadOut);
            }
            else
            {
                Animate.Double(ItemContainer, FrameworkElement.HeightProperty, 0, TimeSpan.FromMilliseconds(200), quadOut);
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

#pragma warning disable CS8622 // La nulabilidad de los tipos de referencia del tipo de parámetro no coincide con el delegado de destino (posiblemente debido a los atributos de nulabilidad).
            viperComboBox.StateChanged+= OnStateChanged;
            viperComboBox.Hovering += OnHover;
            viperComboBox.NoHovering += OnNoHover;
#pragma warning restore CS8622 // La nulabilidad de los tipos de referencia del tipo de parámetro no coincide con el delegado de destino (posiblemente debido a los atributos de nulabilidad).

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
