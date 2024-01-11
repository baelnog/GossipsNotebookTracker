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
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public int Count { get => ViewModel.Count; }
        public Visibility CountVisibility { get { return ViewModel.HasCount ? Visibility.Visible: Visibility.Collapsed; } }

        internal ItemViewModel ViewModel;

        public event PropertyChangedEventHandler? PropertyChanged;

        internal ElementControl(ItemViewModel viewModel, int width, int height, Thickness padding)
        {
            InitializeComponent();
            ViewModel = viewModel;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            ImageWidth = width; ImageHeight = height;
            Padding = padding;

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
