using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[Flags]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
internal enum AgeType
{
    [EnumMember(Value = "child")]
    Child,
    [EnumMember(Value = "adult")]
    Adult,
}
