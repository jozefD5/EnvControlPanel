using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvControlPanel.Models
{
    public static class EnvDevice
    {
        //Selected/Active serial devices
        public static SerialComDevice Device;



        //Serial TX commands
        public static string mt_tx_activate = "env_aenvm\t";              //activate monitoring thread
        public static string mt_tx_deactivate = "env_deaenvm\t";          //deactivate monitoring thread
        public static string mt_tx_rstatus = "env_rstatus\t";             //read monitoring status

        //Serial RX commands 
        private static string mt_rx_status = "env_status";                



    }
}
