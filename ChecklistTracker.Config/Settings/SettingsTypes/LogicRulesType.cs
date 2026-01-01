using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum LogicRulesType
{
    [EnumMember(Value = "none")]
    NoLogic,
    [EnumMember(Value = "glitchless")]
    GlitchlessLogic,
    [EnumMember(Value = "glitched")]
    GlitchedLogic
}
