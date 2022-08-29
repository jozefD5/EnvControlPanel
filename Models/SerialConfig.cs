using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EnvControlPanel.Models
{
    public struct SerialConfig
    {
        public string Port { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public Parity ParityBits { get; set; }
        public StopBits StopBits { get; set; }
    }
}
