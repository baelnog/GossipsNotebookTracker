using ChecklistTracker.Config;
using ChecklistTracker.Controls.Click;
using ChecklistTracker.CoreUtils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ChecklistTracker.ViewModel
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler? PropertyChanged;

        public Item Item { get; private set; }

        public int Count { get; private set; }

        public bool HasCount { get => Item.collection == CollectionType.Count; }

        public ImageSource CurrentImage { get; private set; }

        public int? IndexOverride { get; set; }

        internal CheckListViewModel ViewModel { get; private set; }

        internal ItemViewModel(Item item, CheckListViewModel viewModel)
        {
            Item = item;
            ViewModel = viewModel;
            ViewModel.Inventory.OnPropertyChanged(Item.logic_name, NotifyItemChanged);
            UpdateImageAndCount();
            Contract.Assert(CurrentImage != null);
        }

        protected void RaisePropertyChanged([CallerMemberName] string? name = null)
        {
            this.RaisePropertyChanged(PropertyChanged, name);
        }

        private void UpdateImageAndCount()
        {
            if (IndexOverride.HasValue)
            {
                CurrentImage = ResourceFinder.FindItemImage(Item, IndexOverride.Value);
            }
            else
            {
                CurrentImage = ViewModel.Inventory.GetCurrentItemImage(Item);
            }

            Count = ViewModel.Inventory.GetCurrentItemCount(Item);
            Contract.Assert(CurrentImage != null);
        }

        private void NotifyItemChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateImageAndCount();
        }

        public void Collect(int n = 1)
        {
            ViewModel.Inventory.CollectAmount(Item, n);
        }

        public void Uncollect(int n = 1)
        {
            ViewModel.Inventory.CollectAmount(Item, -n);
        }

        internal void OnClick(UIElement sender, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    Collect();
                    break;
                case MouseButton.Right:
                    Uncollect();
                    break;
                default:
                    return;
            }
        }

        internal ImageSource OnDragImage(UIElement sender, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                Collect();
                return CurrentImage;
            }
            else
            {
                return ResourceFinder.FindItemImage(Item, 1);
            }
        }

        internal virtual void OnScroll(UIElement sender, int scrollAmount)
        {
            Collect(scrollAmount);
        }
    }
}
