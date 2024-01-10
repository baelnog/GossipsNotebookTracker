using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.LogicProvider.DataFiles.Settings
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    internal enum TimeOfDay
    {
        [EnumMember(Value = "default")]
        Default,
        [EnumMember(Value = "random")]
        Random,
        [EnumMember(Value = "sunrise")]
        Sunrise,
        [EnumMember(Value = "morning")]
        Morning,
        [EnumMember(Value = "noon")]
        Noon,
        [EnumMember(Value = "afternoon")]
        Afternoon,
        [EnumMember(Value = "sunset")]
        Sunset,
        [EnumMember(Value = "evening")]
        Evening,
        [EnumMember(Value = "midnight")]
        Midnight,
        [EnumMember(Value = "witching-hour")]
        WitchingHour,
    }
}
