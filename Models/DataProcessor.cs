using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvControlPanel.Models
{


    public  class DataProcessor
    {
        
        //public event EventHandler NewEnvData?;


        /*
        protected virtual void OnNewEnvData(EventArgs e)
        {
            EventHandler handler = NewEnvData;

        }
        */

        public void OnNewEnvData(string str)
        {
            //NewEnvData?.Invoke(this, EventArgs.Empty);
        }




    }
}
