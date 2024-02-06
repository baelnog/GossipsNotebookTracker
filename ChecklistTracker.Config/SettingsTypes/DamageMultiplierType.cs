using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
internal enum DamageMultiplierType
{
    [EnumMember(Value = "none")]
    None,
    [EnumMember(Value = "half")]
    Half,
    [EnumMember(Value = "normal")]
    Normal,
    [EnumMember(Value = "double")]
    Double,
    [EnumMember(Value = "quadruple")]
    Quadruple,
    [EnumMember(Value = "ohko")]
    OHKO,
}
