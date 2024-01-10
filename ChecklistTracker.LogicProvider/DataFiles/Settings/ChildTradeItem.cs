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
    internal enum ChildTradeItem
    {
        [EnumMember(Value = "Weird Egg")]
        WeirdEgg = 1,
        [EnumMember(Value = "Chicken")]
        Chicken = 2,
        [EnumMember(Value = "Zeldas Letter")]
        ZeldasLetter = 3,
        [EnumMember(Value = "Keaton Mask")]
        KeatonMask = 4,
        [EnumMember(Value = "Skull Mask")]
        SkullMask = 5,
        [EnumMember(Value = "Bunny Hood")]
        BunnyHood = 6,
        [EnumMember(Value = "Goron Mask")]
        GoronMask = 7,
        [EnumMember(Value = "Zora Mask")]
        ZoraMask = 8,
        [EnumMember(Value = "Gerudo Mask")]
        GerudoMask = 9,
        [EnumMember(Value = "Mask of Truth")]
        MaskOfTruth = 10,
    }
}
