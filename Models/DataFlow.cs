using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvControlPanel.Models
{
    public class DataFlow
    {
        public event EventHandler? NewEnvData;

        public void OnNewEnvData(string str)
        {
            NewEnvData?.Invoke(this, EventArgs.Empty);
        }
    }
}
