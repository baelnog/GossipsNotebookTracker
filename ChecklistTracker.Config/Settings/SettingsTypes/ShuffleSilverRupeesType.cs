using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ShuffleSilverRupeesType
{
    [EnumMember(Value = "remove")]
    Remove,
    [EnumMember(Value = "vanilla")]
    Vanilla,
    [EnumMember(Value = "dungeon")]
    OwnDungeon,
    [EnumMember(Value = "regional")]
    Regional,
    [EnumMember(Value = "overworld")]
    Overwold,
    [EnumMember(Value = "any_dungeon")]
    AnyDungeon,
    [EnumMember(Value = "anywhere")]
    Anywhere
}
