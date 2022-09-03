using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ABI.Windows.UI;
using EnvControlPanel.Models;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WinUI;
using Microsoft.UI.Xaml.Media;
using SkiaSharp;

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
            temperatureData = new ObservableCollection<double> {0.0};
            lastTemperature = temperatureData.LastOrDefault();
            lastTemperatureStr = lastTemperature.ToString() + " °C";

            pressureData = new ObservableCollection<double> {0.0};
            lastPressure = pressureData.LastOrDefault();
            lastPressureStr = lastPressure.ToString() + " psi";


            Series = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = TemperatureData,
                    Fill = null,
                    GeometrySize = 0,
                    LineSmoothness = 1,
                    Stroke = new SolidColorPaint(SKColors.MediumVioletRed)
                },

                new LineSeries<double>
                {
                    Values = PressureData,
                    Fill= null,
                    GeometrySize = 0,
                    LineSmoothness = 1,
                    Stroke = new SolidColorPaint(SKColors.DeepSkyBlue)
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

            Random tempVal = new Random();

            double temp1 = tempVal.Next(-10, 120);
            double temp2 = tempVal.Next(-5, 120);


            TemperatureData.Add(temp1);
            PressureData.Add(temp2);

            UpdateReadings();
        }




        



    }
}
