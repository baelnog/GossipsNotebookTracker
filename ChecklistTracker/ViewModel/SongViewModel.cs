using ChecklistTracker.Config;
using ChecklistTracker.Controls.Click;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace ChecklistTracker.ViewModel
{
    internal class SongViewModel : ItemViewModel
    {
        private ImageSource _BottomImage;
        public ImageSource BottomImage
        {
            get => _BottomImage;
            set
            {
                if (value != _BottomImage)
                {
                    _BottomImage = value;
                    RaisePropertyChanged();
                }
            }
        }

        internal SongViewModel(Item item, CheckListViewModel viewModel) : base(item, viewModel)
        {
            _BottomImage = ResourceFinder.FindItem("song", 0);
        }

        internal void OnSmallClick(UIElement sender, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    BottomImage = ResourceFinder.FindItem("song", 1);
                    break;
                case MouseButton.Right:
                    BottomImage = ResourceFinder.FindItem("song", 0);
                    break;
                default:
                    return;
            }
        }

        internal void OnDropImage(UIElement sender, MouseButton button, ImageSource image)
        {
            BottomImage = image;
        }
    }
}
