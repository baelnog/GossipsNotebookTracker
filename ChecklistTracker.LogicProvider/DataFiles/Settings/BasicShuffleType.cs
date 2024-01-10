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
    internal enum BasicShuffleType
    {
        [EnumMember(Value = "off")]
        Off = 0,
        [EnumMember(Value = "dungeons")]
        Dungeons = 1,
        [EnumMember(Value = "overworld")]
        Overworld = 2,
        [EnumMember(Value = "all")]
        All = Dungeons | Overworld,
    }
}
