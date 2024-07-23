using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viper.Game.Interfaces;

namespace Viper.Game.Events
{
    public class ViperUnlimitedSelectorIndexChanged : EventArgs, IViperControlEvents
    {
        public int Index;

        public ViperUnlimitedSelectorIndexChanged(int @index)
        {
            Index = @index;
        }
    }

    public class ViperSliderValueChanged : EventArgs, IViperControlEvents
    {
        public double Value;

        public ViperSliderValueChanged(double @value)
        {
            Value = @value;
        }
    }

    public class ViperComboBoxStateChanged : EventArgs, IViperControlEvents
    {
        public bool IsOpen;

        public ViperComboBoxStateChanged(bool @state)
        {
            IsOpen = @state;
        }
    }

    public class ViperComboBoxSelectionChanged : EventArgs, IViperControlEvents
    {
        public int Index;

        public ViperComboBoxSelectionChanged(int @index)
        {
            Index = @index;
        }
    }

    public class ViperCheckBoxStateChanged : EventArgs, IViperControlEvents
    {
        public bool State;

        public ViperCheckBoxStateChanged(bool @checked)
        {
            State = @checked;
        }
    }
}