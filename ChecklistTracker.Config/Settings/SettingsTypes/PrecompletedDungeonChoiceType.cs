using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum PrecompletedDungeonChoiceType
{
    [EnumMember(Value = "none")]
    None,
    [EnumMember(Value = "specific")]
    Specific,
    [EnumMember(Value = "count")]
    Count,
}
