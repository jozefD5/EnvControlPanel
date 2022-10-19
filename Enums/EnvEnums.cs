using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvControlPanel.Enums
{
    //Enum to determine active view tag
    public enum ViewTagType
    {
        HomeView,
        DataDisplayView,
        SettingsView
    }


    //Serial COM port device statuss
    public enum ComStatus
    {
        connect, 
        disconnect,
        error
    }
}
