using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Windows.Input;
using EnvControlPanel.Models;
using EnvControlPanel.Enums;
using System.Diagnostics;
using System.IO.Ports;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;

namespace EnvControlPanel.ViewModels
{
    public class MainViewModel : BindableBase
    {

        private ObservableCollection<SerialComDevice> serialItems;

        private SerialComDevice selectedComsPort;

        private SerialComDevice EmptyComPort;


        private int selectIndex;

        private int oldIndex;

        private bool enableConnect;
        private bool enableDisconnect;


        public ICommand RefreshCommand { get; set;}
        public ICommand ConnectCommand { get; set;}
        public ICommand DisconnectCommand { get; set;}

        



        public MainViewModel()
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




        public ObservableCollection<SerialComDevice> SerialItems
        {
            get  => serialItems; 

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
                //only enable connect if selected device is not empty com port
                SetProperty(ref selectIndex, value);
                SetProperty(ref selectedComsPort, serialItems[selectIndex]);

                EnableConnect = (selectIndex > 0);
                EnableDisconnect = selectedComsPort.IsOpen;
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
                Debug.WriteLine($"SerialCom: {str}");

                SerialItems.Add(new SerialComDevice(str));
            }
        }

        public void ClearSerialItems()
        {

            //Close all ports
            foreach (SerialComDevice device in serialItems)
            {
                device.Close();
            }

            //Clear all items and then add empty com port
            serialItems.Clear();
            serialItems.Add(EmptyComPort);
        }
         

        //Connecte to selected device
        public void ConnectToDevice()
        {
            //Close old selected-port/device before opening new selected port
            if((oldIndex != selectIndex) && (oldIndex > 0))
            {
                serialItems[oldIndex].Close();
            }

            selectedComsPort.Open();
            EnableDisconnect = selectedComsPort.IsOpen;

            //set old index to current selected device
            oldIndex = selectIndex;
        }


        //Disconnect from selected device
        public void DisconnectDevice()
        {
            if (selectedComsPort.IsOpen)
            {
                selectedComsPort.Close();
            }
            EnableDisconnect = selectedComsPort.IsOpen;
        }




    }
}
