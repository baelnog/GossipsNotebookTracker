using ChecklistTracker.Config;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

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

        internal RewardViewModel(Item item, CheckListViewModel viewModel, string labelSet, int startingIndex)
            : base(item, viewModel)
        {
            LabelSet = labelSet;
            LabelIndex = startingIndex;
            _Label = ResourceFinder.GetLabel(labelSet, startingIndex) ?? "none";
        }

        internal override void OnScroll(UIElement sender, int scrollAmount)
        {
            LabelIndex = ResourceFinder.BoundLabelIndex(LabelSet, LabelIndex + scrollAmount);
            Label = ResourceFinder.GetLabel(LabelSet, LabelIndex) ?? "none";
        }
    }
}
