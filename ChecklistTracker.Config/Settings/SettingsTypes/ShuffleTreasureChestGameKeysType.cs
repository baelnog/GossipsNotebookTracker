using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ShuffleTreasureChestGameKeysType
{
    [EnumMember(Value = "remove")]
    Remove,
    [EnumMember(Value = "vanilla")]
    Vanilla,
    [EnumMember(Value = "regional")]
    Regional,
    [EnumMember(Value = "overworld")]
    Overworld,
    [EnumMember(Value = "any_dungeon")]
    AnyDungeon,
    [EnumMember(Value = "keysanity")]
    Anywhere,
}

public static class ShuffleTreasureChestGameKeysTypeExtensions
{
    public static bool IsShuffled(this ShuffleTreasureChestGameKeysType value)
    {
        return value != ShuffleTreasureChestGameKeysType.Vanilla;
    }
}
