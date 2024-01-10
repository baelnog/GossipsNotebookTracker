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
    internal enum OpenFountainType
    {
        [EnumMember(Value = "closed")]
        Closed,
        [EnumMember(Value = "adult")]
        OpenAdult,
        [EnumMember(Value = "open")]
        Open,
    }
}
