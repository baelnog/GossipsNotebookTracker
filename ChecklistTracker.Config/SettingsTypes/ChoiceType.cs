using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ChoiceType
{
    [EnumMember(Value = "off")]
    Off,
    [EnumMember(Value = "choice")]
    Choice,
    [EnumMember(Value = "all")]
    All,
    [EnumMember(Value = "random")]
    Random,
}
