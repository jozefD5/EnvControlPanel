﻿using System;
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
        public static string mt_rx_status = "env_status";
        public static string mt_rx_temp_data = "env_temp";
        public static string mt_rx_pres_data = "env_pres";



    }
}
