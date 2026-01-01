using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum WinConditionType
{
    [EnumMember(Value = "open")]
    Open,
    [EnumMember(Value = "vanilla")]
    Vanilla,
    [EnumMember(Value = "stones")]
    Stones,
    [EnumMember(Value = "medallions")]
    Medallions,
    [EnumMember(Value = "dungeons")]
    Dungeons,
    [EnumMember(Value = "tokens")]
    Tokens,
    [EnumMember(Value = "hearts")]
    Hearts,
    [EnumMember(Value = "random")]
    Random,
}
