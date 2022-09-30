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

using EnvControlPanel.Views;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EnvControlPanel
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        


        public MainWindow()
        {
            this.InitializeComponent();
            SetNavigationView("HomeViewNv");
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem item = args.SelectedItem as NavigationViewItem;

            SetNavigationView(item.Tag.ToString());
        }


        private void SetNavigationView(string viewTag)
        {
            switch (viewTag)
            {
                case "HomeViewNv":
                    ContentFrame.Navigate(typeof(Views.HomeView));
                    break;

                case "DataDisplyViewNv":
                    ContentFrame.Navigate(typeof(Views.DataDisplayView));
                    break;

                case "SettingsViewNv":
                    ContentFrame.Navigate(typeof(Views.SettingsView));
                    break;

                default:
                    break;
            }

        }

        
    }
}
