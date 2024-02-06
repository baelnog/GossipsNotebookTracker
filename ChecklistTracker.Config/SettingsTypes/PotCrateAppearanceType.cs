using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
internal enum PotCrateAppearanceType
{
    [EnumMember(Value = "off")]
    Off,
    [EnumMember(Value = "textures_content")]
    MatchContents,
    [EnumMember(Value = "textures_unchecked")]
    MatchChecked,
}
