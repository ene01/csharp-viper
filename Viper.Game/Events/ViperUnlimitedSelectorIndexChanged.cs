using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    public class ViperUnlimitedSelectorIndexChanged : EventArgs
    {
        public int Index;

        public ViperUnlimitedSelectorIndexChanged(int @index)
        {
            Index = @index;
        }
    }
}
