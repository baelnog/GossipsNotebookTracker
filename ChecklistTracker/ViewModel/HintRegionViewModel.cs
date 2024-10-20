using ChecklistTracker.CoreUtils;
using ChecklistTracker.LogicProvider;
using CommunityToolkit.WinUI.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

//using AdvancedCollectionView = ChecklistTracker.View.AdvancedCollectionView;
using AdvancedCollectionView = CommunityToolkit.WinUI.Collections.AdvancedCollectionView;

namespace ChecklistTracker.ViewModel
{
    internal class HintRegionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public CheckListViewModel Model { get; private set; }
        public HintRegion Region { get; private set; }
        public AdvancedCollectionView Locations { get; private set; }

        public bool AnyLocations { get => Locations.Any(); }

        internal HintRegionViewModel(CheckListViewModel model, HintRegion region)
        {
            Model = model;
            Region = region;

            model.PropertyChanged += Model_PropertyChanged;

            var locationsCollection = new ObservableCollection<LocationViewModel>(region.Locations.Select(location => new LocationViewModel(model, location)));

            Locations = new AdvancedCollectionView(locationsCollection, isLiveShaping: true);
            Locations.Filter = Model.LocationFilter;
            Locations.SortDescriptions.Add(new SortDescription(SortDirection.Ascending, new FuncComparer(SortLocations)));
            model.RegisterFilterCallbacks(Locations);
            Locations.ObserveFilterProperty(nameof(LocationInfo.IsChecked));
            Locations.ObserveFilterProperty(nameof(LocationInfo.Accessiblity));
            Locations.ObserveFilterProperty(nameof(LocationInfo.IsSkull));
            Locations.ObserveFilterProperty(nameof(LocationInfo.IsActive));
            Locations.PropertyChanged += Locations_PropertyChanged;

            region.PropertyChanged += Region_PropertyChanged;

            foreach (var location in locationsCollection)
            {
                location.PropertyChanged += Location_PropertyChanged;
            }
        }

        private int SortLocations(object? x, object? y)
        {
            if (x is LocationViewModel locX && y is LocationViewModel locY)
            {
                return locX.Location.Name.CompareTo(locY.Location.Name);
            }
            return 0;
        }

        private void Location_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Locations.RefreshFilter();
            OnPropertyChanged(nameof(AnyLocations));
        }

        private void Locations_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
            OnPropertyChanged(nameof(AnyLocations));
        }

        private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        private void Region_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Region));
        }

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
