using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChecklistTracker.LogicProvider.DataFiles.Settings
{
    internal static class SettingsExtensions
    {
        public static bool DungeonItemShuffleEnabled(this string type)
        {
            return type != "dungeon" && type != "vanilla";
        }

        public static bool Enabled(this ShuffleTreasureChestGameKeysType type)
        {
            return type != ShuffleTreasureChestGameKeysType.Vanilla;
        }

        public static bool SkipChildZelda(this Settings settings)
        {
            return settings.ChildTradeEarliestItem.Contains("Zeldas Letter") ||
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
