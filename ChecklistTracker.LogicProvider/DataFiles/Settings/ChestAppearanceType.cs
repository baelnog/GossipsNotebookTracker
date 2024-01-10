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
    internal enum ChestAppearanceType
    {
        [EnumMember(Value = "off")]
        Off = 0,
        [EnumMember(Value = "classic")]
        Size = 1,
        [EnumMember(Value = "textures")]
        Textures = 2,
        [EnumMember(Value = "both")]
        Both = Size | Textures,
    }
}
