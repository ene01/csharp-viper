using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    public class ViperComboBoxSelectionChanged : EventArgs
    {
        public int Index;

        public ViperComboBoxSelectionChanged(int @index)
        {
            Index = @index;
        }
    }
}
