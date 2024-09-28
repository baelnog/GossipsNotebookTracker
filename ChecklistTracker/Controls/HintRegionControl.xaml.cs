using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChecklistTracker.Controls
{
    public sealed partial class HintRegionControl : UserControl
    {
        internal HintRegionViewModel? Region { get; set; }

        public Visibility ShowLocations { get; set; } = Visibility.Visible;

        public HintRegionControl()
        {
            this.InitializeComponent();
        }

        public void OnSelectLocation(object sender, SelectionChangedEventArgs args)
        {
            if (sender is ListView view)
            {
                foreach (var item in args.AddedItems)
                {
                    if (item is LocationViewModel location)
                    {
                        Region.Model.CheckLocation(location.Location);
                    }
                }
                if (args.AddedItems.Any())
                {
                    view.SelectedItem = null;
                }
            }
        }
    }
}
