using Antlr4.Runtime.Tree;
using ChecklistTracker.Config;
using ChecklistTracker.Config.SettingsTypes;
using ChecklistTracker.CoreUtils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Region = ChecklistTracker.LogicProvider.DataFiles.Region;

namespace ChecklistTracker.LogicProvider
{
    internal partial class LocationsData : INotifyPropertyChanged
    {
        internal struct AccessRule
        {
            internal bool IsMq;
            internal string ParentRegion;
            internal IParseTree Rule;
            internal Accessibility Accessibility;
        }

        internal struct RuleData
        {
            internal IList<AccessRule> AccessRules;
            internal string HintRegion;
            internal bool IsDungeon;
            internal string LocationName;
            internal string Type;
            internal string VanillaItem;
        }

        internal ConcurrentDictionary<string, RuleData> ActiveLocations { get; set; } = new ConcurrentDictionary<string, RuleData>();
        internal DoubleConcurrentDictionary<string, string, RuleData> ActiveLocationsByRegion { get; set; } = new DoubleConcurrentDictionary<string, string, RuleData>();
        internal ConcurrentDictionary<string, RuleData> ActiveDropLocationsByItem { get; set; } = new ConcurrentDictionary<string, RuleData>();
        internal ConcurrentDictionary<string, RuleData> ActiveSkullsLocations { get; set; } = new ConcurrentDictionary<string, RuleData>();
        internal ConcurrentDictionary<string, RuleData> ActiveEvents { get; set; } = new ConcurrentDictionary<string, RuleData>();
        internal DoubleConcurrentDictionary<string, string, RuleData> ActiveExitsByRegion { get; set; } = new DoubleConcurrentDictionary<string, string, RuleData>();

        internal IDictionary<string, string> RegionMap { get; set; }
        IDictionary<string, LocationData> LocationTable { get; set; }

        TrackerConfig TrackerConfig { get; set; }
        Settings Settings { get => TrackerConfig.RandomizerSettings; }

        public event PropertyChangedEventHandler? PropertyChanged;

        internal static async Task<LocationsData> Initialize(TrackerConfig config, LogicFiles logicFiles)
        {
            var location = new LocationsData();

            location.TrackerConfig = config;

            location.RegionMap = config.HintRegions
                .SelectMany(entry => entry.Value.Select(subValue => (subValue, entry.Key)))
                .ToDictionary(tuple => tuple.subValue, tuple => tuple.Key);

            location.LocationTable = config.LocationTable;

            // TODO: Don't parse MQ files until there is a better way to disambiguate efficiently
            foreach (var dungeon in logicFiles.DungeonFiles.Where(df => !df.Key.EndsWith("MQ")))
            {
                location.ParseLogicFile(dungeon.Value, isDungeon: true, isMQ: dungeon.Key.EndsWith("MQ"));
            }

            foreach (var dungeon in logicFiles.DungeonFilesAdditional.Where(df => !df.Key.EndsWith("MQ")))
            {
                location.ParseLogicFile(dungeon.Value, isDungeon: true, isMQ: dungeon.Key.EndsWith("MQ"));
            }

            // Run through the boss file twice, since MQ and non-MQ share the same boss file
            location.ParseLogicFile(logicFiles.BossesFile, true, false);
            location.ParseLogicFile(logicFiles.BossesFileAdditional, true, false);

            location.ParseLogicFile(logicFiles.OverworldFile, false, false);
            location.ParseLogicFile(logicFiles.OverworldFileAdditional, false, false);

            location.TrackerConfig.PropertyChanged += location.TrackerConfig_PropertyChanged;
            location.TrackerConfig.RandomizerSettings.PropertyChanged += location.RandomizerSettings_PropertyChanged;

            location.ResetActiveLocations();

            return location;
        }

        private void TrackerConfig_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TrackerConfig.RandomizerSettings))
            {
                ResetActiveLocations();
                TrackerConfig.RandomizerSettings.PropertyChanged += RandomizerSettings_PropertyChanged;
            }
        }

        private void RandomizerSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ResetActiveLocations();
        }

        void ParseLogicFile(IEnumerable<Region> logicFile, bool isDungeon, bool isMQ)
        {
            var locationKey = isDungeon ? (isMQ ? "dungeon_mq" : "dungeon") : "overworld";

            foreach (var region in logicFile)
            {
                var parentRegion = region.RegionName;
                if (!RegionMap.ContainsKey(parentRegion) && isDungeon)
                {
                    RegionMap[parentRegion] = region.Dungeon;
                }
                var hintRegion = RegionMap[parentRegion];

                var missingLocations = new HashSet<string>();
                foreach (var location in region.Locations)
                {
                    var locationName = location.Key;
                    if (isMQ)
                    {
                        if (locationName.StartsWith(hintRegion))
                        {
                            if (!locationName.Contains("MQ"))
                            {
                                locationName = locationName.Replace(hintRegion, $"{hintRegion} MQ");
                            }
                        }
                        else
                        {
                            locationName = $"MQ {locationName}";
                        }
                    }
                    var rule = location.Value;
                    try
                    {
                        var data = LocationTable[location.Key];

                        if (data.Type.StartsWith("Hint"))
                        {
                            // Ignore hints
                            continue;
                        }

                        if (data.Type == "Drop")
                        {
                            var dropData = ActiveDropLocationsByItem.GetOrAdd(data.VanillaItem, (s) => new RuleData
                            {
                                LocationName = locationName,
                                VanillaItem = data.VanillaItem,
                                AccessRules = new List<AccessRule>()
                            });

                            dropData.AccessRules.Add(new AccessRule
                            {
                                IsMq = isMQ,
                                ParentRegion = parentRegion,
                                Rule = LogicHelpers.ParseRule(rule),
                                Accessibility = Accessibility.All
                            });
                        }
                        else
                        {
                            var locationData = ActiveLocations.GetOrAdd(locationName, new RuleData
                            {
                                LocationName = locationName,
                                HintRegion = RegionMap[parentRegion],
                                IsDungeon = isDungeon,
                                Type = data.Type,
                                VanillaItem = data.VanillaItem,
                                AccessRules = new List<AccessRule>()
                            });

                            locationData.AccessRules.Add(new AccessRule
                            {
                                IsMq = isMQ,
                                ParentRegion = parentRegion,
                                Rule = LogicHelpers.ParseRule(rule),
                                Accessibility = Accessibility.All
                            });

                            ActiveLocationsByRegion.GetOrNew(parentRegion)[locationName] = locationData;

                            // Additionally, if the location contains a skulltula token, record that seperately
                            if (data.Type == "GS Token")
                            {
                                ActiveSkullsLocations[locationName] = locationData;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // Don't stop if an unknown location pops up
                        Debug.Fail($"Location {locationName} missing from location-table.json", e.ToString());
                        missingLocations.Add(locationName);
                        continue;
                    }
                }
                // Alert when there are unknown locations
                if (missingLocations.Any())
                {
                    Debug.Fail($"{region.RegionName} missing {missingLocations.Count} locations");
                }

                foreach (var evt in region.Events)
                {
                    var eventData = ActiveEvents.GetOrAdd(evt.Key, new RuleData
                    {
                        LocationName = evt.Key,
                        AccessRules = new List<AccessRule>()
                    });

                    eventData.AccessRules.Add(new AccessRule
                    {
                        ParentRegion = parentRegion,
                        Rule = LogicHelpers.ParseRule(evt.Value),
                        Accessibility = Accessibility.All
                    });
                }

                foreach (var exit in region.Exits)
                {
                    var exitData = ActiveExitsByRegion.GetOrNew(parentRegion).GetOrAdd(exit.Key, new RuleData
                    {
                        LocationName = exit.Key,
                        AccessRules = new List<AccessRule>()
                    });

                    exitData.AccessRules.Add(new AccessRule
                    {
                        ParentRegion = parentRegion,
                        Rule = LogicHelpers.ParseRule(exit.Value),
                        Accessibility = Accessibility.All
                    });
                }

            }

        }

        bool IsAlwaysPlacedLocation(RuleData location)
        {
            return location.Type == "Drop" ||
                new List<string>() { "Triforce", "Scarecrow_Song", "Deliver_Letter", "Time_Travel", "Bombchu_Drop" }.Contains(location.VanillaItem);
        }

        public bool IsProgessLocation(RuleData location)
        {
            if (location.VanillaItem == null || location.VanillaItem == "None")
            {
                return false;
            }

            if (location.LocationName == "Song from Impa")
            {
                return !Settings.SkipChildZelda();
            }

            if (Settings.DisabledLocations.Contains(location.LocationName))
            {
                return false;
            }

            if (IsAlwaysPlacedLocation(location))
            {
                return false;
            }

            if (location.VanillaItem == "Gold_Skulltula_Token")
            {
                return location.IsDungeon ?
                        Settings.Tokensanity.HasFlag(BasicShuffleType.Dungeons) :
                        Settings.Tokensanity.HasFlag(BasicShuffleType.Overworld);
            }

            if (location.Type == "Shop")
            {
                int maxItems;
                switch (Settings.Shopsanity)
                {
                    case ShopsanityType.Off:
                        return false;
                    case ShopsanityType.Random:
                        maxItems = 4;
                        break;
                    default:
                        maxItems = (int)Settings.Shopsanity;
                        break;
                }
                int itemLocation = int.Parse(location.LocationName.Substring(location.LocationName.Length - 1));

                return itemLocation >= maxItems;
            }

            if (location.Type == "Scrub" || location.Type == "GrottoScrub")
            {
                if (Settings.ScrubShuffle.IsEnabled())
                {
                    return true;
                }

                return location.VanillaItem == "Piece_of_Heart" ||
                       location.VanillaItem == "Deku_Stick_Capacity" ||
                       location.VanillaItem == "Deku_Nut_Capacity";
            }

            if (location.VanillaItem == "Kokiri_Sword")
            {
                return Settings.ShuffleKokiriSword;
            }

            if (location.VanillaItem == "Ocarina")
            {
                return Settings.ShuffleOcarinas;
            }

            if (location.VanillaItem == "Giants_Knife")
            {
                return Settings.ShuffleExpensiveMerchants;
            }

            if (location.LocationName == "Market Bombchu Bowling Bombchus" ||
                location.LocationName == "Market Bombchu Bowling Bomb")
            {
                return false;
            }

            if (new Regex(@"Bombchus(_\d+)").IsMatch(location.VanillaItem))
            {
                return location.LocationName != "Wasteland Bombchu Salesman" ||
                       Settings.ShuffleExpensiveMerchants;
            }

            if (location.VanillaItem == "Blue_Potion")
            {
                return Settings.ShuffleExpensiveMerchants;
            }

            if (location.VanillaItem == "Milk")
            {
                return Settings.ShuffleCows;
            }

            if (location.VanillaItem == "Gerudo_Membership_Card")
            {
                return Settings.ShuffleGerudoCard && !Settings.GerudoFortress.IsOpen();
            }

            if (location.VanillaItem == "Buy_Magic_Bean")
            {
                return Settings.ShuffleBeans;
            }

            if (location.LocationName.StartsWith("ZR Frogs ") &&
                location.VanillaItem == "Rupees_50")
            {
                return Settings.ShuffleFrogSongRupees;
            }

            if (location.LocationName == "LH Loach Fishing")
            {
                return Settings.ShuffleLoach != ShuffleLoachType.Off;
            }

            if (AdultTradeItemExtensions.ItemLookup.ContainsKey(location.VanillaItem))
            {
                if (!Settings.FullAdultTradeShuffle)
                {
                    return location.VanillaItem == "Pocket_Egg" && Settings.AdultTradeItemStart.Any();
                }
                if (Settings.AdultTradeItemStartLogic.Contains(location.VanillaItem))
                {
                    return true;
                }

                return location.VanillaItem == "Pocket_Egg" && Settings.AdultTradeItemStart.Contains(Config.SettingsTypes.AdultTradeItem.PocketEgg);
            }

            if (ChildTradeItemExtensions.ItemLookup.ContainsKey(location.VanillaItem))
            {
                if (location.VanillaItem == ChildTradeItem.WeirdEgg.ToLogicString() && Settings.SkipChildZelda())
                {
                    return false;
                }
                if (!Settings.ChildTradeItemStart.Any())
                {
                    return false;
                }
                if (Settings.ChildTradeItemStartLogic.Contains(location.VanillaItem))
                {
                    return true;
                }
                return location.VanillaItem == ChildTradeItem.WeirdEgg.ToLogicString() &&
                       Settings.ChildTradeItemStart.Contains(ChildTradeItem.Chicken);
            }

            if (location.VanillaItem == "Small_Key_Thieves_Hideout")
            {
                return Settings.ShuffleHideoutKeys.IsShuffled();
            }

            if (location.LocationName.StartsWith("Market Treasure Chest Game ") &&
                location.VanillaItem != "Piece_of_Heart_Treasure_Chest_Game")
            {
                return Settings.ShuffleTreasureChestGameKeys.IsShuffled();
            }

            if (location.Type == "ActorOverride" ||
                location.Type == "Freestanding" ||
                location.Type == "Rupee Tower")
            {
                return Settings.ShuffleFreestandingItems.IsShuffled(location.IsDungeon);
            }
            if (location.Type == "Pot" || location.Type == "Flying Pot")
            {
                return Settings.ShufflePots.IsShuffled(location.IsDungeon);
            }
            if (location.Type == "Crate" || location.Type == "Small Crate")
            {
                return Settings.ShuffleCrates.IsShuffled(location.IsDungeon);
            }

            if (location.Type == "Beehive")
            {
                return Settings.ShuffleBeehives;
            }

            if (location.IsDungeon)
            {
                if (location.VanillaItem.StartsWith("Boss_Key_"))
                {
                    // Always show the boss key location even if they are vanilla.
                    return true;
                }

                if (location.VanillaItem.StartsWith("Map_") || location.VanillaItem.StartsWith("Compass_"))
                {
                    // Show vanilla Maps and Compasses only if they give info
                    return Settings.ShuffleMapAndCompass != ShuffleDungeonItemType.Vanilla || Settings.MapAndCompassGiveInfo;
                }

                if (location.VanillaItem.StartsWith("Small_Key_"))
                {
                    // Always show small key chests
                    return true;
                }

                if (location.Type == "SilverRupee")
                {
                    return Settings.ShuffleSilverRupees != ShuffleSilverRupeesType.Vanilla;
                }

                if (location.Type == "Chest" ||
                    location.Type == "NPC" ||
                    location.Type == "Song" ||
                    location.Type == "Collectable" ||
                    location.Type == "Cutscene" ||
                    location.Type == "BossHeart")
                {
                    return true;
                }

                return false;
            }

            // Remaining Overworld items
            if (location.Type == "Chest" ||
                location.Type == "NPC" ||
                location.Type == "Song" ||
                location.Type == "Collectable" ||
                location.Type == "Cutscene" ||
                location.Type == "BossHeart")
            {
                return true;
            }

            return false;
        }

        void ResetActiveLocations()
        {
            this.RaisePropertyChanged(PropertyChanged);
        }
        ConcurrentDictionary<string, T> ResetActiveLocations<T>(ISet<DungeonChoiceType> dungeonsMQ, TripleConcurrentDictionary<string, string, string, T> source)
        {
            var destination = new ConcurrentDictionary<string, T>();
            foreach (var dungeon in Enum.GetValues<DungeonChoiceType>())
            {
                if (dungeonsMQ.Contains(dungeon))
                {
                    foreach (var data in source["dungeon_mq"])
                    {
                        foreach (var rule in data.Value)
                        {
                            destination[rule.Key] = rule.Value;
                        }
                    }
                }
                else
                {
                    foreach (var data in source["dungeon"])
                    {
                        foreach (var rule in data.Value)
                        {
                            destination[rule.Key] = rule.Value;
                        }
                    }
                }
                foreach (var data in source["overworld"])
                {
                    foreach (var rule in data.Value)
                    {
                        destination[rule.Key] = rule.Value;
                    }
                }
            }

            return destination;
        }

        ConcurrentDictionary<string, ISet<T>> ResetActiveLocationsSets<T>(ISet<DungeonChoiceType> dungeonsMQ, TripleConcurrentDictionary<string, string, string, T> source,
            Func<string, T, string> getKey)
        {
            var destination = new ConcurrentDictionary<string, ISet<T>>();
            foreach (var dungeon in Enum.GetValues<DungeonChoiceType>())
            {
                if (dungeonsMQ.Contains(dungeon))
                {
                    foreach (var data in source["dungeon_mq"])
                    {
                        foreach (var rule in data.Value)
                        {
                            destination.PutOrAdd(getKey(rule.Key, rule.Value), rule.Value);
                        }
                    }
                }
                else
                {
                    foreach (var data in source["dungeon"])
                    {
                        foreach (var rule in data.Value)
                        {
                            destination.PutOrAdd(getKey(rule.Key, rule.Value), rule.Value);
                        }
                    }
                }
                foreach (var data in source["overworld"])
                {
                    foreach (var rule in data.Value)
                    {
                        destination.PutOrAdd(getKey(rule.Key, rule.Value), rule.Value);
                    }
                }
            }

            return destination;
        }

        DoubleConcurrentDictionary<string, string, T> ResetActiveLocationsMap<T>(ISet<DungeonChoiceType> dungeonsMQ, TripleConcurrentDictionary<string, string, string, T> source)
        {
            var destination = new DoubleConcurrentDictionary<string, string, T>();

            foreach (var dungeon in Enum.GetValues<DungeonChoiceType>())
            {
                if (dungeonsMQ.Contains(dungeon))
                {
                    foreach (var data in source["dungeon_mq"])
                    {
                        foreach (var rule in data.Value)
                        {
                            destination.GetOrNew(data.Key)[rule.Key] = rule.Value;
                        }
                    }
                }
                else
                {
                    foreach (var data in source["dungeon"])
                    {
                        foreach (var rule in data.Value)
                        {
                            destination.GetOrNew(data.Key)[rule.Key] = rule.Value;
                        }
                    }
                }
                foreach (var data in source["overworld"])
                {
                    foreach (var rule in data.Value)
                    {
                        destination.GetOrNew(data.Key)[rule.Key] = rule.Value;
                    }
                }
            }

            return destination;
        }

        IDictionary<string, IDictionary<string, RuleData>> LocationsByHintRegion()
        {
            var map = new ConcurrentDictionary<string, IDictionary<string, RuleData>>();
            foreach (var location in ActiveLocations)
            {
                map.GetOrAdd(location.Value.HintRegion, h => new ConcurrentDictionary<string, RuleData>())[location.Value.LocationName] = location.Value;
            }
            return map;
        }
    }
}
