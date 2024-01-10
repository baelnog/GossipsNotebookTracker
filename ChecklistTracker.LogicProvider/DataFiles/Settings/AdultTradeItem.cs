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
    internal enum AdultTradeItem
    {
        [EnumMember(Value = "Pocket Egg")]
        PocketEgg = 1,
        [EnumMember(Value = "Pocket Cucco")]
        PocketCucco = 2,
        [EnumMember(Value = "Cojiro")]
        Cojiro = 3,
        [EnumMember(Value = "Odd Mushroom")]
        OddMushroom = 4,
        [EnumMember(Value = "Odd Potion")]
        OddPotion = 5,
        [EnumMember(Value = "Poachers Saw")]
        PoachersSaw = 6,
        [EnumMember(Value = "Broken Sword")]
        BrokenSword = 7,
        [EnumMember(Value = "Prescription")]
        Prescription = 8,
        [EnumMember(Value = "Eyeball Frog")]
        EyeballFrog = 9,
        [EnumMember(Value = "Eyedrops")]
        Eyedrops = 10,
        [EnumMember(Value = "Claim Check")]
        ClaimCheck = 11,
    }
}
