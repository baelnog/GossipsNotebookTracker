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
    internal enum ShuffleSilverRupeesType
    {
        [EnumMember(Value = "remove")]
        Remove,
        [EnumMember(Value = "vanilla")]
        Vanilla,
        [EnumMember(Value = "dungeon")]
        OwnDungeon,
        [EnumMember(Value = "regional")]
        Regional,
        [EnumMember(Value = "overworld")]
        Overwold,
        [EnumMember(Value = "any_dungeon")]
        AnyDungeon,
        [EnumMember(Value = "anywhere")]
        Anywhere
    }
}
