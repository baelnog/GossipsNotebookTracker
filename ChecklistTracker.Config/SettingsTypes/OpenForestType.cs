using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum OpenForestType
{
    [EnumMember(Value = "open")]
    Open,
    [EnumMember(Value = "closed_deku")]
    ClosedDeku,
    [EnumMember(Value = "closed")]
    Closed,
}
