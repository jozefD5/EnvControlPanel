using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
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










        //Open serial com port and assign received data handler
        public static void OpenComs()
        {
            if (Device.Open())
            {
                Device.Port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            }
        }



        //Output if com port is open
        public static bool IsOpen
        {
            get => Device.IsOpen;
        }









        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string dataStream = sp.ReadLine();

            Debug.WriteLine($"Temp: {dataStream}");

            /*
            if (dataStream.Contains(EnvDevice.mt_rx_temp_data))
            {
                //Debug.WriteLine($"Temp: {str}");
            }
            */
        }
    }
}
