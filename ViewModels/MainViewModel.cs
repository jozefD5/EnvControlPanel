﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Windows.Input;
using EnvControlPanel.Models;
using EnvControlPanel.Enums;
using System.Diagnostics;


namespace EnvControlPanel.ViewModels
{
    public class MainViewModel : BindableBase
    {

        private ObservableCollection<SerialComDevice> serialItems;

        private SerialComDevice selectedComsPort;

        private int selectIndex;

        public bool CanConect;

        public ICommand RefreshCommand { get; set;}
        public ICommand ConnectCommand { get; set;}

        

           



        public MainViewModel()
        {
            PopulateData();

            RefreshCommand = new RelayCommand(RefreshDeviceList);
            ConnectCommand = new RelayCommand(ConnectToDevice);
        }





        public void PopulateData()
        {
            var comEmpty = new SerialComDevice {SerialName = "Empty Port", ConnectStatus = ComStatus.disconnect };

            var com0 = new SerialComDevice { SerialName = "COM 0", ConnectStatus = ComStatus.disconnect};

            var com1 = new SerialComDevice { SerialName = "COM 1", ConnectStatus = ComStatus.disconnect };

            var com2 = new SerialComDevice { SerialName = "COM 2", ConnectStatus = ComStatus.disconnect };

            serialItems = new ObservableCollection<SerialComDevice>
            {
                comEmpty, com0, com1, com2
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
                SetProperty(ref selectedComsPort, serialItems[selectIndex]);

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
            var newDevice = new SerialComDevice { SerialName = "COM-N", ConnectStatus = ComStatus.disconnect };

            SerialItems.Add(newDevice);
        }


        public void ConnectToDevice()
        {
            Debug.WriteLine($"Set Index:{selectIndex}");
            Debug.WriteLine($"ComPort: {selectedComsPort.SerialName}");
        }



   
    }
}