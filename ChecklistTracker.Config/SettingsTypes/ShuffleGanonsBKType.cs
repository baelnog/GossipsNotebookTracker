using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ShuffleGanonsBKType
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
    [EnumMember(Value = "keysanity")]
    Anywhere,
    [EnumMember(Value = "on_lacs")]
    OnLACS,
    [EnumMember(Value = "stones")]
    OnStones,
    [EnumMember(Value = "medallions")]
    OnMedallions,
    [EnumMember(Value = "dungeons")]
    OnDungeons,
    [EnumMember(Value = "tokens")]
    OnTokens,
    [EnumMember(Value = "hearts")]
    OnHearts,
    [EnumMember(Value = "triforce")]
    OnTriforce,
}
