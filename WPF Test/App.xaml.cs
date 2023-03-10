using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Prod_Tools {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        MainWindow mainWindow;

        private void Start(object _sender, StartupEventArgs _e) {

            mainWindow = new MainWindow();
            mainWindow.Show();

            CompositionTarget.Rendering += Update;

        }

        private void Update(object _sender, EventArgs _e) {
            mainWindow.Update();
        }

    }
    
}