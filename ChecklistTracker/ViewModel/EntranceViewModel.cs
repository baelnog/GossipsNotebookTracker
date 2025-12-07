using ChecklistTracker.CoreUtils;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChecklistTracker.ViewModel
{
    public class EntranceViewModel : HintViewModel, INotifyPropertyChanged
    {
        internal int Index { get; private set; }
        internal HintStoneViewModel StoneViewModel { get; private set; }

        internal EntranceViewModel EntranceVM { get => this; }
        internal LayoutParams LayoutParams { get; private set; }
        internal LayoutParams IconLayoutParams { get; private set; }
        internal TextParams TextParams { get; private set; }

        internal EntranceViewModel(CheckListViewModel viewModel,
            string elementId, int initialIndex,
            string? labels, string[]? labelsFilter,
            LayoutParams layoutParams,
            LayoutParams iconLayoutParams,
            TextParams textParams)
            : base(viewModel, leftItems: 1, leftIconSet: elementId, labelSet: labels, labelsFilter: labelsFilter, isEntry: true)
        {
            Index = initialIndex;
            StoneViewModel = new HintStoneViewModel(viewModel, elementId);

            LayoutParams = layoutParams;
            IconLayoutParams = iconLayoutParams;
            TextParams = textParams;
        }
    }
}
