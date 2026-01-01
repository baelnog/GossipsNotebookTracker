using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ShuffleHideoutKeysType
{
    [EnumMember(Value = "vanilla")]
    Vanilla,
    [EnumMember(Value = "fortress")]
    Fortress,
    [EnumMember(Value = "regional")]
    Regional,
    [EnumMember(Value = "overworld")]
    Overworld,
    [EnumMember(Value = "any_dungeon")]
    AnyDungeon,
    [EnumMember(Value = "keysanity")]
    Anywhere,
}

public static class ShuffleHideoutKeysTypeExtensions
{
    public static bool IsShuffled(this ShuffleHideoutKeysType type)
    {
        return type != ShuffleHideoutKeysType.Vanilla;
    }
}