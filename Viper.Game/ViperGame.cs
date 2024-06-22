using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Viper.Game
{
    public class ViperGame : Grid
    {
        private TextBlock _message = new()
        {
            Foreground = new SolidColorBrush(Colors.White),
            FontSize = 16,
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            Text = "Nothing to see here",
        };

        public ViperGame()
        {
            Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));

            Children.Add(_message);
        }
    }
}
