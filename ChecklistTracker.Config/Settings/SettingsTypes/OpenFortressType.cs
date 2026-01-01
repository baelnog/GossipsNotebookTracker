using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum OpenFortressType
{
    [EnumMember(Value = "normal")]
    Normal,
    [EnumMember(Value = "fast")]
    OneCarpenter,
    [EnumMember(Value = "open")]
    Open,
}

public static class OpenFortressTypeExtensions
{
    public static bool IsOpen(this OpenFortressType value)
    {
        return value == OpenFortressType.Open;
    }
}