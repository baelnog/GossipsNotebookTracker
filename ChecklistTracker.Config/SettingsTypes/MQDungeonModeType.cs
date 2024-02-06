using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
