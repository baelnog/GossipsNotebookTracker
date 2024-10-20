using ChecklistTracker.Config;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.Layout.GossipNotebook;
using ChecklistTracker.LogicProvider;
using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml;
using System.Diagnostics.Contracts;
using System.Reflection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChecklistTracker
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private static Lazy<string> ProgramDir = new Lazy<string>(() => new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName);
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

            var logicEngine = config.UserConfig.ShowLocationTracker ? new LogicEngine(config, "v8.0") : null;

            var inventory = new Inventory(logicEngine);

            CheckListViewModel.GlobalInstance = new CheckListViewModel(
                config,
                inventory,
                logicEngine);

            MainWindow = new MainWindow(config);
            MainWindow.Activate();

            LayoutTracker();

            CheckListViewModel.GlobalInstance.Config.UserConfig.OnPropertyChanged(
                nameof(UserConfig.LayoutPath),
                (o, args) => MainWindow.DispatcherQueue.TryEnqueue(DispatchLayoutChange));
        }

        private MainWindow? MainWindow;

        private void LayoutTracker()
        {
            Contract.Assert(MainWindow != null);
            Contract.Assert(CheckListViewModel.GlobalInstance != null);

            var layoutDoc = ResourceFinder.ReadResourceFile(CheckListViewModel.GlobalInstance.Config.UserConfig.LayoutPath).Result;
            var layout = GossipNotebookLayout.ParseLayout(layoutDoc);

            // TODO: Enumerate all windows.
            var window = layout.Windows[0];

            MainWindow.LayoutDesign(window, layout.Style);

            if (layout.TrackerConfig.EnableLogic)
            {
                CheckPage.Launch();
            }
        }

        private void DispatchLayoutChange()
        {
            LayoutTracker();
        }


        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            Logging.WriteLine("Exception caught", e.Exception);
        }
    }
}
