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
        public double ImageWidth { get; set; }
        public double ImageHeight { get; set; }

        internal RewardViewModel ViewModel { get; private set; }

        internal RewardControl(RewardViewModel viewModel, double width, double height, Thickness padding)
        {
            InitializeComponent();

            ViewModel = viewModel;

            var textSize = 10;
            Padding = padding;
            ImageWidth = width;
            ImageHeight = height;

            var littleY = height - textSize;

            var callbacks = new ClickCallbacks();
            callbacks.OnClick = ViewModel.OnClick;
            callbacks.OnScroll = ViewModel.OnScroll;

            this.ConfigureClickHandler(callbacks);
        }
    }
}
