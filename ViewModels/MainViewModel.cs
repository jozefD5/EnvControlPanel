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


namespace EnvControlPanel.ViewModels
{
    public class MainViewModel : BindableBase
    {

        private ObservableCollection<SerialComDevice> serialItems;

        private SerialComDevice selectedComsPort;

        public ICommand RefreshCommand { get; set;}
        public ICommand ConnectCommand { get; set;}

        private int selectIndex;    



        public MainViewModel()
        {
            PopulateData();

            RefreshCommand = new RelayCommand(RefreshDeviceList);
            ConnectCommand = new RelayCommand(ConnectToDevice, CanConnect);
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


        //Select item
        /*
        public SerialComDevice SelectedComsPort
        {
            get => selectedComsPort;

            set
            {
                SerialComDevice item = value as SerialComDevice;
                SetProperty(ref selectedComsPort, item);
                ((RelayCommand)ConnectCommand).RaiseCanExecuteChanged();
            }
        }
        */


        public int SelectIndex
        {
            get
            {
                return selectIndex;
            }

            set
            {
                Debug.WriteLine($"Set Index:{selectIndex}");

                int newIndex = (int)value;
                SetProperty(ref selectIndex, value);
                ((RelayCommand)ConnectCommand).RaiseCanExecuteChanged();

            }
        }




        public void RefreshDeviceList()
        {
            var newDevice = new SerialComDevice { SerialName = "COM-N", ConnectStatus = ComStatus.disconnect };

            SerialItems.Add(newDevice);
        }


        public void ConnectToDevice()
        {
 
        }


        private bool CanConnect()
        {
            Debug.WriteLine($"Set CanConnect:{selectIndex}");
            if (selectIndex > 0)
            {
                return true;
            }

            return false;
        }



    }
}
