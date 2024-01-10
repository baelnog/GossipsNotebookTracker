using ChecklistTracker.Config;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChecklistTracker.CoreUtils;

namespace ChecklistTracker.ViewModel
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Item Item { get; private set; }

        private int _Count;
        public int Count
        { 
            get => _Count;
            set 
            { 
                if (value != _Count) 
                { 
                    _Count = value;
                    this.RaisePropertyChanged(PropertyChanged);
                } 
            }
        }

        public bool HasCount { get => Item.collection == CollectionType.Count; }

        private ImageSource _CurrentImage;
        public ImageSource CurrentImage
        {
            get => _CurrentImage;
            set
            {
                if (value != _CurrentImage)
                {
                    _CurrentImage = value;
                    this.RaisePropertyChanged(PropertyChanged);
                }
            }
        }

        public int? IndexOverride { get; set; }

        public CheckListViewModel ViewModel { get; private set; }

        public ItemViewModel(Item item, CheckListViewModel viewModel)
        {
            Item = item;
            ViewModel = viewModel;
            ViewModel.Inventory.OnPropertyChanged(Item.logic_name, OnItemChanged);
            UpdateImageAndCount();
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
        }

        private void OnItemChanged(object? sender, PropertyChangedEventArgs e)
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
    }
}
