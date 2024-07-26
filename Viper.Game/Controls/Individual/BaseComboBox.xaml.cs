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
    public partial class BaseComboBox : UserControl
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

        /// <summary>
        /// Event that triggers when the mouse is being hovered over the combo box.
        /// </summary>
        public EventHandler? Hovering;

        /// <summary>
        /// Event that triggers when the mouse is no longer hovering the combo box.
        /// </summary>
        public EventHandler? NoHovering;

        public EventHandler<ViperComboBoxStateChanged>? StateChanged;

        public EventHandler<ViperComboBoxSelectionChanged>? SelectionChanged;

        public EventHandler<ViperComboBoxHoveringItemChanged>? HoveringItem;

        private const string DEFAULT_CONTENT_VIPER_CHECK_BOX = "Nothing selected";
        private const byte BACKGROUND_COLOR_R = 193;
        private const byte BACKGROUND_COLOR_G = 193;
        private const byte BACKGROUND_COLOR_B = 193;
        private const byte LIST_BACKGROUND_COLOR_R = 193;
        private const byte LIST_BACKGROUND_COLOR_G = 193;
        private const byte LIST_BACKGROUND_COLOR_B = 193;
        private const byte BORDER_COLOR_R = 80;
        private const byte BORDER_COLOR_G = 80;
        private const byte BORDER_COLOR_B = 80;
        private const byte FOREGROUND_COLOR_R = 0;
        private const byte FOREGROUND_COLOR_G = 0;
        private const byte FOREGROUND_COLOR_B = 0;
        private const byte ITEMS_DISPLAYERS_COLOR_R = 240;
        private const byte ITEMS_DISPLAYERS_COLOR_G = 240;
        private const byte ITEMS_DISPLAYERS_COLOR_B = 240;
        private const byte ARROW_COLOR_R = 64;
        private const byte ARROW_COLOR_G = 64;
        private const byte ARROW_COLOR_B = 64;
        private const double DEFAULT_GRID_HEIGHT = double.NaN;
        private const double DEFAULT_GRID_WIDTH = double.NaN;
        private const double ITEM_CONT_MAX_HEIGHT = 100;
        private const bool DEFAULT_IS_OPEN = false;
        private const bool DEFAULT_USE_PREVIOUS_ITEM = false;
        private const bool DEFAULT_INSTA_SELECT = false;

        private object _dContent = DEFAULT_CONTENT_VIPER_CHECK_BOX;
        private Brush _background = new SolidColorBrush(Color.FromRgb(BACKGROUND_COLOR_R, BACKGROUND_COLOR_G, BACKGROUND_COLOR_B));
        private Brush _border = new SolidColorBrush(Color.FromRgb(BORDER_COLOR_R, BORDER_COLOR_G, BORDER_COLOR_B));
        private Brush _foreground = new SolidColorBrush(Color.FromRgb(FOREGROUND_COLOR_R, FOREGROUND_COLOR_G, FOREGROUND_COLOR_B));
        private Brush _itemDisplayersBrush = new SolidColorBrush(Color.FromRgb(ITEMS_DISPLAYERS_COLOR_R, ITEMS_DISPLAYERS_COLOR_G, ITEMS_DISPLAYERS_COLOR_B));
        private Brush _itemListBrush = new SolidColorBrush(Color.FromRgb(LIST_BACKGROUND_COLOR_R, LIST_BACKGROUND_COLOR_G, LIST_BACKGROUND_COLOR_B));
        private Color _arrowColor = Color.FromRgb(ARROW_COLOR_R, ARROW_COLOR_G, ARROW_COLOR_B);
        private double _stuffGridHeight = DEFAULT_GRID_HEIGHT;
        private double _stuffGridWidth = DEFAULT_GRID_WIDTH;
        private double _itemContMaxHeight = ITEM_CONT_MAX_HEIGHT;
        private bool _isOpen = DEFAULT_IS_OPEN;
        private bool _usePrevious = DEFAULT_USE_PREVIOUS_ITEM;
        private bool _instaSelect = DEFAULT_INSTA_SELECT;
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

        public Color ArrowColor
        {
            get { return _arrowColor; }

            set
            {
                _arrowColor = value;

                (Arrow.Fill as LinearGradientBrush).GradientStops[1].Color = value;
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

        public Brush ItemListBackground
        {
            get
            {
                return _itemListBrush;
            }

            set
            {
                _itemListBrush = value;
                ItemContainer.Background = _itemListBrush;
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

        public IEasingFunction DropDownEasing { get; set; }

        public TimeSpan DropDownTimeSpan { get; set; } = TimeSpan.FromMilliseconds(200);

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
        /// <summary>
        /// All items added to the ComboBox
        /// </summary>
        public List<object> Items { get { return _items; } }


        private List<Grid> _itemDisplayers = new();
        /// <summary>
        /// Grids that encapsulate the objects added.
        /// </summary>
        public List<Grid> ItemDisplayers { get { return _itemDisplayers; } }


        private List<Border> _itemDisplayerBorders = new();
        /// <summary>
        /// Borders that are usually transparent, these are on each ItemDisplayer, use these for background changes or other things when hovering or whatrver.
        /// </summary>
        public List<Border> ItemDisplayerBorders { get { return _itemDisplayerBorders; } }

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
            int currentIndex = 0;

            Grid itemGrid = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 1),
            };

            Border border = new()
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = _itemDisplayersBrush,
                BorderBrush = new SolidColorBrush(),
                BorderThickness = new Thickness(1, 1, 1, 1)
            };

            ContentControl itemContent = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 5, 5, 5),
                Foreground = _foreground,
            };

            itemGrid.MouseEnter += (s, e) =>
            {
                itemContent.FontWeight = FontWeights.Bold;
                HoveringItem?.Invoke(this, new ViperComboBoxHoveringItemChanged(currentIndex));
            };

            itemGrid.MouseLeave += (s, e) =>
            {
                itemContent.FontWeight = FontWeights.Normal;
            };

            if (item is string)
            {
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                itemContent.Content = WrapWithOverflowTextBlock(item as string, _foreground);
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
            _itemDisplayerBorders.Add(border);

            currentIndex = _itemDisplayers.Count - 1;

            itemGrid.PreviewMouseLeftButtonUp += (s, e) =>
            {
                SelectionChanged?.Invoke(this, new ViperComboBoxSelectionChanged(currentIndex));

                _selected = currentIndex;

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

        public BaseComboBox()
        {
            InitializeComponent();

            RootGrid.RenderTransform = new ScaleTransform();
            Arrow.RenderTransform = new RotateTransform();

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

            void LocalMouseLeave(object sender, MouseEventArgs e)
            {
                ComboBoxControl.MouseLeave -= LocalMouseLeave;

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

            EnabledLayerToggle(IsEnabled);
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
            if (IsEnabled)
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
                Animate.Double(ItemContainer, FrameworkElement.HeightProperty, _itemContMaxHeight, DropDownTimeSpan, DropDownEasing);
                Animate.Double(_itemScroll, ScrollViewer.HeightProperty, _itemContMaxHeight, DropDownTimeSpan, DropDownEasing);
                Animate.Double(Arrow.RenderTransform, RotateTransform.AngleProperty, 180, DropDownTimeSpan, DropDownEasing);
            }
            else
            {
                Animate.Double(ItemContainer, FrameworkElement.HeightProperty, 0, DropDownTimeSpan, quadOut);
                Animate.Double(_itemScroll, ScrollViewer.HeightProperty, 0, DropDownTimeSpan, elastic);
                Animate.Double(Arrow.RenderTransform, RotateTransform.AngleProperty, 0, DropDownTimeSpan, DropDownEasing);

                await Task.Delay(200);

                _itemScroll.ScrollToTop();
            }

            if (_isOpen != newState)
            {
                _isOpen = newState;
            }
        }
    }
}
