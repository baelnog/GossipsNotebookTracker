using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.LogicProvider.DataFiles.Settings
{
    [Flags]
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    internal enum IceTrapAppearanceType
    {
        [EnumMember(Value = "major_only")]
        MajorItems= 1,
        [EnumMember(Value = "junk_only")]
        JunkItems = 2,
        [EnumMember(Value = "anything")]
        Anything = MajorItems | JunkItems,
    }
}
