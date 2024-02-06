using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
internal enum HintRequirementType
{
    [EnumMember(Value = "none")]
    NoHints,
    [EnumMember(Value = "mask")]
    MaskOfTruth,
    [EnumMember(Value = "agony")]
    StoneOfAgony,
    [EnumMember(Value = "always")]
    Free,
}
