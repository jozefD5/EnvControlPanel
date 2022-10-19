using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvControlPanel.Models
{
    //Data flow class handels, triggers, events when new data are reacived via active serial COM port
    public class DataFlow
    {
        public event EventHandler<NewEnvDataEventArgs> NewEnvData;

        public void OnNewEnvData(string serialDataStr)
        {
            NewEnvData.Invoke(this, new NewEnvDataEventArgs(serialDataStr));
        }      
    }




    //New enviroment data, a string argument to handle incomming commands or data via serial port
    public class NewEnvDataEventArgs
    {
        public NewEnvDataEventArgs(string serialDataStr)
        {
            SerialDataStr = serialDataStr;
        }

        public string SerialDataStr { get; }
    }

}





