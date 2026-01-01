using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[Flags]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
internal enum ShuffleSongMelodyType
{
    [EnumMember(Value = "off")]
    Off = 0,
    [EnumMember(Value = "frog")]
    Frog = 1,
    [EnumMember(Value = "warp")]
    Warp = 2,
    [EnumMember(Value = "all")]
    All = Frog | Warp,
}
