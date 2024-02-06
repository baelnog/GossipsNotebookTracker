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
internal enum MinorItemsInMajorChests
{
    [EnumMember(Value = "bombchus")]
    Bombchus,
    [EnumMember(Value = "shields")]
    Shields,
    [EnumMember(Value = "capacity")]
    StickAndNutCapacity,
}
