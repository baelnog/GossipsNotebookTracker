using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config;

public partial class Settings
{
    [JsonPropertyName("world_count")]
    public int WorldCount { get; set; } = 1;

    [JsonPropertyName("logic_rules")]
    public string LogicRules { get; set; }

    [JsonPropertyName("hints")]
    public string HintRequirement { get; set; } = "none";
    [JsonPropertyName("item_hints")]
    public ISet<string> ChoiceItemHints { get; set; } = new HashSet<string>();

    [JsonPropertyName("chicken_count_random")]
    public bool ChickensRandomCount { get; set; }
    [JsonPropertyName("chicken_count")]
    public int ChickensCount { get; set; }

    [JsonPropertyName("start_with_consumables")]
    public bool StartWithConsumables { get; set; }
    [JsonPropertyName("start_with_rupees")]
    public bool StartWithRupees { get; set; }

    [JsonPropertyName("one_item_per_dungeon")]
    public bool OneItemPerDungeon { get; set; }
    [JsonPropertyName("shuffle_song_items")]
    public ShuffleSongType ShuffleSongs { get; set; } = ShuffleSongType.Song;

    [JsonPropertyName("no_escape_sequence")]
    public bool SkipCollapse { get; set; }
    [JsonPropertyName("no_guard_stealth")]
    public bool SkipStealth { get; set; }
    [JsonPropertyName("no_epona_race")]
    public bool SkipFreeingEpona { get; set; }
    [JsonPropertyName("skip_some_minigame_phases")]
    public bool FastMinigames { get; set; }
    [JsonPropertyName("useful_cutscenes")]
    public bool ShowUsefulCutscenes { get; set; }
    [JsonPropertyName("fast_chests")]
    public bool FastChests { get; set; }
    [JsonPropertyName("fast_bunny_hood")]
    public bool FastBunnyHood { get; set; }
    [JsonPropertyName("auto_equip_masks")]
    public bool AutoEquipMasks { get; set; }

    [JsonPropertyName("minor_items_as_major_chest")]
    public bool MinorItemsAsMajorChests { get; set; }

    [JsonPropertyName("invisible_chests")]
    public bool InvisibleChests { get; set; }

    [JsonPropertyName("easier_fire_arrow_entry")]
    public bool EasyFAE { get; set; }
    [JsonPropertyName("fae_torch_count")]
    public int EasyFAETorchCount { get; set; }

    [JsonPropertyName("ruto_already_f1_jabu")]
    public bool SkipRutoBasement { get; set; }

    [JsonPropertyName("correct_potcrate_appearances")]
    public string PotAndCrateAppearance { get; set; } = "off";

    [JsonPropertyName("key_appearance_match_dungeon")]
    public bool KeyApperanceMatchesDungeon { get; set; }

    [JsonPropertyName("clearer_hints")]
    public bool ClearerHints { get; set; }

    [JsonPropertyName("hint_dist")]
    public string? HintDistribution { get; set; }

    [JsonPropertyName("bingosync_url")]
    public string? BingoSyncUrl { get; set; }

    [JsonPropertyName("fix_broken_drops")]
    public bool FixBrokenDrops { get; set; }

    [JsonPropertyName("item_pool_value")]
    public string ItemPool { get; set; } = "balanced";

    [JsonPropertyName("junk_ice_traps")]
    public string IceTraps { get; set; } = "off";

    [JsonPropertyName("ice_trap_appearance")]
    public string IceTrapAppearance { get; set; } = "junk_only";

    [JsonPropertyName("text_shuffle")]
    public string? _JustDeleteThisSettingTBH { get; set; }

    [JsonPropertyName("damage_multiplier")]
    public string DamageMultiplier { get; set; } = "none";

    [JsonPropertyName("deadly_bonks")]
    public string BonkDamageMultiplier { get; set; } = "none";

    [JsonPropertyName("no_collectible_hearts")]
    public bool NoCollectibleHearts { get; set; }
}
