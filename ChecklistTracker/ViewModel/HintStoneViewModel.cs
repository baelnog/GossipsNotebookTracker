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

        private ImageSource _CurrentImage;
        public ImageSource CurrentImage
        {
            get => _CurrentImage;
            internal set
            {
                if (value != _CurrentImage)
                {
                    _CurrentImage = value;
                    this.RaisePropertyChanged(PropertyChanged);
                }
            }
        }

        private string ImageGroup;

        private int _Index;
        private int Index
        {
            get => _Index;
            set
            {
                if (value != _Index)
                {
                    _Index = value;
                    this.RaisePropertyChanged(PropertyChanged);
                }
            }
        }

        internal CheckListViewModel ViewModel { get; private set; }

        internal HintStoneViewModel(CheckListViewModel viewModel, string elementId, ImageSource? startingImage = null)
        {
            ViewModel = viewModel;
            ImageGroup = ResourceFinder.FindItemById(elementId) ?? elementId;
            _Index = 0;
            _CurrentImage = startingImage ?? ResourceFinder.FindImageGroupImage(ImageGroup, Index);

            this.OnPropertyChanged(nameof(Index), OnIndexChanged);
        }

        protected void RaisePropertyChanged([CallerMemberName] string? name = null)
        {
            this.RaisePropertyChanged(PropertyChanged, name);
        }

        private void OnIndexChanged(object? arg1, PropertyChangedEventArgs args)
        {
            CurrentImage = ResourceFinder.FindImageGroupImage(ImageGroup, Index);
        }

        public void Collect(int n = 1)
        {

            var maxLength = ResourceFinder.GetImageSet(ImageGroup).Count;

            Index = Math.Clamp(Index + n, 0, maxLength);
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
