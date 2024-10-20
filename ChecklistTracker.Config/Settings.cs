using ChecklistTracker.Config.SettingsTypes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config;

public partial class Settings : INotifyPropertyChanged
{
#pragma warning disable 67
    public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 67

    [JsonPropertyName("reachable_locations")]
    public ReachableLocationsType ReachableLocations { get; set; } = ReachableLocationsType.AllLocations;

    [JsonPropertyName("triforce_hunt")]
    public bool TriforceHunt { get; set; }
    [JsonPropertyName("triforce_count_per_world")]
    public int TriforceHuntCountPerWorld { get; set; }
    [JsonPropertyName("triforce_goal_per_world")]
    public int TriforceHuntGoalPerWorld { get; set; }

    [JsonPropertyName("lacs_condition")]
    public WinConditionType LACSCondition { get; set; } = WinConditionType.Vanilla;
    [JsonPropertyName("lacs_medallions")]
    public int LACSMedallions { get; set; }
    [JsonPropertyName("lacs_stones")]
    public int LACSStones { get; set; }
    [JsonPropertyName("lacs_rewards")]
    public int LACSRewards { get; set; }
    [JsonPropertyName("lacs_tokens")]
    public int LACSTokens { get; set; }
    [JsonPropertyName("lacs_hearts")]
    public int LACSHearts { get; set; }

    [JsonPropertyName("bridge")]
    public WinConditionType BridgeCondition { get; set; } = WinConditionType.Medallions;
    [JsonPropertyName("bridge_medallions")]
    public int BridgeMedallions { get; set; }
    [JsonPropertyName("bridge_stones")]
    public int BridgeStones { get; set; }
    [JsonPropertyName("bridge_rewards")]
    public int BridgeRewards { get; set; }
    [JsonPropertyName("bridge_tokens")]
    public int BridgeTokens { get; set; }
    [JsonPropertyName("bridge_hearts")]
    public int BridgeHearts { get; set; }

    [JsonPropertyName("trials_random")]
    public bool TrialsRandomCount { get; set; }

    [JsonPropertyName("trials")]
    public int TrialsCount { get; set; }

    [JsonPropertyName("shuffle_ganon_bosskey")]
    public ShuffleGanonsBKType ShuffleGanonsBK { get; set; } = ShuffleGanonsBKType.Vanilla;
    [JsonPropertyName("ganon_bosskey_medallions")]
    public int GanonsBKMedallions { get; set; }
    [JsonPropertyName("ganon_bosskey_stones")]
    public int GanonsBKStones { get; set; }
    [JsonPropertyName("ganon_bosskey_rewards")]
    public int GanonsBKRewards { get; set; }
    [JsonPropertyName("ganon_bosskey_tokens")]
    public int GanonsBKTokens { get; set; }
    [JsonPropertyName("ganon_bosskey_hearts")]
    public int GanonsBKHearts { get; set; }

    [JsonPropertyName("shuffle_bosskeys")]
    public ShuffleDungeonItemType ShuffleBossKeys { get; set; } = ShuffleDungeonItemType.OwnDungeon;
    [JsonPropertyName("shuffle_smallkeys")]
    public ShuffleDungeonItemType ShuffleSmallKeys { get; set; } = ShuffleDungeonItemType.OwnDungeon;
    [JsonPropertyName("shuffle_hideoutkeys")]
    public ShuffleHideoutKeysType ShuffleHideoutKeys { get; set; } = ShuffleHideoutKeysType.Vanilla;
    [JsonPropertyName("shuffle_tcgkeys")]
    public ShuffleTreasureChestGameKeysType ShuffleTreasureChestGameKeys { get; set; } = ShuffleTreasureChestGameKeysType.Vanilla;

    [JsonPropertyName("key_rings_choice")]
    public ChoiceType KeyRingsChoice { get; set; } = ChoiceType.Off;
    [JsonPropertyName("key_rings")]
    public ISet<DungeonChoiceType> KeyRings { get; set; } = new HashSet<DungeonChoiceType>();
    [JsonPropertyName("keyring_give_bk")]
    public bool KeyRingsGiveBK { get; set; }

    [JsonPropertyName("shuffle_silverrupees")]
    public ShuffleSilverRupeesType ShuffleSilverRupees { get; set; } = ShuffleSilverRupeesType.Vanilla;
    [JsonPropertyName("silver_rupee_pouches_choice")]
    public ChoiceType SilverRupeePouchType { get; set; } = ChoiceType.Off;

    [JsonPropertyName("shuffle_mapcompass")]
    public ShuffleDungeonItemType ShuffleMapAndCompass { get; set; } = ShuffleDungeonItemType.StartWith;
    [JsonPropertyName("enhance_map_compass")]
    public bool MapAndCompassGiveInfo { get; set; }

    [JsonPropertyName("open_forest")]
    public OpenForestType KokiriForest { get; set; } = OpenForestType.Open;
    [JsonPropertyName("open_kakariko")]
    public OpenKakarikoType Kakariko { get; set; } = OpenKakarikoType.Open;
    [JsonPropertyName("open_door_of_time")]
    public bool OpenDoorOfTime { get; set; } = true;
    [JsonPropertyName("zora_fountain")]
    public OpenFountainType ZorasFountain { get; set; } = OpenFountainType.Closed;
    [JsonPropertyName("gerudo_fortress")]
    public OpenFortressType GerudoFortress { get; set; } = OpenFortressType.OneCarpenter;

    [JsonPropertyName("dungeon_shortcuts_choice")]
    public ChoiceType DungeonShortcutsChoice { get; set; } = ChoiceType.Off;
    [JsonPropertyName("dungeon_shortcuts")]
    public ISet<DungeonChoiceType> DungeonShortcuts { get; set; } = new HashSet<DungeonChoiceType>();

    [JsonPropertyName("staring_age")]
    public StartingAgeType StartingAge { get; set; } = StartingAgeType.Child;

    [JsonPropertyName("mq_dungeons_mode")]
    public MQDungeonModeType DungeonMode { get; set; }
    [JsonPropertyName("mq_dungeons_specific")]
    public ISet<DungeonChoiceType> MQDungeons { get; set; } = new HashSet<DungeonChoiceType>();
    [JsonPropertyName("mq_dungeons_count")]
    public int MQDungeonsCount { get; set; }

    [JsonPropertyName("empty_dungeons_mode")]
    public PrecompletedDungeonChoiceType PrecompletedDungeonChoice { get; set; }
    [JsonPropertyName("empty_dungeons_specific")]
    public ISet<DungeonChoiceType> PrecompletedDungeons { get; set; } = new HashSet<DungeonChoiceType>();
    [JsonPropertyName("empty_dungeons_count")]
    public int PrecompletedDungeonsCount { get; set; }

    [JsonPropertyName("shuffle_interior_entrances")]
    public ShuffleEntranceType ShuffleInteriorEntrances { get; set; } = ShuffleEntranceType.Off;
    [JsonPropertyName("shuffle_hideout_entrances")]
    public bool ShuffleHideoutEntrances { get; set; }
    [JsonPropertyName("shuffle_grotto_entrances")]
    public bool ShuffleGrottoEntrances { get; set; }
    [JsonPropertyName("shuffle_dungeon_entrances")]
    public ShuffleEntranceType ShuffleDungeonEntrances { get; set; } = ShuffleEntranceType.Off;
    [JsonPropertyName("shuffle_bosses")]
    public ShuffleEntranceType ShuffleBossEntrances { get; set; } = ShuffleEntranceType.Off;
    [JsonPropertyName("shuffle_overworld_entrances")]
    public bool ShuffleOverworldEntrances { get; set; }
    [JsonPropertyName("shuffle_gerudo_valley_river_exit")]
    public bool ShuffleValleyRiverExit { get; set; }
    [JsonPropertyName("owl_drops")]
    public bool ShuffleOwlDrops { get; set; }
    [JsonPropertyName("warp_songs")]
    public bool ShuffleWarpSongs { get; set; }
    [JsonPropertyName("spawn_positions")]
    public ISet<string> ShuffleSpawnLocations { get; set; } = new HashSet<string>();

    [JsonPropertyName("free_bombchu_drops")]
    public bool BombchuBags { get; set; }

    [JsonPropertyName("shopsanity")]
    public ShopsanityType Shopsanity { get; set; } = ShopsanityType.Off;
    [JsonPropertyName("shopsanity_prices")]
    public ShopsanityPriceType ShopsanityPrices { get; set; }

    [JsonPropertyName("tokensanity")]
    public BasicShuffleType Tokensanity { get; set; } = BasicShuffleType.Off;

    [JsonPropertyName("shuffle_scrubs")]
    public ScrubShuffleType ScrubShuffle { get; set; } = ScrubShuffleType.Off;

    [JsonPropertyName("shuffle_freestanding_items")]
    public BasicShuffleType ShuffleFreestandingItems { get; set; } = BasicShuffleType.Off;
    [JsonPropertyName("shuffle_pots")]
    public BasicShuffleType ShufflePots { get; set; } = BasicShuffleType.Off;
    [JsonPropertyName("shuffle_crates")]
    public BasicShuffleType ShuffleCrates { get; set; } = BasicShuffleType.Off;

    [JsonPropertyName("shuffle_cows")]
    public bool ShuffleCows { get; set; }
    [JsonPropertyName("shuffle_beehives")]
    public bool ShuffleBeehives { get; set; }
    [JsonPropertyName("shuffle_wonderitems")]
    public bool ShuffleWonderItems { get; set; }
    [JsonPropertyName("shuffle_kokiri_sword")]
    public bool ShuffleKokiriSword { get; set; }
    [JsonPropertyName("shuffle_ocarinas")]
    public bool ShuffleOcarinas { get; set; }
    [JsonPropertyName("shuffle_gerudo_card")]
    public bool ShuffleGerudoCard { get; set; }
    [JsonPropertyName("shuffle_beans")]
    public bool ShuffleBeans { get; set; }
    [JsonPropertyName("shuffle_expensive_merchants")]
    public bool ShuffleExpensiveMerchants { get; set; }
    [JsonPropertyName("shuffle_frog_song_rupees")]
    public bool ShuffleFrogSongRupees { get; set; }
    [JsonPropertyName("shuffle_individual_ocarina_notes")]
    public bool ShuffleOcarinaNotes { get; set; }
    [JsonPropertyName("shuffle_loach_reward")]
    public ShuffleLoachType ShuffleLoach { get; set; } = ShuffleLoachType.Off;

    [JsonPropertyName("disabled_locations")]
    public ISet<string> DisabledLocations { get; set; } = new HashSet<string>();

    // Tricks
    [JsonPropertyName("logic_no_night_tokens_without_suns_song")]
    public bool LogicNightSkullsRequireSunsSong { get; set; }

    [JsonPropertyName("allowed_tricks")]
    public ISet<string> EnabledTricks { get; set; } = new HashSet<string>();

    [JsonPropertyName("starting_items")]
    public IDictionary<string, int> StartingItems { get; set; } = new Dictionary<string, int>();

    [JsonPropertyName("starting_equipment")]
    public ISet<string> StartingEquipment { get; set; } = new HashSet<string>();
    [JsonPropertyName("starting_songs")]
    public ISet<string> StartingSongs { get; set; } = new HashSet<string>();
    [JsonPropertyName("starting_inventory")]
    public ISet<string> StartingInventory { get; set; } = new HashSet<string>();
    [JsonPropertyName("starting_hearts")]
    public int StartingHearts { get; set; }

    [JsonPropertyName("free_scarecrow")]
    public bool FreeScarecrow { get; set; }
    [JsonPropertyName("plant_beans")]
    public bool PreplantBeans { get; set; }

    [JsonPropertyName("big_poe_count_random")]
    public bool BigPoeRandomCount { get; set; }
    [JsonPropertyName("big_poe_count")]
    public int BigPoeCount { get; set; }

    [JsonPropertyName("ocarina_songs")]
    public string ShuffleSongMelodies { get; set; } = "off";

    [JsonPropertyName("correct_chest_appearances")]
    public string CAMC { get; set; } = "off";

    [JsonPropertyName("misc_hints")]
    public ISet<string> MiscHints { get; set; } = new HashSet<string>();

    [JsonPropertyName("starting_tod")]
    public string StartingTimeOfDay { get; set; } = "default";

    [JsonPropertyName("blue_fire_arrows")]
    public bool BlueFireArrows { get; set; }
}
