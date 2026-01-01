using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[Flags]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
internal enum ChestAppearanceType
{
    [EnumMember(Value = "off")]
    Off = 0,
    [EnumMember(Value = "classic")]
    Size = 1,
    [EnumMember(Value = "textures")]
    Textures = 2,
    [EnumMember(Value = "both")]
    Both = Size | Textures,
}
