using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    public class ViperSliderValueChanged : EventArgs
    {
        public double Value;

        public ViperSliderValueChanged(double @value)
        {
            Value = @value;
        }
    }
}
