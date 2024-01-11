using ChecklistTracker.Config;
using ChecklistTracker.Controls.Click;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.ComponentModel;

namespace ChecklistTracker.Controls
{
    public sealed partial class ElementControl : UserControl, INotifyPropertyChanged
    {
        public Visibility CountVisibility { get { return ViewModel.HasCount ? Visibility.Visible: Visibility.Collapsed; } }

        internal ItemViewModel ViewModel;
        internal LayoutParams Layout { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        internal ElementControl(ItemViewModel viewModel, LayoutParams layout)
        {
            InitializeComponent();
            ViewModel = viewModel;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            Layout = layout;

            var callbacks = new ClickCallbacks();
            callbacks.OnClick = ViewModel.OnClick;
            callbacks.OnDragImageCompleted = ViewModel.OnDragImage;
            callbacks.OnScroll += ViewModel.OnScroll;

            this.Image.ConfigureClickHandler(callbacks);
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(PropertyChanged, e.PropertyName);
        }
    }
}
