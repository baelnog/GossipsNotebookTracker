using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
