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
    internal enum ScrubShuffleType
    {
        [EnumMember(Value = "off")]
        Off,
        [EnumMember(Value = "low")]
        LowPrice,
        [EnumMember(Value = "regular")]
        ExpensivePrice,
        [EnumMember(Value = "random")]
        RandomPrice,
    }
}
