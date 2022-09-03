using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using EnvControlPanel.Models;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinUI;



namespace EnvControlPanel.ViewModels
{
    public class DataViewModel : BindableBase
    {

        private ObservableCollection<double> temperatureData;
        private ObservableCollection<double> pressureData;


        private double lastTemperature;
        private string lastTemperatureStr;

        private double lastPressure;
        private string lastPressureStr;


        private double newTemp;
        private double newPressure;

        public double MaxTemp { get; set; }
        public double MinTemp { get; set; }

        public double MaxPressure { get; set; }
        public double MiniPressure { get; set; }
        


        public ICommand AddTempCommand { get; set; }

        public ObservableCollection<ISeries> Series { get; set; }





public DataViewModel()
        {
            //Settings
            MaxTemp = 140;
            MinTemp = -50;

            MaxPressure = 120;
            MiniPressure = -20;

            //Setup
            newTemp = MinTemp;
            temperatureData = new ObservableCollection<double> { newTemp };
            lastTemperature = temperatureData.LastOrDefault();
            lastTemperatureStr = lastTemperature.ToString() + " °C";

            newPressure = MiniPressure;
            pressureData = new ObservableCollection<double> { newPressure };
            lastPressure = pressureData.LastOrDefault();
            lastPressureStr = lastPressure.ToString() + " psi";


            Series = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = TemperatureData,
                    Fill = null
                }
            };

            AddTempCommand = new RelayCommand(AddTemp);
        }




        //Temperature 
        public ObservableCollection<double> TemperatureData
        {
            get => temperatureData;

            set
            {
                SetProperty(ref temperatureData, value);
            }
        }

        public double LastTemp
        {
            get => lastTemperature;

            set
            {
                SetProperty(ref lastTemperature, value);
            }
        }

        public string LastTempStr
        {
            get => lastTemperatureStr;

            set
            {
                SetProperty(ref lastTemperatureStr, value);
            }
        }


        //Pressure
        public ObservableCollection<double> PressureData
        {
            get => pressureData;

            set
            {
                SetProperty(ref pressureData, value);
            }
        }

        public double LastPressure
        {
            get => lastPressure;

            set
            {
                SetProperty(ref lastPressure, value);
            }
        }

        public string LastPressureStr
        {
            get => lastPressureStr;

            set
            {
                SetProperty(ref lastPressureStr, value);
            }
        }




        private void UpdateReadings()
        {
            LastTemp = temperatureData.LastOrDefault();
            LastTempStr = lastTemperature.ToString() + " °C";

            LastPressure = pressureData.LastOrDefault();
            LastPressureStr = lastPressure.ToString() + " psi";
        }

     

        public void AddTemp()
        {
            newTemp ++;
            newPressure++;

            if (newTemp > MaxTemp) { newTemp = MinTemp; }
            if(newPressure > MaxPressure) { newPressure = MiniPressure; }

            TemperatureData.Add(newTemp);
            PressureData.Add(newPressure);

            UpdateReadings();
        }




        



    }
}
