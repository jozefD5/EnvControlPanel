using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EnvControlPanel.Models;




namespace EnvControlPanel.ViewModels
{
    public class DataViewModel : BindableBase
    {

        private ObservableCollection<double> tempData;
        private double newTemp;
        private double lastTemp;
        private string lastTempStr;

        public double MaxTemp { get; set; }
        public double MinTemp { get; set; }

        public ICommand AddTempCommand { get; set; }







        public DataViewModel()
        {
            //Settings
            MaxTemp = 140;
            MinTemp = -50;

            //Setup
            newTemp = MinTemp;
            tempData = new ObservableCollection<double> { newTemp };
            lastTemp = tempData.LastOrDefault();
            lastTempStr = lastTemp.ToString() + " °C";
       
            AddTempCommand = new RelayCommand(AddTemp);
        }




        public ObservableCollection<double> TempData
        {
            get => tempData;

            set
            {
                SetProperty(ref tempData, value);
            }
        }

        public double LastTemp
        {
            get => lastTemp;

            set
            {
                SetProperty(ref lastTemp, value);
            }
        }

        public string LastTempStr
        {
            get => lastTempStr;

            set
            {
                SetProperty(ref lastTempStr, value);
            }
        }



        private void UpdateTemperature()
        {
            LastTemp = tempData.LastOrDefault();
            LastTempStr = lastTemp.ToString() + " °C";
        }

     

        public void AddTemp()
        {
            newTemp ++;

            if (newTemp > MaxTemp) { newTemp = MinTemp; }
            TempData.Add(newTemp);

            UpdateTemperature();
        }





    }
}
