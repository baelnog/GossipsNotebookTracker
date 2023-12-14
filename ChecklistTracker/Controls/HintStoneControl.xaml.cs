using ChecklistTracker.Config;
using ChecklistTracker.Controls.Click;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;

namespace ChecklistTracker.Controls
{
    public sealed partial class HintStoneControl : UserControl
    {
        private ImageSource _currentImage;
        public ImageSource CurrentImage { get => _currentImage; private set { _currentImage = value; Bindings.Update(); } }

        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        private string ImageGroupName;
        private int ImageGroupIndex = 0;

        public HintStoneControl(int width, int height, string elementId, Thickness padding, ImageSource startingImage = null)
        {
            InitializeComponent();
            ImageWidth = width; ImageHeight = height;
            Margin = padding;

            ImageGroupName = ResourceFinder.FindItemById(elementId) ?? elementId;
            CurrentImage = startingImage ?? ResourceFinder.FindImageGroupImage(ImageGroupName, ImageGroupIndex);

            var callbacks = new ClickCallbacks();
            callbacks.OnClick = OnClick;
            callbacks.OnScroll = OnScroll;
            callbacks.OnDragImageCompleted = OnDragImageCompleted;
            callbacks.OnDropImageCompleted = OnDropImageCompleted;

            this.ConfigureClickHandler(callbacks);

            Bindings.Update();
        }

        internal void OnClick(UIElement sender, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    ImageGroupIndex = Math.Min(ImageGroupIndex + 1, ResourceFinder.GetImageSet(ImageGroupName).Count - 1);
                    break;
                case MouseButton.Right:
                    ImageGroupIndex = Math.Max(ImageGroupIndex - 1, 0);
                    break;
                default:
                    return;
            }

            CurrentImage = ResourceFinder.FindImageGroupImage(ImageGroupName, ImageGroupIndex);
        }

        internal ImageSource OnDragImageCompleted(UIElement sender, MouseButton button)
        {

            var rc = CurrentImage;

            if (button == MouseButton.Right)
            {
                CurrentImage = ResourceFinder.FindItem("sometimes", 0);
            }

            return rc;
        }

        internal void OnDropImageCompleted(UIElement sender, MouseButton button, ImageSource image)
        {
            CurrentImage = image;
        }

        internal void OnScroll(UIElement sender, int scrollAmount)
        {
            for (int i = 0; i  < Math.Abs(scrollAmount); i++)
            {
                OnClick(sender, scrollAmount < 0 ? MouseButton.Right : MouseButton.Left);
            }
        }
    }
}
