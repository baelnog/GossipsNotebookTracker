using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChecklistTracker.Controls
{
    public sealed partial class EntranceTableControl : UserControl
    {

        internal EntranceTableViewModel ViewModel { get; set; }
        internal LayoutParams Layout { get; set; }

        public EntranceTableControl()
        {
            this.InitializeComponent();
        }
    }
}
