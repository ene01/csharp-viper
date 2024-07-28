using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Viper.Game.Animations;

namespace Viper.Game.Controls.Individual.Custom
{
    public class ClassicUnlimitedSelector : BaseUnlimitedSelector
    {
        public ClassicUnlimitedSelector()
        {
            GradientStop gs1 = new()
            {
                Color = Color.FromArgb(0, 255, 255, 255),
                Offset = 0,
            };

            GradientStop gs2 = new()
            {
                Color = Color.FromArgb(0, 255, 255, 255),
                Offset = 1,
            };

            LinearGradientBrush lg = new() { GradientStops = { gs1, gs2 }, StartPoint = new Point(0, 0), EndPoint = new Point(1, 0) };

            Background = new SolidColorBrush(Color.FromRgb(23, 23, 23));
            Foreground = new SolidColorBrush(Colors.White);
            BorderBrush = lg;

            LeftClick += (s, e) =>
            {
                Animate.Color(gs1, GradientStop.ColorProperty, Color.FromArgb(0, 255, 255, 255), TimeSpan.FromMilliseconds(400), new QuadraticEase(), Color.FromArgb(255, 255, 255, 255));
            };

            RightClick += (s, e) =>
            {
                Animate.Color(gs2, GradientStop.ColorProperty, Color.FromArgb(0, 255, 255, 255), TimeSpan.FromMilliseconds(400), new QuadraticEase(), Color.FromArgb(255, 255, 255, 255));
            };

            NoPossibleMoves += (s, e) =>
            {
                Animate.Color(gs1, GradientStop.ColorProperty, Color.FromArgb(0, 255, 255, 255), TimeSpan.FromMilliseconds(400), new QuadraticEase(), Color.FromRgb(255, 79, 66));
                Animate.Color(gs2, GradientStop.ColorProperty, Color.FromArgb(0, 255, 255, 255), TimeSpan.FromMilliseconds(400), new QuadraticEase(), Color.FromRgb(255, 79, 66));
            };
        }
    }
}
