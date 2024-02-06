using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum PrecompletedDungeonChoiceType
{
    [EnumMember(Value = "none")]
    None,
    [EnumMember(Value = "specific")]
    Specific,
    [EnumMember(Value = "count")]
    Count,
}
