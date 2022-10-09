using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvControlPanel.Enums;
using EnvControlPanel.ViewModels;
using System.IO.Ports;
using System.Diagnostics;
using EnvControlPanel.Models;
using System.Text.RegularExpressions;

namespace EnvControlPanel.Models
{
    public class SerialComDevice : BindableBase
    {
        public ComStatus connectStatus;

        private SerialPort serialPort;




        public SerialComDevice(string name)
        {
            ConnectStatus = ComStatus.disconnect;

            serialPort = new SerialPort
            {
                PortName = name
            };

            SetSerialConf();
        }



        public string PortName
        {
            get => serialPort.PortName;
            set => serialPort.PortName = value;
        }

        public int BaudRate
        {
            get => serialPort.BaudRate;
            set => serialPort.BaudRate = value;
        }

        public int DataBits
        {
            get => serialPort.DataBits; 
            set => serialPort.DataBits = value;
        }

        public Parity ParityBits
        {
            get => serialPort.Parity;
            set => serialPort.Parity = value;
        }

        public StopBits StopBit
        {
            get => serialPort.StopBits; 
            set => serialPort.StopBits = value;
        }

        public ComStatus ConnectStatus
        {
            get => connectStatus;

            set
            {
                SetProperty(ref connectStatus, value);
            }
        }

        public bool IsOpen
        {
            get => serialPort.IsOpen;
        }



        public void SerialWrite(string str)
        {
            if (IsOpen)
            {
                serialPort.Write(str);
            }
        }

        public void Close()
        {
            if (IsOpen)
            {
                serialPort.Close();
            }
            CheckConnect();
        }

        public void Open()
        {
            try
            {
                if (!IsOpen)
                {
                    serialPort.Open();

                    serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                }
                CheckConnect();

            }
            catch(System.IO.FileNotFoundException ex)
            {
                Debug.WriteLine("Exception: " + ex.ToString());
                ConnectStatus = ComStatus.error;
            }
        }


        private void CheckConnect()
        {
            if (IsOpen)
            {
                ConnectStatus = ComStatus.connect;
            }
            else
            {
                ConnectStatus = ComStatus.disconnect;
            }
        }



        //Set default serial config parameters
        private void SetSerialConf()
        {
            BaudRate = 230400;
            DataBits = 8;
            ParityBits = Parity.None;
            StopBit = StopBits.One;
        }





        private void DataProcess(string rxData)
        {
            string str = new string((from c in rxData
                                     where char.IsDigit(c) || c == '.'
                                     select c).ToArray());

        }

     



        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            string dataStream = sp.ReadLine();
            //int dataCount = dataStream.Length;
            //Debug.WriteLine($"sl:   {dataStream}");

            if (dataStream.Contains(EnvDevice.mt_rx_temp_data))
            {
                

                

                //Debug.WriteLine($"Temp: {str}");
            }



        }


    }
}
