using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum OpenForestType
{
    [EnumMember(Value = "open")]
    Open,
    [EnumMember(Value = "closed_deku")]
    ClosedDeku,
    [EnumMember(Value = "closed")]
    Closed,
}
