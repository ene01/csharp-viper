using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    public class ViperComboBoxStateChanged : EventArgs
    {
        public bool IsOpen;

        public ViperComboBoxStateChanged(bool @state)
        {
            IsOpen = @state;
        }
    }
}
