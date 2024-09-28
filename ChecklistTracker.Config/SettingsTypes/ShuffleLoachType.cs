using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ShuffleLoachType
{
    [EnumMember(Value = "off")]
    Off,
    [EnumMember(Value = "vanilla")]
    Vanilla,
    [EnumMember(Value = "easy")]
    Easy,
}
