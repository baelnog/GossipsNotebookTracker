using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.SettingsTypes;

[Flags]
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
internal enum SilverRupeePouches
{
    [EnumMember(Value = "Dodongo's Cavern Staircase")]
    DodongosCavernStaircase,
    [EnumMember(Value = "Ice Cavern Spinning Scythe")]
    IceCavernSpinningScythe,
    [EnumMember(Value = "Ice Cavern Push Block")]
    IceCavernPushBlock,
    [EnumMember(Value = "Bottom of the Well Basement")]
    BottomOfTheWellBasement,
    [EnumMember(Value = "Shadow Temple Scythe Shortcut")]
    ShadowTempleScytheShortcut,
    [EnumMember(Value = "Shadow Temple Invisible Blades")]
    ShadowTempleInvisibleBlades,
    [EnumMember(Value = "Shadow Temple Huge Pit")]
    ShadowTempleHugePit,
    [EnumMember(Value = "Shadow Temple Invisible Spikes")]
    ShadowTempleInvisibleSpikes,
    [EnumMember(Value = "Gerudo Training Ground Slopes")]
    GerudoTrainingGroundSlopes,
    [EnumMember(Value = "Gerudo Training Ground Lava")]
    GerudoTrainingGroundLava,
    [EnumMember(Value = "Gerudo Training Ground Water")]
    GerudoTrainingGroundWater,
    [EnumMember(Value = "Spirit Temple Child Early Torches")]
    SpiritTempleChildEarlyTorches,
    [EnumMember(Value = "Spirit Temple Adult Boulders")]
    SpiritTempleAdultBoulders,
    [EnumMember(Value = "Spirit Temple Lobby and Lower Adult")]
    SpiritTempleLobbyAndLowerAdult,
    [EnumMember(Value = "Spirit Temple Sun Block")]
    SpiritTempleSunBlock,
    [EnumMember(Value = "Spirit Temple Adult Climb")]
    SpiritTempleAdultClimb,
    [EnumMember(Value = "Ganon's Castle Spirit Trial")]
    GanonsCastleSpiritTrial,
    [EnumMember(Value = "Ganon's Castle Light Trial")]
    GanonsCastleLightTrial,
    [EnumMember(Value = "Ganon's Castle Fire Trial")]
    GanonsCastleFireTrial,
    [EnumMember(Value = "Ganon's Castle Shadow Trial")]
    GanonsCastleShadowTrial,
    [EnumMember(Value = "Ganon's Castle Water Trial")]
    GanonsCastleWaterTrial,
    [EnumMember(Value = "Ganon's Castle Forest Trial")]
    GanonsCastleForestTrial,
}
