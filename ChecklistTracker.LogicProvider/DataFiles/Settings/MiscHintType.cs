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
    internal enum MiscHintType
    {
        [EnumMember(Value = "altar")]
        Altar,
        [EnumMember(Value = "dampe_diary")]
        DampeDiaryHookShot,
        [EnumMember(Value = "ganondorf")]
        GanondorfLightArrows,
        [EnumMember(Value = "warp_songs_and_owls")]
        WarpSongsAndOwls,
        [EnumMember(Value = "10_skulltulas")]
        Skulltulas10,
        [EnumMember(Value = "20_skulltulas")]
        Skulltulas20,
        [EnumMember(Value = "30_skulltulas")]
        Skulltulas30,
        [EnumMember(Value = "40_skulltulas")]
        Skulltulas40,
        [EnumMember(Value = "50_skulltulas")]
        Skulltulas50,
        [EnumMember(Value = "frogs2")]
        FinalFrogs,
        [EnumMember(Value = "mask_shop")]
        MaskShop,
        [EnumMember(Value = "unique_merchants")]
        UniqueMerchants,
    }
}
