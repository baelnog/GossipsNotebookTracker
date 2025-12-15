using ChecklistTracker.Config;
using ChecklistTracker.Controls.Click;
using ChecklistTracker.CoreUtils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace ChecklistTracker.ViewModel
{
    internal class SongViewModel : ItemViewModel, IDropProvider<ImageSource>
    {
        public ImageSource BottomImage { get; set; }

        private CircularQueue<string> QuickFillImages { get; set; }

        internal SongViewModel(Item item, CheckListViewModel viewModel, CircularQueue<string> quickFillImages) : base(item, viewModel)
        {
            BottomImage = ResourceFinder.FindItem("song", 0)!;
            QuickFillImages = quickFillImages;
        }

        internal void OnSmallClick(UIElement sender, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    BottomImage = ResourceFinder.FindItem("song", 1)!;
                    break;
                case MouseButton.Middle:
                    if (QuickFillImages.Any())
                    {
                        BottomImage = ResourceFinder.FindItem(QuickFillImages.Next(), 1);
                    }
                    break;
                case MouseButton.Right:
                    BottomImage = ResourceFinder.FindItem("song", 0)!;
                    break;
                default:
                    return;
            }
        }

        internal override void OnClick(UIElement sender, MouseButton button)
        {
            if (button == MouseButton.Middle)
            {
                if (QuickFillImages.Any())
                {
                    BottomImage = ResourceFinder.FindItem(QuickFillImages.Next(), 1);
                }
                return;
            }
            base.OnClick(sender, button);
        }

        internal void OnDropImage(UIElement sender, MouseButton button, ImageSource image)
        {
            BottomImage = image;
        }

        public void OnDataDroppedTo(ImageSource data)
        {
            BottomImage = data;
        }
    }
}
