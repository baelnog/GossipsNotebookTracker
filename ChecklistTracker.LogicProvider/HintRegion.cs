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

        public string Name { get; private set; }

        public string ShortName { get; private set; }

        public bool IsActive { get; set; }

        public ObservableCollection<LocationInfo> Locations { get; private set; }

        internal ILookup<string, LocationInfo> LocationsByName { get; private set; }

        internal HintRegion(string name, string? shortName)
        {
            Name = name;
            ShortName = shortName ?? Name;
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
    }
}
