using ChecklistTracker.Config;
using ChecklistTracker.Controls.Click;
using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace ChecklistTracker.Controls
{
    public sealed partial class RewardControl : UserControl
    {
        internal LayoutParams Layout { get; private set; }

        internal RewardViewModel ViewModel { get; private set; }

        internal RewardControl(RewardViewModel viewModel, LayoutParams layout)
        {
            InitializeComponent();

            ViewModel = viewModel;
            Layout = layout;

            var callbacks = new ClickCallbacks();
            callbacks.OnClick = ViewModel.OnClick;
            callbacks.OnScroll = ViewModel.OnScroll;

            this.ConfigureClickHandler(callbacks);
        }
    }
}
