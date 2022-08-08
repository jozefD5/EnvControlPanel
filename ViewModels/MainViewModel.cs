using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using EnvControlPanel.Models;

namespace EnvControlPanel.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private SerialComDevice selectedComsPort;
        private ObservableCollection<SerialComDevice> serialItems;




        public MainViewModel()
        {
            PopulateData();
            
        }



        public void PopulateData()
        {
            var com0 = new SerialComDevice
            {
                SerialName = "COM 0"
            };

            var com1 = new SerialComDevice
            {
                SerialName = "COM 1"
            };

            var com2 = new SerialComDevice
            {
                SerialName = "COM 2"
            };


            serialItems = new ObservableCollection<SerialComDevice>
            {
                com0, com1, com2
            };

        }


        public ObservableCollection<SerialComDevice> SerialItems
        {
            get { return serialItems; }

            set
            {
                SetProperty(ref serialItems, value);
            }
        }


        public SerialComDevice SelectedComsPort
        {
            get => selectedComsPort;

            set
            {
                SetProperty(ref selectedComsPort, value);
            }
        }





    }
}
