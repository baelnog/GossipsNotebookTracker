using ChecklistTracker.ViewModel;
using Microsoft.UI.Dispatching;
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
        }

        private void OnClickCaptureScreenshot(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.CaptureScreenshot();
        }
    }
}