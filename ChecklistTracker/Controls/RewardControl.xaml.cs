using ChecklistTracker.Controls.Click;
using ChecklistTracker.Layout;
using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace ChecklistTracker.Controls
{
    public sealed partial class RewardControl : UserControl
    {
        internal LayoutParams Layout { get; private set; }

        internal RewardViewModel ViewModel { get; private set; }

        internal ITextStyle TextStyle { get; private set; }

        internal RewardControl(RewardViewModel viewModel, LayoutParams layout, ITextStyle textStyle)
        {
            InitializeComponent();

            ViewModel = viewModel;
            Layout = layout;
            TextStyle = textStyle;

            var callbacks = new ClickCallbacks();
            callbacks.OnClick = ViewModel.OnClick;
            callbacks.OnScroll = ViewModel.OnScroll;

            this.ConfigureClickHandler(callbacks);
        }
    }
}
