using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
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
            lastTemperatureStr = lastTemperature.ToString() + " °C";

            pressureData = new ObservableCollection<double> {0.0};
            lastPressure = pressureData.LastOrDefault();
            lastPressureStr = lastPressure.ToString() + " psi";


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





        //Temperature data control section
        //Collection containing record of received temperature data
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



        //Pressure data control section
        //Collection containing record of received pressure data
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




        //General control section
        //Send command to read device monitoring status
        public bool DeviceStatus
        {
            get => deviceStatus;

            set
            {
                SetProperty(ref deviceStatus, value);
                SetDeviceState();
            }
        }


        //Send activate/deactivate monitoring command and request monitoring status
        public void SetDeviceState()
        {
            if (deviceStatus)
            {
                EnvDevice.Device.SerialWrite(EnvCommand.mt_tx_activate);
            }
            else
            {
                EnvDevice.Device.SerialWrite(EnvCommand.mt_tx_deactivate);
            }
        }


        //Send command to read device monitoring status
        public void ReadDeviceStatus()
        {
            EnvDevice.Device.SerialWrite(EnvCommand.mt_tx_rstatus);
        }



        private void NewDataProcess(object sender, NewEnvDataEventArgs eventArgs)
        {
            //Debug.WriteLine($"New Data: {eventArgs.SerialDataStr}");

            string str = eventArgs.SerialDataStr;

            switch (str)
            {
                case string a when str.Contains(EnvCommand.mt_rx_status):
                    Debug.WriteLine($"Data: {a}");
                    break;

                case string a when str.Contains(EnvCommand.mt_rx_temp_data):
                    SeperateReadings(EnvCommand.mt_rx_temp_data, ref a);
                    break;

                case string a when str.Contains(EnvCommand.mt_rx_pres_data):
                    SeperateReadings(EnvCommand.mt_rx_pres_data, ref a);
                    break;



                default:
                    Debug.WriteLine("Unsupported Command!!!");
                    break;

            }




        }



        //Seperate temperature or pressure readings and add data to collection and display lates data
        private void SeperateReadings(string select,ref string rxData)
        {
            string str = new string((from c in rxData
                                     where char.IsDigit(c) || c == '.'
                                     select c).ToArray());

            double val = double.Parse(str, System.Globalization.CultureInfo.InvariantCulture);


            if (select == EnvCommand.mt_rx_temp_data)
            {
                Debug.WriteLine($"Temp: {val}");

                TemperatureData.Add(val);
                LastTemp = val;
                LimitCollectionSize(TemperatureData);
            } 
            else if (select == EnvCommand.mt_rx_pres_data)
            {
                Debug.WriteLine($"Pres: {val}");
                PressureData.Add(val);
                LimitCollectionSize(TemperatureData);
            }

            //UpdateReadings();
        }



    }
}
