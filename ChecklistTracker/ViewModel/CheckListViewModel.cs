using ChecklistTracker.CoreUtils;
using ChecklistTracker.LogicProvider;
using CommunityToolkit.WinUI.UI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ChecklistTracker.ViewModel
{
    internal class CheckListViewModel : INotifyPropertyChanged
    {
        internal static CheckListViewModel? GlobalInstance { get; set; }

        internal Inventory Inventory { get; private set; }
        internal LogicEngine Engine { get; private set; }

        internal ObservableCollection<HintRegionViewModel> HintRegions { get; private set; }
        internal View.AdvancedCollectionView PaneRegions { get; private set; }
        internal View.AdvancedCollectionView ViewRegions { get; private set; }

        internal ObservableCollection<string> CheckedLocations = new ObservableCollection<string>();

        internal bool SkullsToggle { get; private set; } = false;

        internal CheckListViewModel(Inventory inventory, LogicEngine engine)
        {
            Inventory = inventory;
            Engine = engine;
            HintRegions = new ObservableCollection<HintRegionViewModel>(Engine.GetRegions().Select(hr => new HintRegionViewModel(this, hr)));

            PaneRegions = new View.AdvancedCollectionView(HintRegions, isLiveShaping: true);
            PaneRegions.SortDescriptions.Add(new SortDescription(SortDirection.Ascending, new FuncComparer(SortRegions)));
            PaneRegions.Filter = RegionPaneFilter;
            PaneRegions.ObserveFilterProperty(nameof(HintRegionViewModel.AnyLocations));
            PaneRegions.ObserveFilterProperty(nameof(HintRegionViewModel.Region));
            RegisterFilterCallbacks(PaneRegions);

            ViewRegions = new View.AdvancedCollectionView(HintRegions, isLiveShaping: true);
            ViewRegions.SortDescriptions.Add(new SortDescription(SortDirection.Ascending, new FuncComparer(SortRegions)));
            ViewRegions.Filter = RegionViewFilter;
            ViewRegions.ObserveFilterProperty(nameof(HintRegionViewModel.AnyLocations));
            ViewRegions.ObserveFilterProperty(nameof(HintRegionViewModel.Region));
            RegisterFilterCallbacks(ViewRegions);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private int SortRegions(object? x, object? y)
        {
            if (x is HintRegionViewModel regionX && y is HintRegionViewModel regionY)
            {
                return regionX.Region.Name.CompareTo(regionY.Region.Name);
            }
            return 0;
        }

        internal bool RegionPaneFilter(object o)
        {
            if (o is HintRegionViewModel region)
            {
                return IsPaneRegionVisible(region);
            }
            else
            {
                throw new ArgumentException(o?.ToString());
            }
        }

        internal bool RegionViewFilter(object o)
        {
            if (o is HintRegionViewModel region)
            {
                return IsViewRegionVisible(region);
            }
            else
            {
                throw new ArgumentException(o?.ToString());
            }
        }

        internal bool LocationFilter(object o)
        {
            if (o is LocationViewModel location)
            {
                return IsLocationVisible(location.Location);
            }
            else
            {
                throw new ArgumentException(o?.ToString());
            }
        }

        internal bool IsLocationVisible(LocationInfo location)
        {
            if (!location.IsActive)
            {
                return false;
            }
            if (!location.IsAccessible)
            {
                return false;
            }
            if (location.IsChecked)
            {
                return false;
            }
            if (location.IsSkull && SkullsToggle)
            {
                return true;
            }
            return location.IsProgress;
        }

        internal bool IsPaneRegionVisible(HintRegionViewModel region)
        {
            if (!region.Region.IsActive)
            {
                return false;
            }
            if (!region.AnyLocations)
            {
                return false;
            }
            return true;
        }

        internal bool IsViewRegionVisible(HintRegionViewModel region)
        {
            if (_SelectedRegion != null && _SelectedRegion != region.Region)
            {
                return false;
            }
            return IsPaneRegionVisible(region);
        }

        private HintRegion? _SelectedRegion;
        internal HintRegion? SelectedRegion
        {
            get => _SelectedRegion;
            set
            {
                _SelectedRegion = value;
                OnFiltersChanged();
                this.RaisePropertyChanged(PropertyChanged);
            }
        }

        public void Undo()
        {
            Inventory.Undo();
        }

        public void Redo()
        {
            Inventory.Redo();
        }

        internal void SelectRegion(HintRegion? region)
        {
            SelectedRegion = region;
        }

        internal void CheckLocation(LocationInfo location)
        {
            Inventory.CheckLocation(location);
        }

        internal bool IsLocationChecked(string location)
        {
            return Inventory.IsLocationChecked(location);
        }

        internal void ToggleSkulls(bool isChecked)
        {
            if (SkullsToggle != isChecked)
            {
                SkullsToggle = isChecked;
                OnFiltersChanged();
            }
        }

        private void OnFiltersChanged()
        {
            this.RaisePropertyChanged(PropertyChanged, "ViewModelFilters");
        }

        internal void RegisterFilterCallbacks(View.AdvancedCollectionView collection)
        {
            collection.ObserveFilterProperty("ViewModelFilters");
        }
    }
}
