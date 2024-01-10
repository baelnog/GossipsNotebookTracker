using ChecklistTracker.Config;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.LogicProvider;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistTracker
{
    internal class Inventory : INotifyPropertyChanged
    {

        private Dictionary<Item, int> ItemCounts;
        private ISet<string> CheckedLocations = new HashSet<string>();

        private Stack<(Action undo, Action redo)> UndoActions = new Stack<(Action undo, Action redo)>();
        private Stack<(Action undo, Action redo)> RedoActions = new Stack<(Action undo, Action redo)>();

        private LogicEngine LogicEngine;

        public event PropertyChangedEventHandler? PropertyChanged;

        internal Inventory(LogicEngine engine)
        {
            ItemCounts = ResourceFinder.GetItems().ToDictionary(item => item, item => 0);
            LogicEngine = engine;
        }

        public void InitFromLogic()
        {
            if (LogicEngine != null)
            {
                foreach (var item in ItemCounts.Keys)
                {
                    if (item.logic_name != null)
                    {
                        ItemCounts[item] = LogicEngine.Inventory[item.logic_name];
                    }
                }
            }
        }

        public void Undo()
        {
            if (UndoActions.TryPop(out var result))
            {
                RedoActions.Push(result);
                result.undo.Invoke();
            }
        }

        public void Redo()
        {
            if (RedoActions.TryPop(out var result))
            {
                UndoActions.Push(result);
                result.redo.Invoke();
            }
        }

        public void PushAction(Action action, Action undoAction)
        {
            UndoActions.Push((undoAction, action));
            RedoActions.Clear();
            action.Invoke();
        }

        public bool IsLocationChecked(string locationName)
        {
            return CheckedLocations.Contains(locationName);
        }

        public void CheckLocation(LocationInfo location)
        {
            var isChecked = location.IsChecked;
            PushAction(() => location.IsChecked = !isChecked, () => location.IsChecked = isChecked);
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
            var oldValue = ItemCounts[item];
            var newValue = oldValue + amount;
            if (item.collection == CollectionType.Count)
            {
                newValue = Math.Min(newValue, item.max_count ?? int.MaxValue);
            }
            else
            {
                newValue = Math.Min(newValue, item.images.Length - 1);
            }
            newValue = Math.Max(newValue, 0);

            var set = (int value) =>
            {
                ItemCounts[item] = value;
                if (item.logic_name != null)
                {
                    LogicEngine.Inventory[item.logic_name] = value;
                    LogicEngine.UpdateItems(LogicEngine.Inventory);
                    this.RaisePropertyChanged(PropertyChanged, item.logic_name);
                }
            };

            PushAction(() => set(newValue), () => set(oldValue));
        }

    }
}
