using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viper.Game.Interfaces;

namespace Viper.Game.Events
{
    public class SearchPanelIsSearchingChanged : EventArgs, IPanelEvents
    {
        public bool IsSearching;

        public SearchPanelIsSearchingChanged(bool @isSearching)
        {
            IsSearching = @isSearching;
        }
    }
}
