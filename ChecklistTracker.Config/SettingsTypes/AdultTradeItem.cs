using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum AdultTradeItem
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

public static class AdultTradeItemExtensions
{

    public static ISet<AdultTradeItem> Items { get; } = new HashSet<AdultTradeItem>
    {
        AdultTradeItem.PocketEgg,
        AdultTradeItem.PocketCucco,
        AdultTradeItem.Cojiro,
        AdultTradeItem.OddMushroom,
        AdultTradeItem.OddPotion,
        AdultTradeItem.PoachersSaw,
        AdultTradeItem.BrokenSword,
        AdultTradeItem.Prescription,
        AdultTradeItem.EyeballFrog,
        AdultTradeItem.Eyedrops,
        AdultTradeItem.ClaimCheck,
    };

    public static IDictionary<string, AdultTradeItem> ItemLookup { get; } = Items.ToDictionary(item => item.ToLogicString());

    public static string ToLogicString(this AdultTradeItem item)
    {
        switch(item)
        {
            case AdultTradeItem.PocketEgg:
                return "Pocket_Egg";
            case AdultTradeItem.PocketCucco:
                return "Pocket_Cucco";
            case AdultTradeItem.Cojiro:
                return "Cojiro";
            case AdultTradeItem.OddMushroom:
                return "Odd_Mushroom";
            case AdultTradeItem.OddPotion:
                return "Odd_Potion";
            case AdultTradeItem.PoachersSaw:
                return "Poachers_Saw";
            case AdultTradeItem.BrokenSword:
                return "Broken_Sword";
            case AdultTradeItem.Prescription:
                return "Prescription";
            case AdultTradeItem.EyeballFrog:
                return "Eyeball_Frog";
            case AdultTradeItem.Eyedrops:
                return "Eyedrops";
            case AdultTradeItem.ClaimCheck:
                return "Claim_Check";
            default:
                throw new ArgumentException(item.ToString());
        }
    }
}
