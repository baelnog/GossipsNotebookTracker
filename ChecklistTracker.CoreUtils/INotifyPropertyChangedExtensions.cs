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
            handler?.Invoke(obj, new PropertyChangedEventArgs(name));
        }
    }
}
