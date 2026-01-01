using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings.SettingsTypes;

[Flags]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
internal enum MinorItemsInMajorChests
{
    [EnumMember(Value = "bombchus")]
    Bombchus,
    [EnumMember(Value = "shields")]
    Shields,
    [EnumMember(Value = "capacity")]
    StickAndNutCapacity,
}
