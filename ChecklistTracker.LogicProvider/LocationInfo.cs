using ChecklistTracker.CoreUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistTracker.LogicProvider
{
    public class LocationInfo : INotifyPropertyChanged
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

        public ObservableCollection<LocationInfo> Locations { get; private set; } = [];

        private bool _IsActive = false;
        public bool IsActive
        {
            get => _IsActive;
            internal set
            {
                if (_IsActive != value)
                {
                    _IsActive = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _IsAccessible = false;
        public bool IsAccessible
        {
            get => _IsAccessible;
            internal set
            {
                if (_IsAccessible != value)
                {
                    _IsAccessible = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _IsProgress = false;
        public bool IsProgress
        {
            get => _IsProgress;
            internal set
            {
                if (_IsProgress != value)
                {
                    _IsProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _IsSkull = false;
        public bool IsSkull
        {
            get => _IsSkull;
            internal set
            {
                if (_IsSkull != value)
                {
                    _IsSkull = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _IsChecked = false;
        public bool IsChecked
        {
            get => _IsChecked;
            set
            {
                if (_IsChecked != value)
                {
                    Logging.WriteLine($"{Name} {value}");
                    _IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        internal LocationInfo(HintRegion parent, string name)
        {
            _Name = name;
            _ShortName = name;
            if (_ShortName.StartsWith($"{parent.Name} "))
            {
                _ShortName = _ShortName.Substring(parent.Name.Length + 1);
            }
            if (ShortName.StartsWith($"{parent.ShortName} "))
            {
                _ShortName = _ShortName.Substring(parent.ShortName.Length + 1);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
