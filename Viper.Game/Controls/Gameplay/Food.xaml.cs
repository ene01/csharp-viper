using System;
using System.Collections.Generic;
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

namespace Viper.Game.Controls.Gameplay
{
    /// <summary>
    /// A square that moves arround when desired.
    /// </summary>
    public partial class Food : UserControl
    {
        public EventHandler<PositionChangedEventArgs>? PositionChanged;

        public EventHandler<StrokeChangedEventArgs>? StrokeChanged;

        public EventHandler<BrushChangedEventArgs>? BrushChanged;

        private TransformGroup _transforms = new();

        private Rectangle _foodElement = new();

        private Brush _foodBrush = new SolidColorBrush(Colors.White);
        public Brush FoodBrush
        {
            get { return _foodBrush; }

            set { _foodBrush = value; BrushChanged?.Invoke(this, new BrushChangedEventArgs(value)); }
        }

        private Brush _foodStroke = new SolidColorBrush(Colors.White);
        public Brush FoodStroke
        {
            get { return _foodStroke; }

            set { _foodStroke = value; StrokeChanged?.Invoke(this, new StrokeChangedEventArgs(value)); }
        }

        private double _xLimit = 0;
        public double XLimit
        {
            get { return _xLimit; }

            set
            {
                if (value > 0)
                {
                    // Round value to prevent weird decimal limits
                    double roundedValue = Math.Round(value, 0);

                    // Value can only be set if divisible by the element size
                    while (!(roundedValue % FOOD_DEFAULT_SIZE == 0))
                    {
                        roundedValue -= 1;
                    }

                    _xLimit = roundedValue;
                }
            }
        }

        private double _yLimit = 0;
        public double YLimit
        {
            get { return _yLimit; }

            set
            {
                if (value > 0)
                {
                    // Round value to prevent weird decimal limits
                    double roundedValue = Math.Round(value, 0);

                    // Value can only be set if divisible by the element size
                    while (!(roundedValue % FOOD_DEFAULT_SIZE == 0))
                    {
                        roundedValue -= 1;
                    }

                    _yLimit = roundedValue;
                }
            }
        }

        private double _xPos = 0;
        public double PositionX
        {
            get { return _xPos; }
        }

        private double _yPos = 0;
        public double PositionY
        {
            get { return _yPos; }
        }

        public const int FOOD_DEFAULT_SIZE = 10;

        private ScaleTransform _scale = new(1, 1)
        {
            CenterX = FOOD_DEFAULT_SIZE / 2,
            CenterY = FOOD_DEFAULT_SIZE / 2,
        };

        private TranslateTransform _translate = new(0, 0);

        private ElasticEase elastic = new() { Oscillations = 5, Springiness = 5 };

        public Food()
        {
            InitializeComponent();

            this.Loaded += Food_Loaded;

            FoodGrid.Children.Add(_foodElement);
        }

        private void Food_Loaded(object sender, RoutedEventArgs e)
        {
            _foodElement.Height = FOOD_DEFAULT_SIZE;
            _foodElement.Width = FOOD_DEFAULT_SIZE;

            _transforms.Children.Add(_scale);
            _transforms.Children.Add(_translate);

            _foodElement.RenderTransform = _transforms;

            _foodElement.Fill = _foodBrush;
            _foodElement.Stroke = _foodStroke;

            Animate.Double(_foodElement, Shape.OpacityProperty, 1, TimeSpan.FromMilliseconds(200), new SineEase(), 0);
            Animate.Double(_scale, ScaleTransform.ScaleYProperty, 1, TimeSpan.FromMilliseconds(900), elastic, 0.6);
            Animate.Double(_scale, ScaleTransform.ScaleXProperty, 1, TimeSpan.FromMilliseconds(900), elastic, 0.6);

            RandomizePosition();
        }

        public void RandomizePosition()
        {
            Random rnd = new();

            // Not sure it this formula works for a value other than 10 but idk xd.
            double newX = rnd.Next(0, (int)_xLimit / FOOD_DEFAULT_SIZE) * FOOD_DEFAULT_SIZE;
            double newY = rnd.Next(0, (int)_yLimit / FOOD_DEFAULT_SIZE) * FOOD_DEFAULT_SIZE;

            _translate.X = newX;
            _translate.Y = newY;

            PositionChanged?.Invoke(this, new PositionChangedEventArgs(newX, newY));
        }
    }
}
