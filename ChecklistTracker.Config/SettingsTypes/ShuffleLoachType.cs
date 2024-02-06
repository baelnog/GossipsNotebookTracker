using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
