using ChecklistTracker.CoreUtils;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config
{
    public partial class UserConfig : INotifyPropertyChanged
    {
        internal static readonly string UserConfigFile = $"{TrackerConfig.ProgramDir}/user-config.json";

        [JsonPropertyName("showMenuBar")]
        public bool ShowMenuBar { get; set; } = true;

        [JsonPropertyName("layout")]
        public string LayoutPath { get; set; } = "layouts/season8.json";

        [JsonPropertyName("layouts")]
        public List<string> LayoutHistory { get; set; } = new List<string>
        {
            "layouts/season8.json",
            "layouts/season7.json",
            "layouts/season7-interiors.json",
            "layouts/screenshots.json",
            "layouts/screenshots-doors.json",
        };

        [JsonPropertyName("settings")]
        public string SettingsPath { get; set; } = "settings/season7-base.json";

        [JsonPropertyName("settingsPresets")]
        public List<string> SettingsPresets { get; set; } = new List<string>();

        [JsonInclude]
        public bool ShowLocationTracker { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void SetLayout(string layoutPath)
        {
            var trackerDir = new DirectoryInfo(TrackerConfig.ProgramDir);
            var layoutFile = new FileInfo(layoutPath);
            if (layoutFile.FullName.StartsWith(trackerDir.FullName))
            {
                layoutPath = Path.GetRelativePath(trackerDir.FullName, layoutFile.FullName);
            }
            if (!LayoutHistory.Contains(layoutPath))
            {
                LayoutHistory.Add(layoutPath);
            }
            else
            {
                LayoutHistory.Remove(layoutPath);
                LayoutHistory.Insert(0, layoutPath);
            }

            LayoutPath = layoutPath;
        }

        public void TriggerLayoutReload()
        {
            // Signal that the layout was changed to trigger a reload.
            this.RaisePropertyChanged(PropertyChanged, nameof(LayoutPath));
        }

        public void SetSettings(string settingsPath)
        {
            var trackerDir = new DirectoryInfo(TrackerConfig.ProgramDir);
            var settingsFile = new FileInfo(settingsPath);
            if (settingsFile.FullName.StartsWith(trackerDir.FullName))
            {
                settingsPath = Path.GetRelativePath(trackerDir.FullName, settingsFile.FullName);
            }
            if (!SettingsPresets.Contains(settingsPath))
            {
                SettingsPresets.Add(settingsPath);
            }

            SettingsPath = settingsPath;
        }
    }
}
