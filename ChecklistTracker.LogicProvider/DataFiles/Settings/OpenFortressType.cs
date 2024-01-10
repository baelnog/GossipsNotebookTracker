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
    internal enum OpenFortressType
    {
        [EnumMember(Value = "normal")]
        Normal,
        [EnumMember(Value = "fast")]
        OneCarpenter,
        [EnumMember(Value = "open")]
        Open,
    }
}
