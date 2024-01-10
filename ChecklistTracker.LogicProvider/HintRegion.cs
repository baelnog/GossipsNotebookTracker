using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistTracker.LogicProvider
{
    public class HintRegion : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        private string _Name;
        public string Name
        {
            get => _Name;
            private set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _ShortName;
        public string ShortName
        {
            get => _ShortName;
            private set
            {
                if (_ShortName != value)
                {
                    _ShortName = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _IsActive = false;
        public bool IsActive
        {
            get => _IsActive;
            set
            {
                if (_IsActive != value)
                {
                    _IsActive = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<LocationInfo> Locations { get; private set; }

        internal ILookup<string, LocationInfo> LocationsByName { get; private set; }

        internal HintRegion(string name, string? shortName)
        {
            _Name = name;
            _ShortName = shortName ?? Name;
            Locations = [];
            LocationsByName = Locations.ToLookup(loc => loc.Name);
        }

        public override bool Equals(object? obj)
        {
            return obj is HintRegion other && other.Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
