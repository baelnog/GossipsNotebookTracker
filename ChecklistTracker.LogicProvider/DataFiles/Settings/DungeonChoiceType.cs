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
    internal enum DungeonChoiceType
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
}
