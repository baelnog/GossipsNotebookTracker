using ChecklistTracker.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChecklistTracker.CoreUtils;
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml;

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
            string? labels,
            LayoutParams layoutParams,
            LayoutParams iconLayoutParams,
            TextParams textParams) 
            : base(viewModel, leftItems: 1, leftIconSet: elementId, labelSet: labels, isEntry: true)
        {
            Index = initialIndex;
            StoneViewModel = new HintStoneViewModel(viewModel, elementId);

            LayoutParams = layoutParams;
            IconLayoutParams = iconLayoutParams;
            TextParams = textParams;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        internal void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.RaisePropertyChanged(PropertyChanged, propertyName);
        }
    }
}
