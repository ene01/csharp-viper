using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Viper.Game.Animations;

namespace Viper.Game.Controls.Individual.Custom
{
    public class ClassicComboBox : BaseComboBox
    {
        public ClassicComboBox()
        {
            ElasticEase elastic = new() { Springiness = 5, Oscillations = 5 };

            Background = new SolidColorBrush(Color.FromRgb(23, 23, 23));
            Foreground = new SolidColorBrush(Colors.White);
            BorderBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            ItemsDisplayersBackground = new SolidColorBrush(Color.FromRgb(10, 10, 10));
            ItemsDisplayersBorder = new SolidColorBrush(Color.FromRgb(40, 40, 40));
            ArrowColor = Color.FromRgb(160, 160, 160);
            ItemListBackground = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            DropDownEasing = new QuadraticEase();
            DropDownTimeSpan = TimeSpan.FromMilliseconds(200);

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
                if (!IsOpen)
                {
                    Animate.Color(BorderBrush, SolidColorBrush.ColorProperty, Color.FromRgb(120, 120, 120), TimeSpan.FromMilliseconds(50), new QuadraticEase());
                }
            };

            Release += (s, e) =>
            {
                if (!IsOpen)
                {
                    Animate.Color(BorderBrush, SolidColorBrush.ColorProperty, Color.FromRgb(80, 80, 80), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                }
            };

            StateChanged += (s, e) =>
            {
                if (e.IsOpen)
                {
                    Animate.Color(BorderBrush, SolidColorBrush.ColorProperty, Color.FromRgb(255, 255, 255), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                }
                else
                {
                    Animate.Color(BorderBrush, SolidColorBrush.ColorProperty, Color.FromRgb(80, 80, 80), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                }
            };

            HoveringItem += (s, e) =>
            {
                Animate.Color(ItemDisplayers[e.Index].Background, SolidColorBrush.ColorProperty, Color.FromRgb(80, 80, 80), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                Animate.Color(ItemDisplayerBorders[e.Index].BorderBrush, SolidColorBrush.ColorProperty, Color.FromRgb(255, 255, 255), TimeSpan.FromMilliseconds(200), new QuadraticEase());

                ItemDisplayers[e.Index].MouseLeave += ClassicComboBox_MouseLeave;

                void ClassicComboBox_MouseLeave(object sender, System.Windows.Input.MouseEventArgs mEvents)
                {
                    Animate.Color(ItemDisplayers[e.Index].Background, SolidColorBrush.ColorProperty, Color.FromRgb(10, 10, 10), TimeSpan.FromMilliseconds(200), new QuadraticEase());
                    Animate.Color(ItemDisplayerBorders[e.Index].BorderBrush, SolidColorBrush.ColorProperty, Color.FromRgb(10, 10, 10), TimeSpan.FromMilliseconds(200), new QuadraticEase());

                    ItemDisplayerBorders[e.Index].MouseLeave -= ClassicComboBox_MouseLeave;
                }
            };
        }
    }
}
