using ChecklistTracker.Controls.Click;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace ChecklistTracker.Controls
{
    internal partial class HintStoneControl : UserControl, INotifyPropertyChanged
    {

#pragma warning disable 67
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 67

        internal LayoutParams? LayoutParams { get; set; }

        internal HintStoneViewModel? ViewModel { get; set; }

        internal HintStoneControl()
        {
            InitializeComponent();

            this.OnPropertyChanged(nameof(ViewModel), HandleViewModelChanged);
        }

        internal HintStoneControl(HintStoneViewModel viewModel, LayoutParams layout) : this()
        {
            ViewModel = viewModel;
            LayoutParams = layout;
        }

        private void HandleViewModelChanged(object? sender, PropertyChangedEventArgs e)
        {
            InitClickCallbacks();
        }

        private void InitClickCallbacks()
        {
            Contract.Assert(ViewModel != null);

            var callbacks = new ClickCallbacks();
            callbacks.OnClick = ViewModel.OnClick;
            callbacks.OnScroll = ViewModel.OnScroll;
            callbacks.DragImageProvider = ViewModel;
            callbacks.DropImageProvider = ViewModel;

            this.ConfigureClickHandler(callbacks);
        }
    }
}
