using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
internal enum IceTrapType
{
    [EnumMember(Value = "off")]
    Off,
    [EnumMember(Value = "normal")]
    Normal,
    [EnumMember(Value = "on")]
    On,
    [EnumMember(Value = "mayhem")]
    Mayhem,
    [EnumMember(Value = "onslaught")]
    Onslaught,
}
