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
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WinUI;
using Microsoft.UI.Xaml.Media;
using SkiaSharp;
using Windows.Foundation.Collections;

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

        private int collectionSizeLimit;

        private bool deviceStatus;


        public ICommand AddTempCommand { get; set; }
        public ICommand SetDeviceStateCommand { get; set; }

        public ObservableCollection<ISeries> SeriesTemperature { get; set; }
        public ObservableCollection<ISeries> SeriesPressure { get; set; }




          
        public DataViewModel()
        {
            //Settings
            MaxTemp = 140;
            MinTemp = -50;

            MaxPressure = 120;
            MiniPressure = -20;

            collectionSizeLimit = 50;

            //Setup
            temperatureData = new ObservableCollection<double> {0.0};
            lastTemperature = temperatureData.LastOrDefault();
            lastTemperatureStr = lastTemperature.ToString() + " °C";

            pressureData = new ObservableCollection<double> {0.0};
            lastPressure = pressureData.LastOrDefault();
            lastPressureStr = lastPressure.ToString() + " psi";

            
            //Set initial device status
            deviceStatus = false;


            //Setup temperature graph
            SeriesTemperature = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Name = "Temperature",
                    Values = TemperatureData,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.PaleVioletRed),
                    GeometrySize = 0,
                    LineSmoothness = 1,
                    TooltipLabelFormatter = (charPoin) => $"{charPoin.Context.Series.Name}: {charPoin.PrimaryValue}"
                }
            };

            //Setup pressure graph
            SeriesPressure = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Name = "Pressure",
                    Values = PressureData,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.MidnightBlue),
                    GeometrySize = 0,
                    LineSmoothness = 1,
                    TooltipLabelFormatter = (charPoin) => $"{charPoin.Context.Series.Name}: {charPoin.PrimaryValue}"
                }
            };


            AddTempCommand = new RelayCommand(AddTemp);
            SetDeviceStateCommand = new RelayCommand(SetDeviceState);

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


        public bool DeviceStatus
        {
            get => deviceStatus;

            set
            {
                SetProperty(ref deviceStatus, value);

                SetDeviceState();
            }
        }





        private void UpdateReadings()
        {
            LastTemp = temperatureData.LastOrDefault();
            LastTempStr = lastTemperature.ToString() + " °C";

            LastPressure = pressureData.LastOrDefault();
            LastPressureStr = lastPressure.ToString() + " psi";
        }

        


        private void LimitCollectionSize(ObservableCollection<double> collection)
        {
            int size = collection.Count();

            Debug.WriteLine($"Size: {size}");

            //Remove first two elements
            if(size >= collectionSizeLimit)
            {

                for(int i=0; i< (collectionSizeLimit/2); i++)
                {
                    collection.RemoveAt(i);
                }
            }
        }


        public void AddTemp()
        {

            Random tempVal = new();

            double temp1 = tempVal.Next(-10, 120);
            double temp2 = tempVal.Next(-5, 120);


            TemperatureData.Add(temp1);
            PressureData.Add(temp2);

            LimitCollectionSize(TemperatureData);
            LimitCollectionSize(PressureData);

            UpdateReadings();
            
        }


        //Send activate/deactivate monitoring command and request monitoring status
        public void SetDeviceState()
        {
            Debug.WriteLine($"DevStatus: {deviceStatus}");

            if (deviceStatus)
            {
                EnvDevice.Device.SerialWriteLine(EnvDevice.mt_tx_activate);
            }
            else
            {
                EnvDevice.Device.SerialWriteLine(EnvDevice.mt_tx_deactivate);
            }


            EnvDevice.Device.SerialWriteLine(EnvDevice.mt_tx_rstatus);
        }


    }
}
