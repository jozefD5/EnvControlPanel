using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvControlPanel.Model
{
    public class EnvDevice
    {
        private string _deviceSerialName;
        private int _deviceId;
        private static int _idCount = 0;
        


        public EnvDevice(string serialName)
        {
            DeviceSerialName = serialName;
            _deviceId = _idCount;
            _idCount++;

        }




        public string DeviceSerialName
        {
            get => _deviceSerialName;
            set { 
                _deviceSerialName = value; 
            }
        }


        public string DeviceId
        {
            get => _deviceId.ToString();
        }




    }
}
