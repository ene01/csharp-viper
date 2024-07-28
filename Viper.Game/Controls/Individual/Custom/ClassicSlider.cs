using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Viper.Game.Animations;

namespace Viper.Game.Controls.Individual.Custom
{
    public class ClassicSlider : BaseSlider
    {
        public ClassicSlider()
        {
            StaticBar.StrokeThickness = 0;
            StaticBar.Stroke = new SolidColorBrush(Colors.Gray);

            MouseEnter += ClassicSlider_MouseEnter;
            MouseLeave += ClassicSlider_MouseLeave;

            void ClassicSlider_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            {
                Animate.Double(StaticBar, Shape.StrokeThicknessProperty, 1, TimeSpan.FromMilliseconds(100), new QuadraticEase());
            }

            void ClassicSlider_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
            {
                Animate.Double(StaticBar, Shape.StrokeThicknessProperty, 0, TimeSpan.FromMilliseconds(300), new QuadraticEase());
            }
        }
    }
}
