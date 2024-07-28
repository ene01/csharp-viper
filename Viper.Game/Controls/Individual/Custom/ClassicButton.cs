using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Viper.Game.Animations;

namespace Viper.Game.Controls.Individual.Custom
{
    /// <summary>
    /// A BaseButton with my personal preferences
    /// </summary>
    public class ClassicButton : BaseButton
    {
        public ClassicButton()
        {
            ElasticEase elastic = new() { Springiness = 5, Oscillations = 5 };

            Background = new SolidColorBrush(Color.FromRgb(23, 23, 23));
            BorderBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            Foreground = new SolidColorBrush(Colors.White);
            ButtonGrid.RenderTransform = new ScaleTransform();

            Hovering += (s, e) =>
            {
                Animate.Color(Background, SolidColorBrush.ColorProperty, Color.FromRgb(40, 40, 40), TimeSpan.FromMilliseconds(200), new QuadraticEase());
            };

            NoHovering += (s, e) =>
            {
                Animate.Color(Background, SolidColorBrush.ColorProperty, Color.FromRgb(23, 23, 23), TimeSpan.FromMilliseconds(200), new QuadraticEase());
            };

            Holding += (s, e) =>
            {
                Animate.Color(BorderBrush, SolidColorBrush.ColorProperty, Color.FromRgb(255, 255, 255), TimeSpan.FromMilliseconds(50), new QuadraticEase());
                Animate.Double(ButtonGrid.RenderTransform, ScaleTransform.ScaleXProperty, 0.9, TimeSpan.FromMilliseconds(600), new QuadraticEase());
                Animate.Double(ButtonGrid.RenderTransform, ScaleTransform.ScaleYProperty, 0.9, TimeSpan.FromMilliseconds(600), new QuadraticEase());
            };

            Release += (s, e) =>
            {
                Animate.Color(BorderBrush, SolidColorBrush.ColorProperty, Color.FromRgb(80, 80, 80), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                Animate.Double(ButtonGrid.RenderTransform, ScaleTransform.ScaleXProperty, 1, TimeSpan.FromMilliseconds(1400), elastic);
                Animate.Double(ButtonGrid.RenderTransform, ScaleTransform.ScaleYProperty, 1, TimeSpan.FromMilliseconds(1400), elastic);
            };

            ButtonGrid.Loaded += (s, e) =>
            {
                RenderTransformOrigin = new System.Windows.Point(ButtonGrid.ActualWidth / 2, ButtonGrid.ActualHeight / 2);
            };

            ButtonGrid.SizeChanged += (s, e) =>
            {
                RenderTransformOrigin = new System.Windows.Point(ButtonGrid.ActualWidth / 2, ButtonGrid.ActualHeight / 2);
            };
        }
    }
}
