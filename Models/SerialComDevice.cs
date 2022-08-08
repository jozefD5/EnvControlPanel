using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvControlPanel.Enums;


namespace EnvControlPanel.Models
{
    public class SerialComDevice
    {
        public string SerialName { get; set; }
        public ComStatus ConnectStatus { get; set; }


    }
}
