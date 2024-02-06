using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Config.SettingsTypes;

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
