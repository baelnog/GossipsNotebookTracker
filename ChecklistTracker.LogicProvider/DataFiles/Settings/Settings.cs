using Antlr4.Runtime.Atn;
using ChecklistTracker.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChecklistTracker.LogicProvider.DataFiles.Settings
{
    public class Settings
    {

        public static async Task<Settings> ReadFromJson(string jsonFilePath)
        {
            return await TrackerConfig.ParseJson<Settings>(jsonFilePath).ConfigureAwait(false);
        }

        private static Lazy<IDictionary<string, Func<Settings, object>>> SettingsByJsonName = new Lazy<IDictionary<string, Func<Settings, object>>>(() =>
        {
            var dict = new Dictionary<string, Func<Settings, object>>();

            foreach (var property in typeof(Settings).GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                var propertyNameAttribute = property.GetCustomAttribute(typeof(JsonPropertyNameAttribute)) as JsonPropertyNameAttribute;
                if (propertyNameAttribute != null)
                {
                    dict[propertyNameAttribute.Name] = (Settings settings) => property.GetValue(settings);
                }
            }

            return dict;
        });

        public bool ContainsKey(string key)
        {
            return SettingsByJsonName.Value.ContainsKey(key);
        }

        public bool IsEnabled(string key)
        {
            return SettingsByJsonName.Value[key].Invoke(this) is bool enabled && enabled;
        }

        public bool IsSettingEqual(string key, string value)
        {
            var setting = SettingsByJsonName.Value[key].Invoke(this);

            return setting.ToString() == value;
        }

        public T GetSetting<T>(string key)
        {
            return (T)SettingsByJsonName.Value[key].Invoke(this);
        }

        public bool SettingHas(string key, string value)
        {
            var setting = SettingsByJsonName.Value[key].Invoke(this);
            return (setting as ISet<string>).Contains(value);
        }

        private bool CheckSetContains<T>(object set, string value)
        {
            if (set is ISet<T> typedSet)
            {
                var enumVal = value.ToEnum<T>();
                return enumVal != null && typedSet.Contains(enumVal);
            }
            return false;
        }

        [JsonPropertyName("world_count")]
        public int WorldCount { get; set; } = 1;

        [JsonPropertyName("logic_rules")]
        public string LogicRules { get; set; }
        [JsonPropertyName("reachable_locations")]
        public string ReachableLocations { get; set; } = "all";

        [JsonPropertyName("triforce_hunt")]
        public bool TriforceHunt{ get; set; }
        [JsonPropertyName("triforce_count_per_world")]
        public int TriforceHuntCountPerWorld { get; set; }
        [JsonPropertyName("triforce_goal_per_world")]
        public int TriforceHuntGoalPerWorld { get; set; }

        [JsonPropertyName("lacs_condition")]
        public string LACSCondition { get; set; } = "vanilla";
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
        public string BridgeCondition { get; set; } = "medallions";
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

        [JsonPropertyName("shuffle_ganon_bosskey ")]
        public string ShuffleGanonsBK { get; set; } = "vanilla";
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
        public string ShuffleBossKeys { get; set; } = "dungeon";
        [JsonPropertyName("shuffle_smallkeys")]
        public string ShuffleSmallKeys { get; set; } = "dungeon";
        [JsonPropertyName("shuffle_hideoutkeys")]
        public string ShuffleHideoutKeys { get; set; } = "vanilla";
        [JsonPropertyName("shuffle_tcgkeys")]
        public string ShuffleTreasureChestGameKeys { get; set; } = "vanilla";

        [JsonPropertyName("key_rings_choice")]
        public string KeyRingsChoice { get; set; } = "off";
        [JsonPropertyName("key_rings")]
        public ISet<string> KeyRings { get; set; } = new HashSet<string>();
        [JsonPropertyName("keyring_give_bk")]
        public bool KeyRingsGiveBK { get; set; }

        [JsonPropertyName("shuffle_silverrupees")]
        public string ShuffleSilverRupees { get; set; } = "vanilla";
        [JsonPropertyName("silver_rupee_pouches_choice")]
        public string SilverRupeePouchType { get; set; } = "off";

        [JsonPropertyName("shuffle_mapcompass")]
        public string ShuffleMapAndCompass { get; set; } = "startwith";
        [JsonPropertyName("enhance_map_compass")]
        public bool MapAndCompassGiveInfo { get; set; }

        [JsonPropertyName("open_forest")]
        public string KokiriForest { get; set; } = "open";
        [JsonPropertyName("open_kakariko")]
        public string Kakariko { get; set; } = "open";
        [JsonPropertyName("open_door_of_time")]
        public bool OpenDoorOfTime { get; set; } = true;
        [JsonPropertyName("zora_fountain")]
        public string ZorasFountain { get; set; } = "closed";
        [JsonPropertyName("gerudo_fortress")]
        public string GerudoFortress { get; set; } = "fast";

        [JsonPropertyName("dungeon_shortcuts_choice")]
        public string DungeonShortcutsChoice { get; set; } = "off";
        [JsonPropertyName("dungeon_shortcuts")]
        public ISet<string> DungeonShortcuts { get; set; } = new HashSet<string>();

        [JsonPropertyName("staring_age")]
        public string StartingAge { get; set; } = "child";

        [JsonPropertyName("mq_dungeons_mode")]
        public MQDungeonModeType DungeonMode { get; set; }
        [JsonPropertyName("mq_dungeons_specific")]
        public ISet<string> MQDungeons { get; set; } = new HashSet<string>();
        [JsonPropertyName("mq_dungeons_count")]
        public int MQDungeonsCount { get; set; }

        [JsonPropertyName("empty_dungeons_mode")]
        public PrecompletedDungeonChoiceType PrecompletedDungeonChoice { get; set; }
        [JsonPropertyName("empty_dungeons_specific")]
        public ISet<string> PrecompletedDungeons { get; set; } = new HashSet<string>();
        [JsonPropertyName("empty_dungeons_count")]
        public int PrecompletedDungeonsCount { get; set; }

        [JsonPropertyName("shuffle_interior_entrances")]
        public string ShuffleInteriorEntrances { get; set; } = "off";
        [JsonPropertyName("shuffle_hideout_entrances")]
        public bool ShuffleHideoutEntrances { get; set; }
        [JsonPropertyName("shuffle_grotto_entrances")]
        public bool ShuffleGrottoEntrances { get; set; }
        [JsonPropertyName("shuffle_dungeon_entrances")]
        public string ShuffleDungeonEntrances { get; set; } = "off";
        [JsonPropertyName("shuffle_bosses")]
        public string ShuffleBossEntrances { get; set; } = "off";
        [JsonPropertyName("shuffle_overworld_entrances")]
        public bool ShuffleOverworldEntrances { get; set; }
        [JsonPropertyName("shuffle_gerudo_valley_river_exit")]
        public bool ShuffleValleyRiverExit { get; set; }
        [JsonPropertyName("owl_drops")]
        public bool ShuffleOwlDrops { get; set; }
        [JsonPropertyName("wrap_songs")]
        public bool ShuffleWarpSongs { get; set; }
        [JsonPropertyName("spawn_positions")]
        public ISet<string> ShuffleSpawnLocations { get; set; } = new HashSet<string>();

        [JsonPropertyName("free_bombchu_drops")]
        public bool BombchuBags { get; set; }

        [JsonPropertyName("one_item_per_dungeon")]
        public bool OneItemPerDungeon { get; set; }
        [JsonPropertyName("shuffle_song_items")]
        public string ShuffleSongs { get; set; } = "song";

        [JsonPropertyName("shopsanity")]
        public string Shopsanity { get; set; } = "off";
        [JsonPropertyName("shopsanity_prices")]
        public string ShopsanityPrices { get; set; }

        [JsonPropertyName("tokensanity")]
        public string Tokensanity { get; set; } = "off";

        [JsonPropertyName("shuffle_scrubs")]
        public string ScrubShuffle { get; set; } = "off";

        [JsonPropertyName("shuffle_child_trade")]
        public ISet<string> ChildTradeEarliestItem { get; set; } = new HashSet<string>();

        [JsonPropertyName("shuffle_freestanding_items")]
        public string ShuffleFreestandingItems { get; set; } = "off";
        [JsonPropertyName("shuffle_pots")]
        public string ShufflePots { get; set; } = "off";
        [JsonPropertyName("shuffle_crates")]
        public string ShuffleCrates { get; set; } = "off";

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
        public string ShuffleLoach { get; set; } = "off";

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

        [JsonPropertyName("start_with_consumables")]
        public bool StartWithConsumables { get; set; }
        [JsonPropertyName("start_with_rupees")]
        public bool StartWithRupees { get; set; }
        [JsonPropertyName("starting_hearts")]
        public int StartingHearts { get; set; }

        [JsonPropertyName("no_escape_sequence")]
        public bool SkipCollapse { get; set; }
        [JsonPropertyName("no_guard_strealth")]
        public bool SkipStealth { get; set; }
        [JsonPropertyName("no_epona_race")]
        public bool SkipFreeingEpona { get; set; }
        [JsonPropertyName("skip_some_minigame_phases")]
        public bool FastMinigames { get; set; }
        [JsonPropertyName("complete_mask_quest")]
        public bool CompleteMaskQuest { get; set; }
        [JsonPropertyName("useful_cutscenes")]
        public bool ShowUsefulCutscenes { get; set; }
        [JsonPropertyName("fast_chests")]
        public bool FastChests { get; set; }
        [JsonPropertyName("free_scarecrow")]
        public bool FreeScarecrow { get; set; }
        [JsonPropertyName("fast_bunny_hood")]
        public bool FastBunnyHood { get; set; }
        [JsonPropertyName("auto_equip_masks")]
        public bool AutoEquipMasks { get; set; }
        [JsonPropertyName("plant_beans")]
        public bool PreplantBeans { get; set; }

        [JsonPropertyName("chicken_count_random")]
        public bool ChickensRandomCount { get; set; }
        [JsonPropertyName("chicken_count")]
        public int ChickensCount { get; set; }

        [JsonPropertyName("big_poe_count_random")]
        public bool BigPoeRandomCount { get; set; }
        [JsonPropertyName("big_poe_count")]
        public int BigPoeCount { get; set; }

        [JsonPropertyName("easier_fire_arrow_entry")]
        public bool EasyFAE { get; set; }
        [JsonPropertyName("fae_torch_count")]
        public int EasyFAETorchCount { get; set; }

        [JsonPropertyName("ruto_already_f1_jabu")]
        public bool SkipRutoBasement { get; set; }

        [JsonPropertyName("ocarina_songs")]
        public string ShuffleSongMelodies { get; set; } = "off";

        [JsonPropertyName("correct_chest_appearances")]
        public string CAMC { get; set; } = "off";

        [JsonPropertyName("minor_items_as_major_chest")]
        public bool MinorItemsAsMajorChests { get; set; }

        [JsonPropertyName("invisible_chests")]
        public bool InvisibleChests { get; set; }

        [JsonPropertyName("correct_potcrate_appearances")]
        public string PotAndCrateAppearance { get; set; } = "off";

        [JsonPropertyName("key_appearance_match_dungeon")]
        public bool KeyApperanceMatchesDungeon { get; set; }

        [JsonPropertyName("clearer_hints")]
        public bool ClearerHints { get; set; }

        [JsonPropertyName("hints")]
        public string HintRequirement { get; set; } = "none";

        [JsonPropertyName("hint_dist")]
        public string? HintDistribution { get; set; }

        [JsonPropertyName("bingosync_url")]
        public string? BingoSyncUrl { get; set; }

        [JsonPropertyName("item_hints")]
        public ISet<string> ChoiceItemHints { get; set; } = new HashSet<string>();

        [JsonPropertyName("misc_hints")]
        public ISet<string> MiscHints { get; set; } = new HashSet<string>();

        [JsonPropertyName("text_shuffle")]
        public string? _JustDeleteThisSettingTBH { get; set; }

        [JsonPropertyName("damage_multiplier")]
        public string DamageMultiplier { get; set; } = "none";

        [JsonPropertyName("deadly_bonks")]
        public string BonkDamageMultiplier { get; set; } = "none";

        [JsonPropertyName("no_collectible_hearts")]
        public bool NoCollectibleHearts { get; set; }

        [JsonPropertyName("starting_tod")]
        public string StartingTimeOfDay { get; set; } = "default";

        [JsonPropertyName("blue_fire_arrows")]
        public bool BlueFireArrows { get; set; }

        [JsonPropertyName("fix_broken_drops")]
        public bool FixBrokenDrops { get; set; }

        [JsonPropertyName("item_pool_value")]
        public string ItemPool { get; set; } = "balanced";

        [JsonPropertyName("junk_ice_traps")]
        public string IceTraps { get; set; } = "off";

        [JsonPropertyName("ice_trap_appearance")]
        public string IceTrapAppearance { get; set; } = "junk_only";

        [JsonPropertyName("adult_trade_shuffle")]
        public bool FullAdultTradeShuffle { get; set; }

        [JsonPropertyName("adult_trade_start")]
        public ISet<string> AdultTradeItemStart { get; set; } = new HashSet<string>();
    }

}
