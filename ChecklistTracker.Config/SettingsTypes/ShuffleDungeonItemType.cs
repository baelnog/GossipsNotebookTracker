using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ShuffleDungeonItemType
{
    [EnumMember(Value = "remove")]
    Remove,
    [EnumMember(Value = "startwith")]
    StartWith,
    [EnumMember(Value = "vanilla")]
    Vanilla,
    [EnumMember(Value = "dungeon")]
    OwnDungeon,
    [EnumMember(Value = "regional")]
    Regional,
    [EnumMember(Value = "overworld")]
    Overworld,
    [EnumMember(Value = "any_dungeon")]
    AnyDungeon,
    [EnumMember(Value = "keysanity")]
    Anywhere,
}

public static class ShuffleDungeonItemTypeExtensions
{
    public static bool IsShuffled(this ShuffleDungeonItemType type)
    {
        return type != ShuffleDungeonItemType.OwnDungeon && type != ShuffleDungeonItemType.Vanilla;
    }
}