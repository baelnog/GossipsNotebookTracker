using ChecklistTracker.Controls.Click;
using ChecklistTracker.CoreUtils;
using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChecklistTracker.ViewModel
{
    public class HintViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        internal bool IsEntry { get; set; }
        public string Text { get; set; }
        internal string? LabelSet { get; set; }
        internal ISet<string>? LabelsFilter { get; set; }
        internal List<Config.Label>? BaseLabelSet { get; set; }
        internal int? Counter { get; set; } = null;

        internal ObservableCollection<HintStoneViewModel> LeftStones { get; private set; }
        internal ObservableCollection<HintStoneViewModel> RightStones { get; private set; }

        internal HintViewModel(
            CheckListViewModel viewModel,
            int leftItems = 0, int rightItems = 0,
            string? leftIconSet = "bosses", string? rightIconSet = "sometimes",
            bool showCounter = false,
            string? labelSet = null, string[]? labelsFilter = null, string text = "", bool isEntry = false)
        {
            LeftStones = new ObservableCollection<HintStoneViewModel>();
            for (int i = 0; i < leftItems && leftIconSet != null; i++)
            {
                LeftStones.Add(new HintStoneViewModel(viewModel, leftIconSet));
            }
            RightStones = new ObservableCollection<HintStoneViewModel>();
            for (int i = 0; i < rightItems && rightIconSet != null; i++)
            {
                RightStones.Add(new HintStoneViewModel(viewModel, rightIconSet));
            }
            Text = text;
            IsEntry = isEntry;
            LabelSet = labelSet;
            LabelsFilter = labelsFilter != null ? new HashSet<string>(labelsFilter) : null;
            if (showCounter)
            {
                Counter = 0;
            }

            if (isEntry)
            {
                if (labelSet != null)
                {
                    var baseLabels = ResourceFinder.GetLabels(labelSet);
                    if (LabelsFilter != null)
                    {
                        baseLabels = baseLabels
                            .Where(label => LabelsFilter.Contains(label.name))
                            .ToList();
                    }
                    BaseLabelSet = baseLabels;
                }
            }
        }

        internal void AdoptFrom(HintViewModel other)
        {
            for (int i = 0; i < LeftStones.Count && i < other.LeftStones.Count; i++)
            {
                LeftStones[i].CurrentImage = other.LeftStones[i].CurrentImage;
            }
            for (int i = 0; i < RightStones.Count && i < other.RightStones.Count; i++)
            {
                RightStones[i].CurrentImage = other.RightStones[i].CurrentImage;
            }
            Text = other.Text;
            if (other.Counter.HasValue)
            {
                Counter = other.Counter;
            }
        }

        internal void OnClickCounter(UIElement sender, MouseButton button)
        {
            if (!Counter.HasValue)
            {
                return;
            }
            var change = 1;
            if (button == MouseButton.Right) { change = -1; }
            Counter = int.Clamp(Counter.Value + change, 0, int.MaxValue);
        }

        internal void OnScrollCounter(UIElement sender, int change)
        {
            if (!Counter.HasValue)
            {
                return;
            }
            Counter = int.Clamp(Counter.Value + change, 0, int.MaxValue);
        }

        public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.RaisePropertyChanged(PropertyChanged, propertyName);
        }
    }
}
