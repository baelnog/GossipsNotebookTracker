using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[Flags]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ShuffleEntranceType
{
    [EnumMember(Value = "off")]
    Off = 0,
    [EnumMember(Value = "simple")]
    Simple = 1,
    [EnumMember(Value = "special")]
    Special = 2, // Not a real setting
    [EnumMember(Value = "all")]
    All = Simple | Special,
}

public static class ShuffleEntranceTypeExtensions
{
    public static bool IsEnabled(this ShuffleEntranceType value)
    {
        return value != ShuffleEntranceType.Off;
    }
}