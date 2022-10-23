using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvControlPanel.Models
{
    //List of available serial commands, supported instructions
    public static class SerialCommands
    {
        //Serial TX commands
        public static string env_sc_activate = "env_aenvm\t";              //activate enviroment monitoring
        public static string env_sc_deactivate = "env_deaenvm\t";          //deactivate enviroment monitoring
        public static string env_sc_rstatus = "env_rstatus\t";             //request monitoring status
        public static string env_sc_temp_activate = "env_temp_act\t";      //activate temperature monitoring
        public static string env_sc_temp_deactivate = "env_temp_dact\t";   //deactivate temperature monitoring
        public static string env_sc_pres_activate = "env_pres_act\t";      //activate temperature monitoring
        public static string env_sc_pres_deactivate = "env_pres_dact\t";   //deactivate temperature monitoring
        public static string env_sc_rim_normal = "env_rimnorm\t";          //normal report interval mode
        public static string env_sc_rim_slow = "env_rimslow\t";            //normal report interval mode
        public static string env_sc_rim_fast = "env_rimfast\t";            //normal report interval mode




        //Serial RX commands 
        public static string env_sc_status = "env_status";                 //monitoring status
        public static string env_sc_temp_data = "env_temp";                //temperature data in deg C
        public static string env_sc_pres_data = "env_pres";                //pressure data in psi
    }
}
