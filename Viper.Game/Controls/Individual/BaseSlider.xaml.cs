using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
    public partial class BaseSlider : UserControl
    {
        public EventHandler<ViperSliderValueChanged>? ValueChanged;

        /// <summary>
        /// Im lazy, so heres the entire control container, do whatrever, use events, etc, idk.
        /// </summary>
        public UserControl Container => SliderControl;

        /// <summary>
        /// Small value used to keep the slider in the middle of the mouse when moving it, without this value, the slider would just be on the side and would look weird.
        /// </summary>
        public const double MOUSE_OFFSET = 3;
        public const double HUNDRED_PERCENT = 100;
        private const byte SET_BACKGROUND_COLOR_A = 89;
        private const byte SET_BACKGROUND_COLOR_R = 0;
        private const byte SET_BACKGROUND_COLOR_G = 0;
        private const byte SET_BACKGROUND_COLOR_B = 0;
        public const byte FOREGROUND_COLOR_R = 255;
        public const byte FOREGROUND_COLOR_G = 255;
        public const byte FOREGROUND_COLOR_B = 255;
        public const byte SLIDER_COLOR_R = 175;
        public const byte SLIDER_COLOR_G = 175;
        public const byte SLIDER_COLOR_B = 175;
        public const byte BAR_COLOR_R = 60;
        public const byte BAR_COLOR_G = 60;
        public const byte BAR_COLOR_B = 60;
        public const byte PROGRESS_COLOR_R = 228;
        public const byte PROGRESS_COLOR_G = 228;
        public const byte PROGRESS_COLOR_B = 228;
        private const bool DEFAULT_IS_ENABLED = true;

        private Brush _background = new SolidColorBrush();
        private Brush _setValueBackground = new SolidColorBrush(Color.FromArgb(SET_BACKGROUND_COLOR_A, SET_BACKGROUND_COLOR_R, SET_BACKGROUND_COLOR_G, SET_BACKGROUND_COLOR_B));
        private Brush _slider = new SolidColorBrush(Color.FromRgb(SLIDER_COLOR_R, SLIDER_COLOR_G, SLIDER_COLOR_B));
        private Brush _bar = new SolidColorBrush(Color.FromRgb(BAR_COLOR_R, BAR_COLOR_G, BAR_COLOR_B));
        private Brush _barProgress = new SolidColorBrush(Color.FromRgb(PROGRESS_COLOR_R, PROGRESS_COLOR_G, PROGRESS_COLOR_B));
        private Brush _foreground = new SolidColorBrush(Color.FromRgb(FOREGROUND_COLOR_R, FOREGROUND_COLOR_G, FOREGROUND_COLOR_B));
        private bool _isEnabled = DEFAULT_IS_ENABLED;
        private bool _canSnap = false;
        private bool _allowSliding = true;
        private bool _allowSetValue = true;
        private bool _setValueIsShowing = false;
        private bool _clickedOnTheControl = false;
        private bool _scrollable = false;
        private double _maxValue = 1;
        private double _minValue = 0;
        private double _currentValue = 0;
        private double _interval = 1;
        private double _currentPos = 0;
        private double _currentPercentage = 0;

        private IEasingFunction elastic = new ElasticEase() { Springiness = 8, Oscillations = 2 };
        private IEasingFunction quadOut = new QuadraticEase() { EasingMode = EasingMode.EaseOut };

        public new Brush Background
        {
            get => _background;

            set
            {
                _background = value;

                Container.Background = value;
            }
        }

        public new Brush ValueSetterBackground
        {
            get => _setValueBackground;

            set
            {
                _setValueBackground = value;

                ValueInputElements.Background = value;
            }
        }

        public new Brush Foreground
        {
            get => _foreground;

            set
            {
                _foreground = value;

                ValueInputTitle.Foreground = value;
                ValueInput.Foreground = value;
            }
        }

        public Brush SliderBrush
        {
            get => _slider;

            set
            {
                _slider = value;

                Button.Fill = value;
            }
        }

        public Brush BarBrush
        {
            get => _bar;

            set
            {
                _bar = value;

                StaticBar.Fill = value;
            }
        }

        public Brush ProgressBarBrush
        {
            get => _barProgress;

            set
            {
                _barProgress = value;

                ProgressBar.Fill = value;
            }
        }

        public double MaxValue
        {
            get => _maxValue;

            set
            {
                _maxValue = value;
            }
        }

        public double MinValue
        {
            get => _minValue;

            set
            {
                _minValue = value;
            }
        }

        public double Value
        {
            get => _currentValue;

            set
            {
                if (value < _minValue)
                {
                    _currentValue = _minValue;
                }
                else if (value > _maxValue)
                {
                    _currentValue = _maxValue;
                }
                else
                {
                    _currentValue = value;
                }
            }
        }

        public double Interval
        {
            get => _interval;

            set
            {
                _interval = value;
            }
        }

        public new bool IsEnabled
        {
            get => _isEnabled;

            set
            {
                _isEnabled = value;
                EnabledToggle(_isEnabled);
            }
        }

        public bool Scrollable
        {
            get => _scrollable;

            set
            {
                _scrollable = value;
            }
        }

        private TranslateTransform _tTransform = new();

        public BaseSlider()
        {
            InitializeComponent();

            RootGrid.RenderTransform = new ScaleTransform();
            RootBorder.BorderBrush = new SolidColorBrush();

            ButtonElements.RenderTransform = _tTransform;

            Container.SizeChanged += Container_SizeChanged;

            // Set mouse enter and leave for animations and events.
            Container.MouseLeave += Container_MouseLeave;
            Container.MouseEnter += Container_MouseEnter;
        }

        private void Container_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Precision of this thing is a bit bad, if i let this modify the actual value of the slider it will create weird floating point fluctuations
            // so for now it will just move the slider but will not update any values.
            ChangeSliderPosition(_currentPercentage * StaticBar.ActualWidth, false, false);
        }

        private void EnabledToggle(bool enable)
        {
            if (enable)
            {
                Animate.Double(Blackout, OpacityProperty, 0, TimeSpan.FromMilliseconds(200));

                if (_allowSliding)
                {
                    _allowSliding = true;
                }

                if (_allowSetValue)
                {
                    _allowSetValue = true;
                }
            }
            else
            {
                Animate.Double(Blackout, OpacityProperty, 0.3, TimeSpan.FromMilliseconds(200));

                _allowSliding = false;
                _allowSetValue = false;

                if (_setValueIsShowing)
                {
                    SetValueOverlayToggle(false, true);
                }
            }
        }

        // Stops the mouse move event when the mouse left button is released.
        private void Container_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Container.MouseMove -= Container_MouseMove;
        }

        private void Container_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    MoveSliderSlightly(true, 1);
                    break;
                case Key.Right:
                    MoveSliderSlightly(false, 1);
                    break;
            }
        }

        // Used for very small value changes.
        private void MoveSliderSlightly(bool toLeft,  double interval)
        {
            if (!toLeft && !(_tTransform.X + 1 > StaticBar.ActualWidth))
            {
                UpdatePosAcordingToValue(_currentValue + _interval);
            }
            else if (toLeft && !(_tTransform.X  - 1 < 0))
            {
                UpdatePosAcordingToValue(_currentValue - _interval);
            }
        } 

        // Opens the "SetValue" overlay of the control, sets events for momentary key presses, and removes everything when the overlay is closed.
        private void ButtonElements_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_allowSetValue)
            {
                _allowSliding = false;

                SetValueOverlayToggle(true);

                ValueInput.PreviewKeyDown += ValueInputPreviewKeyDown;
            }
        }

        private void ValueInputPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ValueInput.PreviewKeyDown -= ValueInputPreviewKeyDown;

                double input = 0;

                try
                {
                    input = Convert.ToDouble(ValueInput.Text);

                    UpdatePosAcordingToValue(input);

                    SetValueOverlayToggle(false);
                }
                catch
                {
                    SetValueOverlayToggle(false, true);
                }

                _allowSliding = true;
            }
        }

        private async void SetValueOverlayToggle(bool show, bool failed = false)
        {
            _setValueIsShowing = show;

            if (show)
            {
                Animate.Double(ValueInputElements, OpacityProperty, 1, TimeSpan.FromMilliseconds(200), quadOut);

                await Task.Delay(200); // Small delay because if this runs instantly, the context menu of the TextBox open at the same time you press right click on the slider.

                ValueInputElements.IsHitTestVisible = true;
            }
            else
            {
                Animate.Double(ValueInputElements, OpacityProperty, 0, TimeSpan.FromMilliseconds(200), quadOut);

                ValueInput.Clear();
                ValueInputElements.IsHitTestVisible = false;
            }
        }

        // Trigers when the mouse is clicked, used for quick slider movements, also sets events to actually being able to slide the slider.
        private void Container_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _clickedOnTheControl = true;

            Container.MouseMove += Container_MouseMove;
            Container.PreviewMouseLeftButtonUp += LocalPreviewMouseLeftButtonUp;

            void LocalPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                Container.MouseMove -= Container_MouseMove;
                Container.PreviewMouseLeftButtonUp -= LocalPreviewMouseLeftButtonUp;
            }

            SliderMovement(e);
        }

        // Triggers only when the mouse is being clicked and moved at the same time.
        private void Container_MouseMove(object sender, MouseEventArgs e)
        {
            if (_clickedOnTheControl)
            {
                SliderMovement(e);
            }
        }

        // Sets the slider position acording to the mouse X position.
        private void SliderMovement(MouseEventArgs e)
        {
            double currentPos = e.GetPosition(Container).X - ButtonElements.ActualWidth + MOUSE_OFFSET;

            ChangeSliderPosition(currentPos);
        }

        // Used to change the actual position of the slider
        private void ChangeSliderPosition(double newPos, bool isFromValueInputSetter = false, bool updateValue = true)
        {
            if (_allowSliding || isFromValueInputSetter)
            {
                if (newPos > 0 && newPos < StaticBar.ActualWidth)
                {
                    SetValue(newPos);
                }
                else if (newPos >= StaticBar.ActualWidth)
                {
                    SetValue(StaticBar.ActualWidth);
                }
                else if (newPos <= 0)
                {
                    SetValue(0);
                }

                void SetValue(double newValue)
                {
                    _tTransform.X = newValue;

                    ProgressBar.Width = newValue;

                    _currentPos = newValue;
                }

                if (updateValue)
                {
                    UpdateValueAcordingToPos();
                }
            }
        }

        private void UpdateValueAcordingToPos()
        {
            // The regla de las tres simples:
            double percentage = _currentPos / StaticBar.ActualWidth;
            double range = _maxValue - _minValue;

            _currentPercentage = percentage;
            _currentValue = Math.Round(_currentPercentage * range, 4);
            _currentValue += _minValue;

            ValueChanged?.Invoke(this, new ViperSliderValueChanged(Math.Round(_currentValue / _interval) * _interval));
        }

        private void UpdatePosAcordingToValue(double value)
        {
            if (value > _maxValue)
            {
                value = _maxValue;
            }
            else if (value < _minValue)
            {
                value = _minValue;
            }

            double range = _maxValue - _minValue;

            double decimalPercentage = (value - _minValue) / range;

            double newPos = (decimalPercentage) * StaticBar.ActualWidth;

            ChangeSliderPosition(newPos, true, true);
        }

        private void Container_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_allowSliding && _scrollable)
            {
                switch (e.Delta)
                {
                    case < 0:
                        MoveSliderSlightly(true, 1);
                        break;
                    case > 0:
                        MoveSliderSlightly(false, 1);
                        break;
                }
            }
        }

        // Removes events (just in case) and animates stuff when the mouse exits the control.
        private void Container_MouseLeave(object sender, MouseEventArgs e)
        {
            _clickedOnTheControl = false;

            RootGrid.IsHitTestVisible = false;
            Container.PreviewKeyDown -= Container_PreviewKeyDown;
            Container.PreviewMouseRightButtonDown -= ButtonElements_PreviewMouseRightButtonDown;
            Container.PreviewMouseLeftButtonDown -= Container_MouseLeftButtonDown;
            Container.PreviewMouseLeftButtonUp -= Container_PreviewMouseLeftButtonUp;
            Container.MouseWheel -= Container_MouseWheel;
        }

        // Adds events and animates stuff when the mouse enters the control.
        private void Container_MouseEnter(object sender, MouseEventArgs e)
        {
            RootGrid.IsHitTestVisible = true;
            Container.PreviewKeyDown += Container_PreviewKeyDown;
            Container.PreviewMouseRightButtonDown += ButtonElements_PreviewMouseRightButtonDown;
            Container.PreviewMouseLeftButtonDown += Container_MouseLeftButtonDown;
            Container.PreviewMouseLeftButtonUp += Container_PreviewMouseLeftButtonUp;
            Container.MouseWheel += Container_MouseWheel;
        }
    }
}
