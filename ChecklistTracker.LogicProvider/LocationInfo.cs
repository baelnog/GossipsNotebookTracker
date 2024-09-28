using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChecklistTracker.LogicProvider
{
    public partial class LocationInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name { get; private set; }

        public string ShortName { get; private set; }

        public bool IsActive { get; internal set; }

        //public bool IsAccessible { get => Accessiblity.HasFlag(Accessibility.SyntheticAssumed); }

        public Accessibility Accessiblity { get; internal set; }

        public bool IsProgress { get; internal set; }

        public bool IsSkull { get; internal set; }

        public bool IsChecked { get; set; }

        internal LocationInfo(HintRegion parent, string name)
        {
            Name = name;
            ShortName = name;
            if (ShortName.StartsWith($"{parent.Name} "))
            {
                ShortName = ShortName.Substring(parent.Name.Length + 1);
            }
            if (ShortName.StartsWith($"{parent.ShortName} "))
            {
                ShortName = ShortName.Substring(parent.ShortName.Length + 1);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
