using ChecklistTracker.Config;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistTracker
{
    internal class Inventory
    {

        private Dictionary<Item, int> ItemCounts;

        internal Inventory()
        {
            ItemCounts = ResourceFinder.GetItems().ToDictionary(item => item, item => 0);
        }

        public bool HasItem(Item item)
        {
            return ItemCounts[item] > 0;
        }

        public ImageSource GetCurrentItemImage(Item item)
        {
            return ResourceFinder.FindItemImage(item, ItemCounts[item]);
        }

        public int GetCurrentItemCount(Item item)
        {
            return ItemCounts[item];
        }

        public void CollectItem(Item item)
        {
            CollectAmount(item, 1);
        }

        public void UncollectItem(Item item)
        {
            CollectAmount(item, -1);
        }

        public void CollectAmount(Item item, int amount)
        {
            var newValue = ItemCounts[item] + amount;
            if (item.collection == CollectionType.Count)
            {
                newValue = Math.Min(newValue, item.max_count ?? int.MaxValue);
            }
            else
            {
                newValue = Math.Min(newValue, item.images.Length - 1);
            }
            newValue = Math.Max(newValue, 0);
            ItemCounts[item] = newValue;
        }

    }
}
