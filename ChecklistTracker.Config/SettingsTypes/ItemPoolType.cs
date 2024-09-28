using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
internal enum ItemPoolType
{
    [EnumMember(Value = "ludicrous")]
    Ludicrous,
    [EnumMember(Value = "plentiful")]
    Plentiful,
    [EnumMember(Value = "balanced")]
    Balanced,
    [EnumMember(Value = "scarce")]
    Scarce,
    [EnumMember(Value = "minimal")]
    Minimal,
}
