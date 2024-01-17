using Antlr4.Runtime;
using ChecklistTracker.ANTLR;
using ChecklistTracker.Config;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.LogicProvider.DataFiles.Settings;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChecklistTracker.LogicProvider
{
    internal partial class LogicHelpers
    {
        private static MemoizationCache Cache = new MemoizationCache();

        private Settings SeedSettings { get; set; }
        private IDictionary<string, ParserRuleContext> RuleAliases { get; set; }
        private IDictionary<string, object> RenamedAttributes { get; set; }

        private IDictionary<string, int> Items { get; set; }
        private Dictionary<string, ISet<string>> Regions { get; set; }
        private LocationsData Locations { get; set; }

        private LogicHelpers()
        {
            RegisterCaches();
        }

        private static ParserRuleContext Parse(string key, string value)
        {
            return ParseRule(value);
        }

        internal static async Task<LogicHelpers> InitHelpers(
            TrackerConfig config,
            LogicFiles logicFiles,
            Settings settings,
            LocationsData locations)
        {
            var logicHelpersFile = logicFiles.LogicHelpers;

            var ruleAliases = logicHelpersFile
                .ToDictionary(
                kv => kv.Key,
                kv => Parse(kv.Key, kv.Value));

            var renamedAttributes = InitRenamedAttributes(settings);

            // Fix incompatible closed forest
            if (settings.KokiriForest == "closed")
            {
                if (settings.ShuffleInteriorEntrances == "special" ||
                    settings.ShuffleInteriorEntrances == "all" ||
                    settings.ShuffleOverworldEntrances ||
                    settings.ShuffleWarpSongs ||
                    settings.ShuffleSpawnLocations.Any())
                {
                    settings.KokiriForest = "closed_deku";
                }
            }

            if (settings.TriforceHunt)
            {
                settings.ShuffleGanonsBK = "triforce";
            }

            if (settings.DungeonShortcutsChoice == "all")
            {
                settings.DungeonShortcuts = new HashSet<string>()
                {
                    "Deku Tree",
                    "Dodongos Cavern",
                    "Jabu Jabus Belly",
                    "Forest Temple",
                    "Fire Temple",
                    "Water Temple",
                    "Shadow Temple",
                    "Spirit Temple",
                };
            }

            if (settings.KeyRingsChoice == "all")
            {
                settings.KeyRings = new HashSet<string>()
                {
                    "Thieves Hideout",
                    "Forest Temple",
                    "Fire Temple",
                    "Water Temple",
                    "Shadow Temple",
                    "Spirit Temple",
                    "Bottom of the Well",
                    "Gerudo Training Ground",
                    "Ganons Castle",
                };
            }

            if (settings.DungeonMode == MQDungeonModeType.MasterQuest)
            {
                settings.MQDungeons = new HashSet<string>()
                {
                    "Deku Tree",
                    "Dodongos Cavern",
                    "Jabu Jabus Belly",
                    "Forest Temple",
                    "Fire Temple",
                    "Water Temple",
                    "Shadow Temple",
                    "Spirit Temple",
                    "Ganons Castle",
                    "Bottom of the Well",
                    "Ice Cavern",
                    "Gerudo Training Ground",
                };
            }

            if (settings.DungeonShortcutsChoice == "random")
            {
                settings.DungeonShortcuts = new HashSet<string>();
            }

            var items = new Dictionary<string, int>(config.DefaultInventory);
            var regions = new Dictionary<string, ISet<string>>()
            {
                ["child"] = new HashSet<string>(),
                ["adult"] = new HashSet<string>(),
            };

            return new LogicHelpers()
            {
                SeedSettings = settings,
                RuleAliases = ruleAliases,
                RenamedAttributes = renamedAttributes,
                Items = items,
                Regions = regions,
                Locations = locations,
            };
        }

        internal static IDictionary<string, object> InitRenamedAttributes(Settings settings)
        {
            var keysanity = settings.ShuffleSmallKeys.DungeonItemShuffleEnabled();
            var shuffleSilverRupees = settings.ShuffleSilverRupees != "vanilla";
            var beatableOnly = settings.ReachableLocations != "all";
            var shuffleSpecialInteriorEntrances = (settings.ShuffleInteriorEntrances == "special" || settings.ShuffleInteriorEntrances == "all");
            var shuffleInteriorEntrances = settings.ShuffleInteriorEntrances != "off";
            var shuffleSpecialDungeonEntrances = (settings.ShuffleDungeonEntrances == "special" || settings.ShuffleDungeonEntrances == "off");
            var shuffleDungeonEntrances = settings.ShuffleDungeonEntrances == "special" || settings.ShuffleDungeonEntrances == "off";

            var shuffleBosses = settings.ShuffleBossEntrances != "off";

            var entranceShuffle = shuffleInteriorEntrances ||
                settings.ShuffleGrottoEntrances ||
                shuffleDungeonEntrances ||
                settings.ShuffleOverworldEntrances ||
                settings.ShuffleValleyRiverExit ||
                settings.ShuffleOwlDrops ||
                settings.ShuffleWarpSongs ||
                settings.ShuffleSpawnLocations.Any() ||
                shuffleBosses;

            var mixedPoolsBosses = false;

            var ensureTodAccess =
              shuffleInteriorEntrances || settings.ShuffleOverworldEntrances || settings.ShuffleSpawnLocations.Any();

            var disableTradeRevert =
              shuffleInteriorEntrances || settings.ShuffleOverworldEntrances || settings.FullAdultTradeShuffle;

            var skipChildZelda = settings.SkipChildZelda();

            var triforceGoal = settings.TriforceHuntGoalPerWorld * settings.WorldCount;

            return new Dictionary<string, object>()
            {
                ["keysanity"] = keysanity,
                ["shuffle_silver_rupees"] = shuffleSilverRupees,
                ["check_beatable_only"] = beatableOnly,
                ["shuffle_special_interior_entrances"] = shuffleSpecialInteriorEntrances,
                ["shuffle_interior_entrances"] = shuffleInteriorEntrances,
                ["shuffle_special_dungeon_entrances"] = shuffleSpecialDungeonEntrances,
                ["shuffle_dungeon_entrances"] = shuffleDungeonEntrances,
                ["entrance_shuffle"] = entranceShuffle,
                ["mixed_pools_bosses"] = mixedPoolsBosses,
                ["ensure_tod_access"] = ensureTodAccess,
                ["disable_trade_revert"] = disableTradeRevert,
                ["skip_child_zelda"] = skipChildZelda,
                ["triforce_goal"] = triforceGoal,
            };
        }

        string GetItemName(string item)
        {
            Regex r = new Regex(@"[\(\)]");
            return r.Replace(item, (match) =>
            {
                return match.Value == " " ? "_" : "";
            });
        }

        static internal ParserRuleContext ParseRule(string ruleString)
        {
            return RuleParser.Parse(ruleString);
        }

        internal void UpdateItems(IDictionary<string, int> newItems)
        {
            Items = new Dictionary<string, int>(newItems);
            Regions = new Dictionary<string, ISet<string>>()
            {
                ["child"] = new HashSet<string>(),
                ["adult"] = new HashSet<string>(),
            };

            ISet<string> accessibleChildRegions;
            var newChildRegions = RecalculateAccessibleRegions("Root", "child");

            ISet<string> accessibleAdultRegions;
            var newAdultRegions = RecalculateAccessibleRegions("Root", "adult");

            // Auto-"collect" synthetic copies of always items.
            // Also track synthetic small keys according to how many locations are available if keys are in dungeons.
            var guaranteedKeys = new HashSet<string>();
            bool updatedKeys;
            do
            {
                updatedKeys = false;
                Cache.Clear();

                foreach (var age in new string[]{ "child", "adult" })
                {
                    foreach (var region in Regions[age])
                    {
                        if (!Locations.ActiveLocationsByRegion.ContainsKey(region))
                        {
                            continue;
                        }
                        foreach (var keyLocation in Locations.ActiveLocationsByRegion[region])
                        {
                            if (!guaranteedKeys.Contains(keyLocation.Value.LocationName) &&
                                EvalNode(keyLocation.Value.Rule, region, age))
                            {
                                guaranteedKeys.Add(keyLocation.Value.LocationName);
                                // Location is Accessible. Count it towards same-dungeon key-items.

                                if (keyLocation.Value.VanillaItem.Equals("Gold_Skulltula_Token"))
                                {
                                    if (SeedSettings.Tokensanity == "dungeon" && !keyLocation.Value.IsDungeon ||
                                        SeedSettings.Tokensanity == "overworld" && keyLocation.Value.IsDungeon ||
                                        SeedSettings.Tokensanity == "off")
                                    {
                                        var smallKeyItem = Synthetic("Gold_Skulltula_Token");
                                        if (!Items.ContainsKey(smallKeyItem))
                                        {
                                            Items[smallKeyItem] = 0;
                                        }
                                        Items[smallKeyItem]++;
                                        updatedKeys = true;
                                    }
                                }

                                if (keyLocation.Value.IsDungeon)
                                {
                                    if (SeedSettings.ShuffleSmallKeys == "vanilla" && keyLocation.Value.VanillaItem.StartsWith("Small_Key_") ||
                                        SeedSettings.ShuffleSmallKeys == "dungeon" && Locations.IsProgessLocation(keyLocation.Value))
                                    {
                                        var dungeon = Locations.RegionMap[keyLocation.Value.ParentRegion];

                                        var smallKeyItem = Synthetic($"Small_Key_{dungeon.Replace(" ", "_")}");
                                        if (!Items.ContainsKey(smallKeyItem))
                                        {
                                            Items[smallKeyItem] = 0;
                                        }
                                        Items[smallKeyItem]++;
                                        updatedKeys = true;
                                    }
                                    else if (keyLocation.Value.VanillaItem.StartsWith("Silver_Rupee"))
                                    {
                                        var silverRupeeItem = Synthetic(keyLocation.Value.VanillaItem);
                                        if (!Items.ContainsKey(silverRupeeItem))
                                        {
                                            Items[silverRupeeItem] = 0;
                                        }
                                        Items[silverRupeeItem]++;
                                        updatedKeys = true;
                                    }
                                }
                                else if (Locations.RegionMap[keyLocation.Value.ParentRegion] == "Thieves Hideout")
                                {
                                    if (SeedSettings.ShuffleHideoutKeys == "vanilla" && keyLocation.Value.VanillaItem.StartsWith("Small_Key_"))
                                    {
                                        var smallKeyItem = Synthetic(keyLocation.Value.VanillaItem);
                                        if (!Items.ContainsKey(smallKeyItem))
                                        {
                                            Items[smallKeyItem] = 0;
                                        }
                                        Items[smallKeyItem]++;
                                        updatedKeys = true;
                                    }
                                }

                            }
                        }
                    }
                }

                accessibleChildRegions = new HashSet<string>(newChildRegions);
                accessibleAdultRegions = new HashSet<string>(newAdultRegions);

                foreach (var region in accessibleChildRegions)
                {
                    newChildRegions.UnionWith(RecalculateAccessibleRegions(region, "child"));
                }

                foreach (var region in accessibleAdultRegions)
                {
                    newAdultRegions.UnionWith(RecalculateAccessibleRegions(region, "adult"));
                }


            }
            while (updatedKeys ||
                   !accessibleChildRegions.SetEquals(newChildRegions) ||
                   !accessibleAdultRegions.SetEquals(newAdultRegions));
        }

        internal ISet<string> RecalculateAccessibleRegions(string rootRegion, string age)
        {

            Cache.Clear();
            var regionsToCheck = new List<string>();

            if (!Locations.ActiveExits.ContainsKey(rootRegion))
            {
                return new HashSet<string>(regionsToCheck);
            }

            foreach(var exit in Locations.ActiveExits[rootRegion])
            {
                if (!Regions[age].Contains(exit.Value.LocationName))
                {
                    //Logging.LogMessage($"Testing Exit {exit.Value.LocationName} is accessible as {age}");
                    if (EvalNode(exit.Value.Rule, rootRegion, age))
                    {
                        //Logging.LogMessage($"Exit {exit.Value.LocationName} is accessible as {age}");
                        Regions[age].Add(exit.Value.LocationName);
                        regionsToCheck.AddRange(RecalculateAccessibleRegions(exit.Key, age));
                    }
                    else
                    {
                        //Logging.LogMessage($"Exit {exit.Value.LocationName} is not accessible as {age}");
                        regionsToCheck.Add(rootRegion);
                    }
                }
            }

            return new HashSet<string>(regionsToCheck);
        }

        internal bool IsLocationAvailable(string location)
        {
            return IsLocationAvailable(location, null);
        }

        internal bool IsLocationAvailable(string location, string? age)
        {
            return _IsLocationAvailableMemoized(location, age);
        }

        private Func<string, string, bool> _IsLocationAvailableMemoized;
        internal bool _IsLocationAvailable(string locationName, string? age)
        {
            using var indent = Logging.Indented();
            Logging.WriteLine("{0} as {1}", locationName, age);

            var parentRegion = Locations.ActiveLocations[locationName].ParentRegion;
            var locationRule = Locations.ActiveLocations[locationName].Rule;

            if (age == null)
            {
                return IsLocationAvailable(locationName, "child") || IsLocationAvailable(locationName, "adult");
            }
            else
            {
                return IsRegionAccessible(parentRegion, age) && EvalNode(locationRule, parentRegion, age, allowReentrance: true);
            }
        }

        internal ISet<string> GetAccessilbeSkulls()
        {
            return Locations.ActiveSkullsLocations
                .Select(kv => kv.Key)
                .Where(IsLocationAvailable)
                .ToHashSet();
        }

        internal int CountSkullsInLogic()
        {
            return GetAccessilbeSkulls()
                .Count();
        }

        internal bool IsRegionAccessible(string regionName, string age)
        {
            return Regions[age].Contains(regionName);
        }
    }
}
