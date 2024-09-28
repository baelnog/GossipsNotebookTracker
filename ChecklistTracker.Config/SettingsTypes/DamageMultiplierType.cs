using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
internal enum DamageMultiplierType
{
    [EnumMember(Value = "none")]
    None,
    [EnumMember(Value = "half")]
    Half,
    [EnumMember(Value = "normal")]
    Normal,
    [EnumMember(Value = "double")]
    Double,
    [EnumMember(Value = "quadruple")]
    Quadruple,
    [EnumMember(Value = "ohko")]
    OHKO,
}
