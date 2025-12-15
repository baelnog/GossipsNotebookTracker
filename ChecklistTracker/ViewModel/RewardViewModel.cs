using ChecklistTracker.Config;
using ChecklistTracker.Controls.Click;
using ChecklistTracker.CoreUtils;
using Microsoft.UI.Xaml;

namespace ChecklistTracker.ViewModel
{
    internal class RewardViewModel : ItemViewModel
    {
        private string _Label;
        internal string Label
        {
            get => _Label;
            set
            {
                if (_Label != value)
                {
                    _Label = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string LabelSet;
        private int LabelIndex;

        private CircularQueue<string> QuickFillLabels { get; set; }

        internal RewardViewModel(Item item, CheckListViewModel viewModel, string labelSet, int startingIndex, CircularQueue<string> quickFillLabels)
            : base(item, viewModel)
        {
            LabelSet = labelSet;
            LabelIndex = startingIndex;
            _Label = ResourceFinder.GetLabel(labelSet, startingIndex) ?? "none";
            QuickFillLabels = quickFillLabels;
        }

        internal override void OnScroll(UIElement sender, int scrollAmount)
        {
            LabelIndex = ResourceFinder.BoundLabelIndex(LabelSet, LabelIndex + scrollAmount);
            Label = ResourceFinder.GetLabel(LabelSet, LabelIndex) ?? "none";
        }

        internal override void OnClick(UIElement sender, MouseButton button)
        {
            if (button == MouseButton.Middle)
            {
                if (QuickFillLabels.Any())
                {
                    Label = QuickFillLabels.Next();
                }
                return;
            }
            base.OnClick(sender, button);
        }
    }
}
