using ChecklistTracker.CoreUtils;
using ChecklistTracker.Layout;
using CommunityToolkit.WinUI.Collections;
using System.Collections.ObjectModel;
using AdvancedCollectionView = CommunityToolkit.WinUI.Collections.AdvancedCollectionView;

namespace ChecklistTracker.ViewModel
{
    internal class EntranceTableViewModel
    {
        internal ObservableCollection<EntranceViewModel> HintStones = new ObservableCollection<EntranceViewModel>();

        internal AdvancedCollectionView SortedHintStones;
        internal LayoutParams LayoutParams;

        internal EntranceTableViewModel(
            CheckListViewModel viewModel,
            IList<string> icons,
            string? labels,
            string[]? labelsFilter,
            LayoutParams layoutParams,
            LayoutParams iconLayoutParams,
            ITextStyle textStyle)
        {

            for (int i = 0; i < icons.Count; i++)
            {
                HintStones.Add(new EntranceViewModel(viewModel, icons[i], i, labels, labelsFilter, layoutParams, iconLayoutParams, textStyle));
            }

            SortedHintStones = new AdvancedCollectionView(HintStones, isLiveShaping: true);

            SortedHintStones.SortDescriptions.Add(
                new SortDescription(SortDirection.Ascending, new FuncComparer(CompareStores)));

            LayoutParams = layoutParams;
        }

        int CompareStores(object? x, object? y)
        {
            if (x is EntranceViewModel vmX && y is EntranceViewModel vmY)
            {
                var xStr = vmX.Text ?? "";
                var yStr = vmY.Text ?? "";
                if (xStr == "" ^ yStr == "")
                {
                    return xStr == "" ? 1 : -1;
                }
                var comp = xStr.CompareTo(yStr);
                if (comp != 0)
                {
                    return comp;
                }
                return vmX.Index.CompareTo(vmY.Index);
            }
            return 0;
        }

    }
}
