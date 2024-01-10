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
    internal enum IceTrapType
    {
        [EnumMember(Value = "off")]
        Off,
        [EnumMember(Value = "normal")]
        Normal,
        [EnumMember(Value = "on")]
        On,
        [EnumMember(Value = "mayhem")]
        Mayhem,
        [EnumMember(Value = "onslaught")]
        Onslaught,
    }
}
