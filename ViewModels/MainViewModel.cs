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



namespace EnvControlPanel.ViewModels
{
    public class MainViewModel : BindableBase
    {

        private ObservableCollection<SerialComDevice> serialItems;

        private SerialComDevice selectedComsPort;


        private SerialComDevice EmptyComPort;


        private int selectIndex;

        public bool CanConect;

        public ICommand RefreshCommand { get; set;}
        public ICommand ConnectCommand { get; set;}

        



        public MainViewModel()
        {
            
            EmptyComPort = new SerialComDevice("Empty COM Port");

            PopulateData();

            RefreshCommand = new RelayCommand(RefreshDeviceList);
            ConnectCommand = new RelayCommand(ConnectToDevice);

            RefreshDeviceList();
        }





        public void PopulateData()
        {

            serialItems = new ObservableCollection<SerialComDevice>
            {
                EmptyComPort
            };

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
                SetProperty(ref selectIndex, value);
                EnableConnect = (selectIndex > 0);  
            }
        }

      
        public bool EnableConnect
        {
            get => CanConect;

            set
            {
                SetProperty(ref CanConect, value);
            }
        }


        public void RefreshDeviceList()
        {
            int portsNumber = SerialPort.GetPortNames().Length;

            Debug.WriteLine($"Serial Ports: {portsNumber}");


            ClearSerialItems();



            foreach (string str in SerialPort.GetPortNames())
            {
                Debug.WriteLine($"SerialCom: {str}");

                SerialItems.Add(new SerialComDevice(str));
            }
        }


        public void ConnectToDevice()
        {
            SetProperty(ref selectedComsPort, serialItems[selectIndex]);

            Debug.WriteLine($"Set Index:{selectIndex}");
            Debug.WriteLine($"ComPort: {selectedComsPort.SerialPortName}");
        }


        public void ClearSerialItems()
        {

            int length = SerialItems.Count;

            SelectIndex = 0;
            selectIndex = 0;

            serialItems.Clear();

            serialItems.Add(EmptyComPort);
        }

   
    }
}
