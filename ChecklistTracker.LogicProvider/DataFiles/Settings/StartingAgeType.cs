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
    internal enum StartingAgeType
    {
        [EnumMember(Value = "child")]
        Child,
        [EnumMember(Value = "adult")]
        Adult,
        [EnumMember(Value = "random")]
        Random,
    }
}
