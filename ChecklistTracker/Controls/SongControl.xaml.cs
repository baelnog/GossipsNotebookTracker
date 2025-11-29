using ChecklistTracker.Controls.Click;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace ChecklistTracker.Controls;

public partial class SongControl : UserControl, INotifyPropertyChanged
{
    public double BottomImageWidth { get; set; }
    public double BottomImageHeight { get; set; }
    public Thickness BottomImageMargin { get; set; }

    internal LayoutParams Layout { get; private set; }
    internal SongViewModel ViewModel { get; private set; }

    internal SongControl(SongViewModel viewModel, LayoutParams layout)
    {
        InitializeComponent();
        ViewModel = viewModel;
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;

        Layout = layout;

        var bottomScale = 0.6;
        BottomImageWidth = bottomScale * Layout.Width;
        BottomImageHeight = bottomScale * Layout.Height;

        BottomImageMargin = new Thickness(0, 0, 0, -0.6 * BottomImageWidth);

        var callbacks = new ClickCallbacks();
        callbacks.OnClick = ViewModel.OnClick;
        callbacks.OnScroll = ViewModel.OnScroll;
        callbacks.DragImageProvider = ViewModel;
        callbacks.DropImageProvider = ViewModel;

        Image.ConfigureClickHandler(callbacks);

        var smallImageCallbacks = new ClickCallbacks();
        smallImageCallbacks.OnClick = ViewModel.OnSmallClick;
        smallImageCallbacks.DropImageProvider = ViewModel;
        Image2.ConfigureClickHandler(smallImageCallbacks);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        this.RaisePropertyChanged(PropertyChanged, e.PropertyName);
    }
}