using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ScrubShuffleType
{
    [EnumMember(Value = "off")]
    Off,
    [EnumMember(Value = "low")]
    LowPrice,
    [EnumMember(Value = "regular")]
    ExpensivePrice,
    [EnumMember(Value = "random")]
    RandomPrice,
}

public static class ScrubShuffleTypeExtensions
{
    public static bool IsEnabled(this ScrubShuffleType value)
    {
        return value != ScrubShuffleType.Off;
    }
}