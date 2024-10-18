using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        private void OnClickCaptureScreenshot(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.CaptureScreenshot();
        }
    }
}