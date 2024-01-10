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
        public ImageSource CurrentImage { get => ViewModel.CurrentImage; }
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
            callbacks.OnClick = OnClick;
            callbacks.OnDragImageCompleted = OnDragImage;
            callbacks.OnScroll += OnScroll;

            this.Image.ConfigureClickHandler(callbacks);
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(PropertyChanged, e.PropertyName);
        }

        void OnClick(UIElement sender, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    ViewModel.Collect();
                    break;
                case MouseButton.Right:
                    ViewModel.Uncollect();
                    break;
                default:
                    return;
            }
        }

        ImageSource OnDragImage(UIElement sender, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                ViewModel.Collect();
                return ViewModel.CurrentImage;
            }
            else
            {
                return ResourceFinder.FindItemImage(ViewModel.Item, 1);
            }
        }

        void OnScroll(UIElement sender, int scrollAmount)
        {
            ViewModel.Collect(scrollAmount);
        }
    }
}
