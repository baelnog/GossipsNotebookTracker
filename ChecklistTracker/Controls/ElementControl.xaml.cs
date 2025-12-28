using ChecklistTracker.Controls.Click;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.Layout;
using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace ChecklistTracker.Controls
{
    public sealed partial class ElementControl : UserControl, INotifyPropertyChanged
    {
        public Visibility CountVisibility { get { return ViewModel.HasCount ? Visibility.Visible : Visibility.Collapsed; } }

        internal ItemViewModel ViewModel;
        internal LayoutParams Layout { get; private set; }
        internal ITextStyle TextStyle { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        internal ElementControl(ItemViewModel viewModel, LayoutParams layout, ITextStyle textStyle)
        {
            InitializeComponent();
            ViewModel = viewModel;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            Layout = layout;
            TextStyle = textStyle;

            var callbacks = new ClickCallbacks();
            callbacks.OnClick = ViewModel.OnClick;
            callbacks.OnScroll += ViewModel.OnScroll;
            callbacks.DragImageProvider = ViewModel;

            this.ConfigureClickHandler(callbacks);
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(PropertyChanged, e.PropertyName);
        }
    }
}
