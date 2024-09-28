using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum StartingAgeType
{
    [EnumMember(Value = "child")]
    Child,
    [EnumMember(Value = "adult")]
    Adult,
    [EnumMember(Value = "random")]
    Random,
}
