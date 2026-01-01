using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum TimeOfDay
{
    [EnumMember(Value = "default")]
    Default,
    [EnumMember(Value = "random")]
    Random,
    [EnumMember(Value = "sunrise")]
    Sunrise,
    [EnumMember(Value = "morning")]
    Morning,
    [EnumMember(Value = "noon")]
    Noon,
    [EnumMember(Value = "afternoon")]
    Afternoon,
    [EnumMember(Value = "sunset")]
    Sunset,
    [EnumMember(Value = "evening")]
    Evening,
    [EnumMember(Value = "midnight")]
    Midnight,
    [EnumMember(Value = "witching-hour")]
    WitchingHour,
}
