using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum OpenKakarikoType
{
    [EnumMember(Value = "open")]
    Open,
    [EnumMember(Value = "zelda")]
    AutoOpenOnZeldasLetter,
    [EnumMember(Value = "closed")]
    ClosedGate,
}
