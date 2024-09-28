using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[Flags]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
internal enum IceTrapAppearanceType
{
    [EnumMember(Value = "major_only")]
    MajorItems = 1,
    [EnumMember(Value = "junk_only")]
    JunkItems = 2,
    [EnumMember(Value = "anything")]
    Anything = MajorItems | JunkItems,
}
