using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvControlPanel.Models;




namespace EnvControlPanel.ViewModels
{
    public class DataViewModel : BindableBase
    {

        private List<double> tempData;


        public DataViewModel()
        {
            tempData = new List<double> { 20.0, 30.0, 10.0, 40.0, 50.0 };

        }


        public double Temp
        {
            get => tempData.Last();
        }





  

    }
}
