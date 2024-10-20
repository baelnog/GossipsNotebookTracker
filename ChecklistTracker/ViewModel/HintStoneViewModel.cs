using ChecklistTracker.Controls.Click;
using ChecklistTracker.CoreUtils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChecklistTracker.ViewModel
{
    internal class HintStoneViewModel : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler? PropertyChanged;

        public ImageSource CurrentImage { get; set; }

        private string ImageGroup;

        private int Index { get; set; }

        internal CheckListViewModel ViewModel { get; private set; }

        internal HintStoneViewModel(CheckListViewModel viewModel, string elementId, ImageSource? startingImage = null)
        {
            ViewModel = viewModel;
            ImageGroup = ResourceFinder.FindItemById(elementId) ?? elementId;
            Index = 0;
            CurrentImage = startingImage ?? ResourceFinder.FindImageGroupImage(ImageGroup, Index);

            this.OnPropertyChanged(nameof(Index), NotifyIndexChanged);
        }

        protected void RaisePropertyChanged([CallerMemberName] string? name = null)
        {
            this.RaisePropertyChanged(PropertyChanged, name);
        }

        private void NotifyIndexChanged(object? arg1, PropertyChangedEventArgs args)
        {
            UpdateCurrentImage();
        }

        private void UpdateCurrentImage()
        {
            CurrentImage = ResourceFinder.FindImageGroupImage(ImageGroup, Index);
        }

        public void Collect(int n = 1)
        {

            var maxLength = ResourceFinder.GetImageSet(ImageGroup)!.Count;

            Index = Math.Clamp(Index + n, 0, maxLength);
            if (Index == 0)
            {
                // If the current image was created from a drag/drop, we need to manually trigger an image update.
                // The image may already have been 0.
                UpdateCurrentImage();
            }
        }

        internal void OnClick(UIElement sender, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    Collect();
                    break;
                case MouseButton.Right:
                    Collect(-1);
                    break;
                default:
                    return;
            }
        }

        internal ImageSource OnDrag(UIElement sender, MouseButton button)
        {
            var rc = CurrentImage;

            if (button == MouseButton.Right)
            {
                Index = 0;
            }

            return rc;
        }

        internal void OnDrop(UIElement sender, MouseButton button, ImageSource image)
        {
            CurrentImage = image;
        }

        internal virtual void OnScroll(UIElement sender, int scrollAmount)
        {
            Collect(scrollAmount);
        }
    }
}
