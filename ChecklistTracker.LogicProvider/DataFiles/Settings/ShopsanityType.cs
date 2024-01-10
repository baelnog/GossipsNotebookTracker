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
    internal enum ShopsanityType
    {
        [EnumMember(Value = "off")]
        Off,
        [EnumMember(Value = "0")]
        Items0,
        [EnumMember(Value = "1")]
        Items1,
        [EnumMember(Value = "2")]
        Items2,
        [EnumMember(Value = "3")]
        Items3,
        [EnumMember(Value = "4")]
        Items4,
        [EnumMember(Value = "random")]
        Random,
    }
}
