using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[Flags]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum DungeonChoiceType
{
    [EnumMember(Value = "Thieves Hideout")]
    ThievesHideout,
    [EnumMember(Value = "Treasure Chest Game")]
    TreasureChestGame,
    [EnumMember(Value = "Deku Tree")]
    DekuTree,
    [EnumMember(Value = "Dodongos Cavern")]
    DodongosCavern,
    [EnumMember(Value = "Jabu Jabus Belly")]
    JabuJabusBelly,
    [EnumMember(Value = "Forest Temple")]
    ForestTemple,
    [EnumMember(Value = "Fire Temple")]
    FireTemple,
    [EnumMember(Value = "Water Temple")]
    WaterTemple,
    [EnumMember(Value = "Shadow Temple")]
    ShadowTemple,
    [EnumMember(Value = "Spirit Temple")]
    SpiritTemple,
    [EnumMember(Value = "Bottom of the Well")]
    BottomOfTheWell,
    [EnumMember(Value = "Ice Cavern")]
    IceCavern,
    [EnumMember(Value = "Gerudo Training Ground")]
    GerudoTrainingGround,
    [EnumMember(Value = "Ganons Castle")]
    GanonsCastle,
}

public static class DungeonChoiceTypeExtensions
{

    private static ConcurrentDictionary<string, DungeonChoiceType> StringToEnum { get; } = new ConcurrentDictionary<string, DungeonChoiceType>();


    public static bool Contains(this ISet<DungeonChoiceType> set, string value)
    {
        return false;
        //return set.Contains(DungeonChoiceTypeExtensions.ToEnum(value));
    }
}