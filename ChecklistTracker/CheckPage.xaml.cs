using Antlr4.Runtime.Misc;
using ChecklistTracker.LogicProvider;
using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChecklistTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CheckPage : Page
    {
        private CheckListViewModel? ViewModel { get => CheckListViewModel.GlobalInstance; }

        public bool OpenRegionPage { get; set; } = true;

        public CheckPage()
        {
            this.InitializeComponent();
        }

        public void OnUndo(object s, RoutedEventArgs e)
        {
            ViewModel?.Undo();
        }

        public void OnRedo(object s, RoutedEventArgs e)
        {
            ViewModel?.Redo();
        }

        private void OnToggleSkulls(object sender, RoutedEventArgs e)
        {
            if (sender is AppBarToggleButton toggle)
            {
                ViewModel?.ToggleSkulls(toggle.IsChecked == true);
            }
        }

        private bool UpdatingSelection = false;
        private HintRegionViewModel? Selected = null;
        public void OnRegionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UpdatingSelection)
            {
                return;
            }
            UpdatingSelection = true;
            var item = e.AddedItems.FirstOrDefault();
            if (sender is ListView view)
            {
                if (item is HintRegionViewModel region)
                {
                    if (Selected != region)
                    {
                        Selected = region;
                        ViewModel?.SelectRegion(region.Region);
                        view.SelectedItem = region;
                    }
                }
                else if (e.RemovedItems.Contains(Selected))
                {
                    Selected = null;
                    ViewModel?.SelectRegion(null);
                }
            }
            UpdatingSelection = false;
        }

        public void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is HintRegionViewModel region)
            {
                ViewModel?.SelectRegion(region.Region);
                Selected = region;
            }
            else if (args.SelectedItem == null)
            {
                ViewModel?.SelectRegion(null);
                Selected = null;
            }
        }

        internal static void Launch(LogicEngine logicEngine, Inventory inventory)
        {
            CheckListViewModel.GlobalInstance = new CheckListViewModel(inventory, logicEngine);
            var window = new Window();
            var frame = new Frame();
            frame.Navigate(typeof(CheckPage));
            window.Content = frame;
            window.Activate();
        }
    }
}
