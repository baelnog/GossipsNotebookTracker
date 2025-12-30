using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChecklistTracker.CoreUtils
{
    public static class INotifyPropertyChangedExtensions
    {

        public static void OnPropertyChanged(this INotifyPropertyChanged obj, string propertyName, Action<object?, PropertyChangedEventArgs> handler)
        {
            obj.PropertyChanged += (object? sender, PropertyChangedEventArgs args) =>
            {
                if (args.PropertyName == propertyName)
                {
                    handler(sender, args);
                }
            };
        }

        public static void RaisePropertyChanged(this INotifyPropertyChanged obj, PropertyChangedEventHandler? handler, [CallerMemberName] string? name = null)
        {
            try
            {
                Logging.WriteLine($"Rasing property changed for {obj} property {name}");
                handler?.Invoke(obj, new PropertyChangedEventArgs(name));
            }
            catch (Exception e)
            {
                Logging.WriteLine($"Failed to raise property changed for {obj} property {name}", e);
            }
        }
    }
}
