using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
internal enum ShuffleBossEntrancesType
{
    [EnumMember(Value = "off")]
    Off,
    [EnumMember(Value = "limited")]
    AgeRestricted,
    [EnumMember(Value = "full")]
    All,
}
