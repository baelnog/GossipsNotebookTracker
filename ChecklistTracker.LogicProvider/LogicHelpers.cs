using Antlr4.Runtime;
using ChecklistTracker.ANTLR;
using ChecklistTracker.Config;
using ChecklistTracker.Config.Settings;
using ChecklistTracker.Config.Settings.SettingsTypes;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.LogicProvider.DataFiles.Settings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Access = ChecklistTracker.LogicProvider.AccessibilityExtensions;

namespace ChecklistTracker.LogicProvider
{
    internal partial class LogicHelpers
    {
        private static MemoizationCache Cache = new MemoizationCache();

        private TrackerConfig TrackerConfig;

        private SeedSettings SeedSettings { get => TrackerConfig.RandomizerSettings; }
        private IDictionary<string, ParserRuleContext> RuleAliases { get; set; }
        private IDictionary<string, object> RenamedAttributes { get; set; }

        private IDictionary<string, int> Items { get; set; }
        private Dictionary<string, ConcurrentDictionary<string, Accessibility>> Regions { get; set; }
        private LocationsData Locations { get; set; }

        private LogicHelpers(
            TrackerConfig TrackerConfig,
            Dictionary<string, ParserRuleContext> RuleAliases,
            IDictionary<string, object> RenamedAttributes,
            Dictionary<string, int> Items,
            Dictionary<string, ConcurrentDictionary<string, Accessibility>> Regions,
            LocationsData Locations)
        {
            this.TrackerConfig = TrackerConfig;
            this.RuleAliases = RuleAliases;
            this.RenamedAttributes = RenamedAttributes;
            this.Items = Items;
            this.Regions = Regions;
            this.Locations = Locations;
            RegisterCaches();

            Contract.Assert(_IsLocationAvailableMemoized != null);
            Contract.Assert(_EvalNodeMemoized != null);
            Contract.Assert(EvalAccessRuleAge != null);
            Contract.Assert(EvalIdentifier != null);
            Contract.Assert(EvalLookup != null);
            Contract.Assert(EvalCall != null);
            Contract.Assert(EvalRuleAlias != null);
            Contract.Assert(EvalEvent != null);
            Contract.Assert(CanBuy != null);
            Contract.Assert(CanAccessDrop != null);
            Contract.Assert(HasBottle != null);
            Contract.Assert(CanPlay != null);
            Contract.Assert(CanUse != null);
            Contract.Assert(EvalBinaryComparison != null);
            Contract.Assert(EvalCountCheck != null);
            Contract.Assert(HasItemMemoized != null);
            Contract.Assert(HasMedallions != null);
            Contract.Assert(HasStones != null);
            Contract.Assert(HasDungeonRewards != null);
            Contract.Assert(HasHearts != null);
            Contract.Assert(HasProjectile != null);
            Contract.Assert(RegionHasShortcuts != null);
            Contract.Assert(HasAllNotesForSong != null);
        }

        private static ParserRuleContext Parse(string key, string value)
        {
            return ParseRule(value);
        }

        internal static LogicHelpers InitHelpers(
            TrackerConfig config,
            LogicFiles logicFiles,
            LocationsData locations)
        {
            var helpers = new Dictionary<string, string>(logicFiles.LogicHelpers);
            foreach (KeyValuePair<string, string> helper in logicFiles.LogicHelpersAdditional)
            {
                if (helpers.ContainsKey(helper.Key))
                {
                    helpers[helper.Key] += " or " + helper.Value;
                }
                else
                {
                    helpers[helper.Key] = helper.Value;
                }
            }

            var ruleAliases = helpers
                .ToDictionary(
                kv => kv.Key,
                kv => Parse(kv.Key, kv.Value));

            var renamedAttributes = InitRenamedAttributes(config.RandomizerSettings);

            var items = new Dictionary<string, int>(config.DefaultInventory);
            var regions = new Dictionary<string, ConcurrentDictionary<string, Accessibility>>()
            {
                ["child"] = new ConcurrentDictionary<string, Accessibility>(),
                ["adult"] = new ConcurrentDictionary<string, Accessibility>(),
            };

            return new LogicHelpers(
                TrackerConfig: config,
                RuleAliases: ruleAliases,
                RenamedAttributes: renamedAttributes,
                Items: items,
                Regions: regions,
                Locations: locations
            );
        }

        internal static IDictionary<string, object> InitRenamedAttributes(SeedSettings settings)
        {
            var keysanity = settings.ShuffleSmallKeys.IsShuffled();
            var shuffleSilverRupees = settings.ShuffleSilverRupees != ShuffleSilverRupeesType.Vanilla;
            var beatableOnly = settings.ReachableLocations != ReachableLocationsType.AllLocations;
            var shuffleSpecialInteriorEntrances = settings.ShuffleInteriorEntrances.HasFlag(ShuffleEntranceType.Special);
            var shuffleInteriorEntrances = settings.ShuffleInteriorEntrances.IsEnabled();
            var shuffleDungeonEntrances = settings.ShuffleDungeonEntrances.IsEnabled();
            var shuffleSpecialDungeonEntrances = settings.ShuffleDungeonEntrances.HasFlag(ShuffleEntranceType.Special);

            var shuffleBosses = settings.ShuffleBossEntrances.IsEnabled();

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

        internal static ParserRuleContext ParseRule(string ruleString)
        {
            return RuleParser.Parse(ruleString);
        }

        internal void UpdateItems(IDictionary<string, int> newItems)
        {
            Items = new Dictionary<string, int>(newItems);
            Regions = new Dictionary<string, ConcurrentDictionary<string, Accessibility>>()
            {
                ["child"] = new ConcurrentDictionary<string, Accessibility>() { ["Root"] = Accessibility.All },
                ["adult"] = new ConcurrentDictionary<string, Accessibility>() { ["Root"] = Accessibility.All },
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

                foreach (var age in new string[] { "child", "adult" })
                {
                    foreach (var region in Regions[age])
                    {
                        if (!Locations.ActiveLocationsByRegion.ContainsKey(region.Key) || region.Value < Accessibility.Synthetic)
                        {
                            continue;
                        }
                        foreach (var keyLocation in Locations.ActiveLocationsByRegion[region.Key])
                        {
                            var data = keyLocation.Value;
                            var rulesForRegion = data.AccessRules.Where(rule => rule.ParentRegion == region.Key);
                            if (!guaranteedKeys.Contains(keyLocation.Value.LocationName) &&
                                rulesForRegion.Or(rule => (EvalNode(rule.Rule, region.Key, age) & rule.Accessibility)).HasFlag(Accessibility.InLogic))
                            {
                                guaranteedKeys.Add(keyLocation.Value.LocationName);
                                // Location is Accessible. Count it towards same-dungeon key-items.

                                if (keyLocation.Value.VanillaItem.Equals("Gold_Skulltula_Token"))
                                {
                                    if (!SeedSettings.Tokensanity.IsShuffled(keyLocation.Value.IsDungeon))
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
                                    if (SeedSettings.ShuffleSmallKeys == ShuffleDungeonItemType.Vanilla && keyLocation.Value.VanillaItem.StartsWith("Small_Key_") ||
                                        SeedSettings.ShuffleSmallKeys == ShuffleDungeonItemType.OwnDungeon && Locations.IsProgessLocation(keyLocation.Value))
                                    {
                                        var dungeon = keyLocation.Value.HintRegion;

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
                                else if (keyLocation.Value.HintRegion == "Thieves Hideout")
                                {
                                    if (!SeedSettings.ShuffleHideoutKeys.IsShuffled() && keyLocation.Value.VanillaItem.StartsWith("Small_Key_"))
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
            var rootRegionAccessibility = Regions[age][rootRegion];

            if (!Locations.ActiveExitsByRegion.ContainsKey(rootRegion))
            {
                return new HashSet<string>(regionsToCheck);
            }

            foreach (var exit in Locations.ActiveExitsByRegion[rootRegion])
            {
                if (!Regions[age].TryGetValue(exit.Value.LocationName, out var regionAccessibility) || regionAccessibility < Accessibility.InLogic)
                {
                    var exitsFromRegion = exit.Value.AccessRules.Where(rule => rule.ParentRegion == rootRegion);
                    var exitAccessibility = exitsFromRegion.Or(rule => rule.Accessibility & EvalNode(rule.Rule, rootRegion, age));
                    var accessibility = ((rootRegionAccessibility & exitAccessibility) | regionAccessibility);
                    Regions[age][exit.Value.LocationName] = accessibility;
                    if (accessibility > regionAccessibility)
                    {
                        regionsToCheck.AddRange(RecalculateAccessibleRegions(exit.Key, age));
                    }
                    if (accessibility < rootRegionAccessibility)
                    {
                        regionsToCheck.Add(rootRegion);
                    }
                }
            }

            return new HashSet<string>(regionsToCheck);
        }

        internal Accessibility IsLocationAvailable(string location)
        {
            return IsLocationAvailable(location, null);
        }

        internal Accessibility IsLocationAvailable(string location, string? age)
        {
            return _IsLocationAvailableMemoized(location, age);
        }

        private Func<string, string?, Accessibility> _IsLocationAvailableMemoized;
        internal Accessibility _IsLocationAvailable(string locationName, string? age)
        {
            using var indent = Logging.Indented();
            Logging.WriteLine("{0} as {1}", locationName, age);
            if (age == null)
            {
                return Access.Or(
                    () => IsLocationAvailable(locationName, "child"),
                    () => IsLocationAvailable(locationName, "adult")
                );
            }

            var locationData = Locations.ActiveLocations[locationName];

            return locationData.AccessRules.Or(rule => EvalAccessRuleAge(rule, age));
        }

        internal Accessibility EvalAccessRuleAnyAge(LocationsData.AccessRule rule)
        {
            return Access.Or(
                    () => EvalAccessRuleAge(rule, "child"),
                    () => EvalAccessRuleAge(rule, "adult")
                );
        }

        private Func<LocationsData.AccessRule, string, Accessibility> EvalAccessRuleAge;
        internal Accessibility _EvalAccessRuleAge(LocationsData.AccessRule rule, string age)
        {
            return Access.And(
                    () => rule.Accessibility,
                    () => IsRegionAccessible(rule.ParentRegion, age),
                    () => EvalNode(rule.Rule, rule.ParentRegion, age, allowReentrance: true)
                );
        }

        internal ISet<string> GetAccessilbeSkulls()
        {
            return Locations.ActiveSkullsLocations
                .Select(kv => kv.Key)
                .Where(loc => IsLocationAvailable(loc).HasFlag(Accessibility.Synthetic))
                .ToHashSet();
        }

        internal int CountSkullsInLogic()
        {
            return GetAccessilbeSkulls()
                .Count();
        }

        internal Accessibility IsRegionAccessible(string regionName, string age)
        {
            return Regions[age].TryGetValue(regionName, out var accessibility) ? accessibility : Accessibility.None;
        }
    }
}
