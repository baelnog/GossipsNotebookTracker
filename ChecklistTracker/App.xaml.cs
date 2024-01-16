using ChecklistTracker.Config;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.LogicProvider;
using ChecklistTracker.LogicProvider.DataFiles.Settings;
using ChecklistTracker.ViewModel;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChecklistTracker
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private static Lazy<string> ProgramDir = new Lazy<string>(() => new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName);
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.UnhandledException += App_UnhandledException;
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {

            var config = TrackerConfig.Init().Result;

            var settings = Settings.ReadFromJson($"{ProgramDir.Value}/settings/season7-base.json").Result;
            var logicEngine = new LogicEngine(config, "v8.0", settings);

            var inventory = new Inventory(logicEngine);

            CheckListViewModel.GlobalInstance = new CheckListViewModel(
                config,
                inventory,
                config.UserConfig.ShowLocationTracker ? logicEngine : null);

            m_window = new MainWindow(config);
            m_window.Activate();

            if (config.UserConfig.ShowLocationTracker)
            {
                CheckPage.Launch();
            }

            //var window = AppWindow.Create();
            //Frame appWindowContentFrame = new Frame();
            //appWindowContentFrame.Navigate(typeof(CheckPage));


            //m_checkWindow.Activate();
        }

        private Window m_window;
        private Window m_checkWindow;


        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            Logging.WriteLine("Exception caught", e.Exception);
        }
    }
}
