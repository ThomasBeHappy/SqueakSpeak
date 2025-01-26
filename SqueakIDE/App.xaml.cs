using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SqueakIDE.Windows;

namespace SqueakIDE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.ShutdownMode = ShutdownMode.OnMainWindowClose;

            // Show startup window
            var startupWindow = new StartupWindow();
            var mainWindow = new MainWindow();
            MainWindow = mainWindow;

            var result = startupWindow.ShowDialog();

            if (result == true)
            {
                // Create and show main window
                mainWindow.LoadProject(startupWindow.SelectedProject);
                mainWindow.Show();
                
                // Keep application alive
                this.MainWindow = mainWindow;
            }
            else
            {
                this.Shutdown();
            }
        }
    }
}
