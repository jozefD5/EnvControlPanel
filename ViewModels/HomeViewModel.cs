using EnvControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


using EnvControlPanel.Enums;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;

namespace EnvControlPanel.ViewModels
{
    public class HomeViewModel : BindableBase
    {

        private ObservableCollection<SerialComDevice> serialItems;

        private readonly SerialComDevice EmptyComPort;

        private int selectIndex;

        private int oldIndex;

        private bool enableConnect;
        private bool enableDisconnect;


        public ICommand RefreshCommand { get; set; }
        public ICommand ConnectCommand { get; set; }
        public ICommand DisconnectCommand { get; set; }




        public HomeViewModel()
        {
            //Add empty com port to device list
            EmptyComPort = new SerialComDevice("Empty COM Port");

            serialItems = new ObservableCollection<SerialComDevice>
            {
                EmptyComPort
            };

            //Commands
            RefreshCommand = new RelayCommand(RefreshDeviceList);
            ConnectCommand = new RelayCommand(ConnectToDevice);
            DisconnectCommand = new RelayCommand(DisconnectDevice);

            //Get list of all connected devices
            RefreshDeviceList();
        }



        //Collection containing all detected/available serial COm devices
        public ObservableCollection<SerialComDevice> SerialItems
        {
            get => serialItems;

            set
            {
                SetProperty(ref serialItems, value);
            }
        }

        public int SelectIndex
        {
            get => selectIndex;

            set
            {
                //only enable connect if selected device is not empty COM port
                SetProperty(ref selectIndex, value);

                if (value > 0)
                {
                    SetProperty(ref EnvDevice.Device, serialItems[value]);

                    EnableConnect = true;
                    EnableDisconnect = EnvDevice.IsOpen;
                }
                else
                {
                    EnableConnect = false;
                    EnableDisconnect = false;
                }
            }
        }

        public bool EnableConnect
        {
            get => enableConnect;

            set
            {
                SetProperty(ref enableConnect, value);
            }
        }

        public bool EnableDisconnect
        {
            get => enableDisconnect;

            set
            {
                SetProperty(ref enableDisconnect, value);
            }
        }




        //Close all connections and then refresh device list
        public void RefreshDeviceList()
        {
            ClearSerialItems();
            EnableDisconnect = false;

            foreach (string str in SerialPort.GetPortNames())
            {
                SerialItems.Add(new SerialComDevice(str));
            }
        }


        //Close all available COM ports and clear all items and then add empty com port
        public void ClearSerialItems()
        {    
            foreach (SerialComDevice device in serialItems)
            {
                device.Close();
            }

            serialItems.Clear();
            serialItems.Add(EmptyComPort);
        }


        //Connecte to selected device
        public void ConnectToDevice()
        {
            //Close old selected-port/device before opening new selected port
            if ((oldIndex != selectIndex) && (oldIndex > 0))
            {
                serialItems[oldIndex].Close();
            }

            EnvDevice.OpenComs();
            EnableDisconnect = EnvDevice.IsOpen;

            //set old index to current selected device
            oldIndex = selectIndex;
        }


        //Disconnect from selected device
        public void DisconnectDevice()
        {
            try
            {
                if ((EnvDevice.IsOpen) && (selectIndex > 0))
                {
                    EnvDevice.Device.Close();
                }
                EnableDisconnect = EnvDevice.IsOpen;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Acrtion: DisconnectDevice()     Exception");
                Debug.WriteLine(ex.Message);
            }
        }



    }
}
