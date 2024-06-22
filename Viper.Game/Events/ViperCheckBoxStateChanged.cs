using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    public class ViperCheckBoxStateChanged : EventArgs
    {
        public bool State;

        public ViperCheckBoxStateChanged(bool @checked)
        {
            State = @checked;
        }
    }
}
