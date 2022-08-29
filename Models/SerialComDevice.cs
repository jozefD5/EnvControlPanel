using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvControlPanel.Enums;
using System.IO.Ports;

namespace EnvControlPanel.Models
{
    public class SerialComDevice
    {
        public string serialPortName { get; set; }
        public ComStatus ConnectStatus { get; set; }

        private SerialPort serialPort;

        private SerialConfig SerialConf;



        public SerialComDevice(string name)
        {
            this.serialPortName = name;
            this.ConnectStatus = ComStatus.disconnect;

            serialPort = new SerialPort();

            SetSerialConf();
        }


        public string SerialPortName
        {
            get => serialPortName;

            set
            {
                serialPortName = value;
            }
        }









        //Set default serial config parameters
        private void SetSerialConf()
        {
            SerialConf.Port = this.SerialPortName;
            SerialConf.BaudRate = 115200;
            SerialConf.DataBits = 8;
            SerialConf.ParityBits = Parity.None;
            SerialConf.StopBits = StopBits.One;
        }

    }
}
