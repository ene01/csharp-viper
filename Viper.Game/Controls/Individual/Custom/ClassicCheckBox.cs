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
    public class ClassicCheckBox : BaseCheckBox
    {
        public ClassicCheckBox()
        {
            ElasticEase elastic = new() { Springiness = 5, Oscillations = 5 };

            Background = new SolidColorBrush();
            BorderBrush = new SolidColorBrush();
            Foreground = new SolidColorBrush(Colors.White);
            CheckFill = new SolidColorBrush(Color.FromRgb(23, 23, 23));
            CheckStroke = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            Check.RenderTransform = new ScaleTransform();

            Check.RadiusX = 2;
            Check.RadiusY = 2;

            Hovering += (s, e) =>
            {
                Animate.Color(CheckStroke, SolidColorBrush.ColorProperty, Color.FromRgb(100, 100, 100), TimeSpan.FromMilliseconds(200), new QuadraticEase());
            };

            NoHovering += (s, e) =>
            {
                Animate.Color(CheckStroke, SolidColorBrush.ColorProperty, Color.FromRgb(80, 80, 80), TimeSpan.FromMilliseconds(200), new QuadraticEase());
            };

            Holding += (s, e) =>
            {
                Animate.Color(CheckStroke, SolidColorBrush.ColorProperty, Color.FromRgb(255, 255, 255), TimeSpan.FromMilliseconds(50), new QuadraticEase());
                Animate.Double(Check.RenderTransform, ScaleTransform.ScaleXProperty, 0.7, TimeSpan.FromMilliseconds(600), new QuadraticEase());
                Animate.Double(Check.RenderTransform, ScaleTransform.ScaleYProperty, 0.7, TimeSpan.FromMilliseconds(600), new QuadraticEase());
            };

            Release += (s, e) =>
            {
                Animate.Color(CheckStroke, SolidColorBrush.ColorProperty, Color.FromRgb(80, 80, 80), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                Animate.Double(Check.RenderTransform, ScaleTransform.ScaleXProperty, 1, TimeSpan.FromMilliseconds(1400), elastic);
                Animate.Double(Check.RenderTransform, ScaleTransform.ScaleYProperty, 1, TimeSpan.FromMilliseconds(1400), elastic);
            };

            StateChanged += (s, e) =>
            {
                if (e.State)
                {
                    Animate.Color(CheckFill, SolidColorBrush.ColorProperty, Color.FromRgb(205, 54, 255), TimeSpan.FromMilliseconds(300), new QuarticEase());
                    Animate.Color(CheckStroke, SolidColorBrush.ColorProperty, Color.FromRgb(255, 255, 255), TimeSpan.FromMilliseconds(300), new QuarticEase());
                }
                else
                {
                    Animate.Color(CheckFill, SolidColorBrush.ColorProperty, Color.FromRgb(23, 23, 23), TimeSpan.FromMilliseconds(300), new QuarticEase());
                    Animate.Color(CheckStroke, SolidColorBrush.ColorProperty, Color.FromRgb(80, 80, 80), TimeSpan.FromMilliseconds(300), new QuarticEase());
                }
            };
        }
    }
}
