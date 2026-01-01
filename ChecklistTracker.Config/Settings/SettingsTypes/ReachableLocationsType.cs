using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ReachableLocationsType
{
    [EnumMember(Value = "all")]
    AllLocations,
    [EnumMember(Value = "all_goals")]
    AllGoals,
    [EnumMember(Value = "beatable")]
    BeatableOnly
}
