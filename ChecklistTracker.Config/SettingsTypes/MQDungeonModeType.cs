using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum MQDungeonModeType
{
    [EnumMember(Value = "vanilla")]
    Vanilla,
    [EnumMember(Value = "mq")]
    MasterQuest,
    [EnumMember(Value = "specific")]
    Specific,
    [EnumMember(Value = "count")]
    FixedCount,
    [EnumMember(Value = "random")]
    RandomCount,
}
