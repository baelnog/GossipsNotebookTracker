using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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
