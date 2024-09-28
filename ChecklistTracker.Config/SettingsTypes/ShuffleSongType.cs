using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ShuffleSongType
{
    [EnumMember(Value = "song")]
    Song,
    [EnumMember(Value = "dungeon")]
    Dungeon,
    [EnumMember(Value = "any")]
    Anywhere,
}
