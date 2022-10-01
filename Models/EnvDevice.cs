using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvControlPanel.Models
{
    public static class EnvDevice
    {
        private static string mt_activate = "env_aenvm\t";
        private static string mt_deactivate = "env_deaenvm\t";


        public static SerialComDevice Device;

    }
}
