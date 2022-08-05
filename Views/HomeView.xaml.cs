using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

using EnvControlPanel.Model;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EnvControlPanel.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeView : Page
    {
        private IList<EnvDevice> _devices;
        private bool _initialized;


        public HomeView()
        {
            this.InitializeComponent();
            PopulateData();
        }

        
        private void PopulateData()
        {
            if (!_initialized)
            {
                _initialized = true;

                EnvDevice d0 = new("COM0");
                EnvDevice d1 = new("COM1");
                EnvDevice d2 = new("COM2");

                _devices = new List<EnvDevice>
                {
                    d0, d1, d2
                };

                _initialized = true;
            }
        }
    }
}
