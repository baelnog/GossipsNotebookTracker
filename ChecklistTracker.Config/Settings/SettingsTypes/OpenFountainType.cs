using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum OpenFountainType
{
    [EnumMember(Value = "closed")]
    Closed,
    [EnumMember(Value = "adult")]
    OpenAdult,
    [EnumMember(Value = "open")]
    Open,
}
