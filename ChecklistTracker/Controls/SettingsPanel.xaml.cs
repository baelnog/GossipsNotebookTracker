using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChecklistTracker.Controls
{
    public sealed partial class SettingsPanel : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(SettingsViewModel), typeof(SettingsPanel), new PropertyMetadata(null));

        public SettingsViewModel ViewModel
        {
            get => (SettingsViewModel)GetValue(ViewModelProperty);
            set
            {
                SetValue(ViewModelProperty, value);
            }
        }

        public SettingsPanel()
        {
            this.InitializeComponent();
        }
    }
}
