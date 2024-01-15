using ChecklistTracker.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ChecklistTracker.ViewModel
{
    public class HintViewModel
    {
        internal bool IsEntry { get; set; }
        public string Text { get; set; }
        internal string? LabelSet { get; set; }
        internal List<Config.Label>? BaseLabelSet { get; set; }

        internal ObservableCollection<HintStoneViewModel> LeftStones { get; private set; }
        internal ObservableCollection<HintStoneViewModel> RightStones { get; private set; }

        internal HintViewModel(
            CheckListViewModel viewModel,
            int leftItems = 0, int rightItems = 0,
            string? leftIconSet = "bosses", string? rightIconSet = "sometimes",
            string? labelSet = null, string text = "", bool isEntry = false)
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

            if (isEntry)
            {
                if (labelSet != null)
                {
                    BaseLabelSet = ResourceFinder.GetLabels(labelSet);
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
        }
    }
}
