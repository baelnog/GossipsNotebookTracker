using ChecklistTracker.Config.SettingsTypes;
using System.Text.Json;

namespace ChecklistTracker.Config
{
    public static class SettingsExtensions
    {
        public static bool SkipChildZelda(this Settings settings)
        {
            return settings.ChildTradeItemStart.Contains(ChildTradeItem.ZeldasLetter) ||
                   settings.StartingItems.TryGetValue("Zeldas Letter", out var letterCount) && letterCount > 0;
        }

        public static EnumType? ToEnum<EnumType>(this string enumString)
        {
            return JsonSerializer.Deserialize<EnumType>($"\"{enumString}\"");
        }

        public static bool IsNight(this string timeOfDay)
        {
            return timeOfDay == "sunset" ||
                   timeOfDay == "event" ||
                   timeOfDay == "midnight" ||
                   timeOfDay == "witching-hour";
        }

        public static bool IsNight(this TimeOfDay timeOfDay)
        {
            return timeOfDay == TimeOfDay.Sunset ||
                   timeOfDay == TimeOfDay.Evening ||
                   timeOfDay == TimeOfDay.Midnight ||
                   timeOfDay == TimeOfDay.WitchingHour;
        }
    }
}
