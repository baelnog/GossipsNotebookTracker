using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[Flags]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum BasicShuffleType
{
    [EnumMember(Value = "off")]
    Off = 0,
    [EnumMember(Value = "dungeons")]
    Dungeons = 1,
    [EnumMember(Value = "overworld")]
    Overworld = 2,
    [EnumMember(Value = "all")]
    All = Dungeons | Overworld,
}

public static class BasicShuffleTypeExtensions
{
    public static bool IsShuffled(this BasicShuffleType value, bool isDungeon)
    {
        return isDungeon ?
            value.HasFlag(BasicShuffleType.Dungeons) :
            value.HasFlag(BasicShuffleType.Overworld);
    }
}
