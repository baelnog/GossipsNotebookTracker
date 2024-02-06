using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ReachableLocationsType
{
    [EnumMember(Value = "all")]
    AllLocations,
    [EnumMember(Value = "all_goals")]
    AllGoals,
    [EnumMember(Value = "beatable")]
    BeatableOnly
}
