using ChecklistTracker.CoreUtils;
using ChecklistTracker.LogicProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ChecklistTracker.ViewModel
{
    internal class LocationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CheckListViewModel Model { get; private set; }
        public LocationInfo Location { get; private set; }

        internal LocationViewModel(CheckListViewModel model, LocationInfo location)
        {
            Model = model;
            Location = location;

            model.PropertyChanged += Model_PropertyChanged;
            Location.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Logging.WriteLine($"{Location.Name} {e.PropertyName}");
            OnPropertyChanged(e.PropertyName);
            //OnPropertyChanged(nameof(Location));
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override bool Equals(object o)
        {
            if (o is not LocationViewModel vm)
            {
                return false;
            }
            return Location.Name == vm.Location.Name;
        }

        public override int GetHashCode()
        {
            return Location.Name.GetHashCode();
        }
    }
}
