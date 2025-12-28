using ChecklistTracker.Controls.Click;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.ViewModel;
using CommunityToolkit.WinUI.Collections;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ChecklistTracker.Controls
{
    public sealed partial class ScreenCaptureControl : UserControl
    {
        public ScreenCaptureViewModel ViewModel { get; private set; }

        public LayoutParams LayoutParams { get; private set; }

        internal ScreenCaptureControl(ScreenCaptureViewModel viewModel, LayoutParams layout)
        {
            InitializeComponent();

            ViewModel = viewModel;
            LayoutParams = layout;
            // Disable transitions
            Screenshots.ItemContainerTransitions = null;

            Screenshots.ConfigureClickHandler(new ClickCallbacks
            {
                OnItemClick = ViewModel.OnItemClick
            });
        }

        private void OnClickCaptureScreenshot(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.CaptureScreenshot();
        }
    }
}