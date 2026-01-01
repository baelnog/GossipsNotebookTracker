using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

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
