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
    internal enum ShuffleHideoutKeysType
    {
        [EnumMember(Value = "vanilla")]
        Vanilla,
        [EnumMember(Value = "fortress")]
        Fortress,
        [EnumMember(Value = "regional")]
        Regional,
        [EnumMember(Value = "overworld")]
        Overworld,
        [EnumMember(Value = "any_dungeon")]
        AnyDungeon,
        [EnumMember(Value = "keysanity")]
        Anywhere,
    }
}
