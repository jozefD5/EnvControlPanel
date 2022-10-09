using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvControlPanel.Models
{
    public class DataFlow
    {
 
        public event EventHandler<NewEnvDataEventArgs>? NewEnvData;

        public void OnNewEnvData(string serialDataStr)
        {
            NewEnvData?.Invoke(this, new NewEnvDataEventArgs(serialDataStr));
        }      
    }
}



public class NewEnvDataEventArgs
{
    public NewEnvDataEventArgs(string serialDataStr)
    {
        SerialDataStr = serialDataStr;
    }

    public string SerialDataStr { get;}
}