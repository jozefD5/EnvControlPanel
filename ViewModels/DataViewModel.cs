using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using ABI.Windows.UI;
using EnvControlPanel.Models;
using EnvControlPanel.Views;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using SkiaSharp;
using Windows.Foundation.Collections;
using Windows.UI.Core;

namespace EnvControlPanel.ViewModels
{
    public class DataViewModel : BindableBase
    {
        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        private ObservableCollection<double> temperatureData;
        private ObservableCollection<double> pressureData;
        private double lastTemperature;
        private double lastPressure;

        public double MaxTemp { get; set; }
        public double MinTemp { get; set; }

        public double MaxPressure { get; set; }
        public double MiniPressure { get; set; }

        private int  collectionSizeLimit;
        private bool deviceStatus;
        private bool temoperatureMonitoring;
        private bool pressureMonitoring;


        public ICommand AddTempCommand { get; set; }
        public ICommand SetDeviceStateCommand { get; set; }
        public ICommand ReadDeviceStatusCommand { get; set; }

        public ObservableCollection<ISeries> SeriesTemperature { get; set; }
        public ObservableCollection<ISeries> SeriesPressure { get; set; }


          
        public DataViewModel()
        {
            //Initial settings
            MaxTemp = 140;
            MinTemp = -50;
            MaxPressure = 120;
            MiniPressure = -20;
            collectionSizeLimit = 50;
            deviceStatus = false;

            //Add event handler for incomming new data
            EnvDevice.dataFlow.NewEnvData += NewDataProcess;

            //Setup
            temperatureData = new ObservableCollection<double> {0.0};
            lastTemperature = temperatureData.LastOrDefault();

            pressureData = new ObservableCollection<double> {0.0};
            lastPressure = pressureData.LastOrDefault();


            //Setup temperature and presure graphs
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


            //Add commands for UI components
            AddTempCommand = new RelayCommand(AddTemp);
            SetDeviceStateCommand = new RelayCommand(SetDeviceState);
            ReadDeviceStatusCommand = new RelayCommand(ReadDeviceStatus);
        }



        //Collection containing record of received temperature data
        public ObservableCollection<double> TemperatureData
        {
            get => temperatureData;

            set
            {
                SetProperty(ref temperatureData, value);
            }
        }

        //Last/newest temperature value in collection
        public double LastTemperature
        {
            get => lastTemperature;

            set
            {
                SetProperty(ref lastTemperature, value);
            }
        }



        //Collection containing record of received pressure data
        public ObservableCollection<double> PressureData
        {
            get => pressureData;

            set
            {
                SetProperty(ref pressureData, value);
            }
        }

        //Last/newest pressure value in collection
        public double LastPressure
        {
            get => lastPressure;

            set
            {
                SetProperty(ref lastPressure, value);
            }
        }




        //Keep collection size withinthe limit by removing  required ammount of elements
        private void LimitCollectionSize(ObservableCollection<double> collection)
        {
            if(collection.Count() >= collectionSizeLimit)
            {
                for(int i=0; i< (collectionSizeLimit/2); i++)
                {
                    collection.RemoveAt(i);
                }
            }
        }


        //Update UI graph components
        private void UpdateGraph()
        {
            LastTemperature = temperatureData.LastOrDefault();
            LastPressure = pressureData.LastOrDefault();

            LimitCollectionSize(TemperatureData);
            LimitCollectionSize(PressureData);
        }


        public void AddTemp()
        {
            Random tempVal = new();

            double temp1 = tempVal.Next(-10, 120);
            double temp2 = tempVal.Next(-5, 120);

            TemperatureData.Add(temp1);
            PressureData.Add(temp2);

            LastTemperature = temperatureData.LastOrDefault();
            LastPressure = pressureData.LastOrDefault();

            LimitCollectionSize(TemperatureData);
            LimitCollectionSize(PressureData);
        }



        //Device monitoring status, activates/deactivate monitoring
        public bool DeviceStatus
        {
            get => deviceStatus;

            set
            {
                SetProperty(ref deviceStatus, value);
                SetDeviceState();
            }
        }

        //Activate/deactivate temperature monitoring 
        public bool TemoperatureMonitoring
        {
            get => temoperatureMonitoring;

            set
            {
                SetProperty(ref temoperatureMonitoring, value);
                SetTemperatureMonitoring();
            }
        }

        //Activate/deactivate pressure monitoring 
        public bool PressureMonitoring
        {
            get => pressureMonitoring;

            set
            {
                SetProperty(ref pressureMonitoring, value);
                SetPressureMonitoring();
            }
        }





        public void ReadDeviceStatus()
        {
            EnvDevice.Device.SerialWrite(SerialCommands.env_sc_rstatus);
        }

        public void SetDeviceState()
        {
            if (deviceStatus)
            {
                EnvDevice.Device.SerialWrite(SerialCommands.env_sc_activate);
            }
            else
            {
                EnvDevice.Device.SerialWrite(SerialCommands.env_sc_deactivate);
            }
        }

        public void SetTemperatureMonitoring()
        {
            if (temoperatureMonitoring)
            {
                EnvDevice.Device.SerialWrite(SerialCommands.env_sc_temp_activate);
            }
            else
            {
                EnvDevice.Device.SerialWrite(SerialCommands.env_sc_temp_deactivate);
            }
        }

        public void SetPressureMonitoring()
        {
            if (pressureMonitoring)
            {
                EnvDevice.Device.SerialWrite(SerialCommands.env_sc_pres_activate);
            }
            else
            {
                EnvDevice.Device.SerialWrite(SerialCommands.env_sc_pres_deactivate);
            }
        }






        //Process new UART data on rx event
        private void NewDataProcess(object sender, NewEnvDataEventArgs eventArgs)
        {
            string str = eventArgs.SerialDataStr;

            switch (str)
            {
                case string a when str.Contains(SerialCommands.env_sc_status):
                    Debug.WriteLine($"Data: {a}");
                    break;

                case string a when str.Contains(SerialCommands.env_sc_temp_data):
                    SeperateReadings(SerialCommands.env_sc_temp_data,  a);
                    break;

                case string a when str.Contains(SerialCommands.env_sc_pres_data):
                    SeperateReadings(SerialCommands.env_sc_pres_data,  a);
                    break;

                default:
                    Debug.WriteLine($"Unsupported Command!: {str}");
                    break;
            }
        }



        //Seperate temperature or pressure readings and add data to collection and display lates data
        private void SeperateReadings(string select, string rxData)
        {
            string str = new string((from c in rxData
                                     where char.IsDigit(c) || c == '.'
                                     select c).ToArray());

            double val = double.Parse(str, System.Globalization.CultureInfo.InvariantCulture);


            //update UI components
            Task.Run(() =>
            {
                _ = _dispatcherQueue.TryEnqueue(() =>
                {
                    if (select == SerialCommands.env_sc_temp_data)
                    {
                        TemperatureData.Add(val);
                        UpdateGraph();
                    }
                    else if (select == SerialCommands.env_sc_pres_data)
                    {
                        PressureData.Add(val);
                        UpdateGraph();
                    }
                });
            });
        }



    }
}
