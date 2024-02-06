using ChecklistTracker.Config.SettingsTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ChildTradeItem
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

public static class ChildTradeItemExtensions
{

    public static ISet<ChildTradeItem> Items { get; } = new HashSet<ChildTradeItem>
    {
        ChildTradeItem.WeirdEgg,
        ChildTradeItem.Chicken,
        ChildTradeItem.ZeldasLetter,
        ChildTradeItem.KeatonMask,
        ChildTradeItem.SkullMask,
        ChildTradeItem.BunnyHood,
        ChildTradeItem.GoronMask,
        ChildTradeItem.ZoraMask,
        ChildTradeItem.GerudoMask,
        ChildTradeItem.MaskOfTruth,
    };

    public static IDictionary<string, ChildTradeItem> ItemLookup { get; } = Items.ToDictionary(item => item.ToLogicString());

    public static string ToLogicString(this ChildTradeItem item)
    {
        switch (item)
        {
            case ChildTradeItem.WeirdEgg:
                return "Weird_Egg";
            case ChildTradeItem.Chicken:
                return "Chicken";
            case ChildTradeItem.ZeldasLetter:
                return "Zeldas_Letter";
            case ChildTradeItem.KeatonMask:
                return "Keaton_Mask";
            case ChildTradeItem.SkullMask:
                return "Skull_Mask";
            case ChildTradeItem.BunnyHood:
                return "Bunny_Hood";
            case ChildTradeItem.GoronMask:
                return "Goron_Mask";
            case ChildTradeItem.ZoraMask:
                return "Zora_Mask";
            case ChildTradeItem.GerudoMask:
                return "Gerudo_Mask";
            case ChildTradeItem.MaskOfTruth:
                return "Mask_Of_Truth";
            default:
                throw new ArgumentException(item.ToString());
        }
    }
}
