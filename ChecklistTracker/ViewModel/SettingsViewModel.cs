using ChecklistTracker.Config;
using ChecklistTracker.CoreUtils;
using System.ComponentModel;

namespace ChecklistTracker.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {

#pragma warning disable 67
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 67

        public Settings Settings { get; set; }

        internal SettingsViewModel(TrackerConfig trackerConfig)
        {
            Settings = trackerConfig.RandomizerSettings;

            Settings.PropertyChanged += Setting_Changed;

            trackerConfig.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(TrackerConfig.RandomizerSettings))
                {
                    var newSettings = trackerConfig.RandomizerSettings;
                    Settings = newSettings;
                    Settings.PropertyChanged += Setting_Changed;
                }
            };
        }

        private void Setting_Changed(object? sender, PropertyChangedEventArgs e)
        {
            Logging.WriteLine($"Changed {e.PropertyName}");
        }
    }
}
