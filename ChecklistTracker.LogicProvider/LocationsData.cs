using Antlr4.Runtime.Tree;
using ChecklistTracker.Config;
using ChecklistTracker.LogicProvider.DataFiles;
using ChecklistTracker.LogicProvider.DataFiles.Settings;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Region = ChecklistTracker.LogicProvider.DataFiles.Region;

namespace ChecklistTracker.LogicProvider
{
    internal class LocationsData
    {
        internal struct RuleData
        {
            internal string ParentRegion;
            internal IParseTree Rule;
            internal bool IsDungeon;
            internal string LocationName;
            internal string Type;
            internal string VanillaItem;
        }

        internal TripleConcurrentDictionary<RuleData> Locations { get; set; } = new TripleConcurrentDictionary<RuleData>();
        TripleConcurrentDictionary<RuleData> DropLocations { get; set; } = new TripleConcurrentDictionary<RuleData>();
        TripleConcurrentDictionary<RuleData> SkullsLocations { get; set; } = new TripleConcurrentDictionary<RuleData>();
        TripleConcurrentDictionary<RuleData> Events { get; set; } = new TripleConcurrentDictionary<RuleData>();
        TripleConcurrentDictionary<RuleData> Exits { get; set; } = new TripleConcurrentDictionary<RuleData>();

        internal ConcurrentDictionary<string, RuleData> ActiveLocations { get; set; } = new ConcurrentDictionary<string, RuleData>();
        internal DoubleConcurrentDictionary<RuleData> ActiveLocationsByRegion { get; set; } = new DoubleConcurrentDictionary<RuleData>();
        internal ConcurrentDictionary<string, ISet<RuleData>> ActiveDropLocations { get; set; } = new ConcurrentDictionary<string, ISet<RuleData>>();
        internal ConcurrentDictionary<string, RuleData> ActiveSkullsLocations { get; set; } = new ConcurrentDictionary<string, RuleData>();
        internal ConcurrentDictionary<string, ISet<RuleData>> ActiveEvents { get; set; } = new ConcurrentDictionary<string, ISet<RuleData>>();
        internal DoubleConcurrentDictionary<RuleData> ActiveExits { get; set; } = new DoubleConcurrentDictionary<RuleData> ();

        internal IDictionary<string, string> RegionMap { get; set; }
        IDictionary<string, LocationData> LocationTable { get; set; }

        Settings Settings { get; set; }
        TrackerConfig TrackerConfig { get; set; }

        internal static async Task<LocationsData> Initialize(TrackerConfig config, LogicFiles logicFiles, Settings settings)
        {
            var location = new LocationsData();

            location.Settings = settings;
            location.TrackerConfig = config;

            location.RegionMap = config.HintRegions
                .SelectMany(entry => entry.Value.Select(subValue => (subValue, entry.Key)))
                .ToDictionary(tuple => tuple.subValue, tuple => tuple.Key);

            location.LocationTable = config.LocationTable;

            foreach (var dungeon in logicFiles.DungeonFiles)
            {
                location.ParseLogicFile(dungeon.Value, isDungeon: true, isMQ: dungeon.Key.EndsWith("MQ"));
            }

            // Run through the boss file twice, since MQ and non-MQ share the same boss file
            location.ParseLogicFile(logicFiles.BossesFile, true, true);
            location.ParseLogicFile(logicFiles.BossesFile, true, false);

            location.ParseLogicFile(logicFiles.OverworldFile, false, false);

            location.ResetActiveLocations();
            // TODO: Include this in the normal flow to always have a regions object up to date ?
            // Not really optimized, so leaving it out for now.
            // updateHintRegionsJSON(_.set(dungeonFiles, "Overworld", overworldFile));

            return location;
        }


        void ParseLogicFile(IEnumerable<Region> logicFile, bool isDungeon, bool isMQ)
        {
            var locationKey = isDungeon ? (isMQ ? "dungeon_mq" : "dungeon") : "overworld";

            foreach (var region in logicFile)
            {
                var parentRegion = region.RegionName;
                var hintRegion = RegionMap[parentRegion];

                var missingLocations = new HashSet<string>();
                foreach (var location in region.Locations)
                {
                    var locationName = location.Key;
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
                            var dropData = new RuleData
                            {
                                IsDungeon = isDungeon,
                                ParentRegion = parentRegion,
                                Rule = LogicHelpers.ParseRule(rule),
                                VanillaItem = data.VanillaItem,
                            };

                            DropLocations.GetOrNew(locationKey).GetOrNew(parentRegion).PutOrAdd(data.VanillaItem, dropData);
                        }
                        else
                        {
                            // Record the location, along with pertinent information to that location
                            var locationData = new RuleData
                            {
                                IsDungeon = isDungeon,
                                LocationName = locationName,
                                ParentRegion = parentRegion,
                                Rule = LogicHelpers.ParseRule(rule),
                                Type = data.Type,
                                VanillaItem = data.VanillaItem,
                            };
                            Locations.GetOrNew(locationKey).GetOrNew(parentRegion).PutOrAdd(locationName, locationData);

                            // Additionally, if the location contains a skulltula token, record that seperately
                            if (data.Type == "GS Token")
                            {
                                SkullsLocations.GetOrNew(locationKey).GetOrNew(parentRegion).PutOrAdd(locationName, locationData);
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
                    var eventData = new RuleData
                    {
                        ParentRegion = parentRegion,
                        Rule = LogicHelpers.ParseRule(evt.Value),
                    };

                    Events.GetOrNew(locationKey).GetOrNew(parentRegion).PutOrAdd(evt.Key, eventData);
                }

                foreach (var exit in region.Exits)
                {
                    var exitData = new RuleData
                    {
                        ParentRegion = parentRegion,
                        Rule = LogicHelpers.ParseRule(exit.Value),
                        LocationName = exit.Key
                    };

                    Exits.GetOrNew(locationKey).GetOrNew(parentRegion).PutOrAdd(exit.Key, exitData);
                }

            }

        }

        bool IsAlwaysPlacedLocation(RuleData location)
        {
            return location.Type == "Drop" ||
                new List<string>() { "Triforce", "Scarecrow Song", "Deliver Letter", "Time Travel", "Bombchu Drop" }.Contains(location.VanillaItem);
        }

        bool IsGuaranteedKey(RuleData location)
        {
            var itemName = location.VanillaItem;

            return (Settings.ShuffleGanonsBK == "vanilla" && itemName == "Boss Key (Ganons Castle)") ||
                   (Settings.ShuffleBossKeys == "vanilla" && itemName.StartsWith("Boss Key ")) ||
                   (Settings.ShuffleHideoutKeys == "vanilla" && itemName == "Small Key (Thieves Hideout)") ||
                   (Settings.ShuffleSilverRupees == "vanilla" && itemName.StartsWith("Silver Rupee ")) ||
                   (Settings.ShuffleSmallKeys == "vanilla" && itemName.StartsWith("Small Key ")) ||
                   (Settings.ShuffleTreasureChestGameKeys == "vanilla" && itemName == "Small Key (Treasure Chest Game)");
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

            if (location.VanillaItem == "Gold Skulltula Token")
            {
                return Settings.Tokensanity == "all" ||
                       location.IsDungeon && Settings.Tokensanity == "dungeon" ||
                       !location.IsDungeon && Settings.Tokensanity == "overworld";
            }

            if (location.Type == "Shop")
            {
                int maxItems;
                switch (Settings.Shopsanity)
                {
                    case "off":
                        return false;
                    case "random":
                        maxItems = 4;
                        break;
                    default:
                        maxItems = int.Parse(Settings.Shopsanity);
                        break;
                }
                int itemLocation = int.Parse(location.LocationName.Substring(location.LocationName.Length - 1));

                return itemLocation >= maxItems;
            }

            if (location.Type == "Scrub" || location.Type == "GrottoScrub")
            {
                if (Settings.ScrubShuffle != "off")
                {
                    return true;
                }

                return location.VanillaItem == "Piece of Heart" ||
                       location.VanillaItem == "Deku Stick Capacity" ||
                       location.VanillaItem == "Deku Nut Capacity";
            }

            if (location.VanillaItem == "Kokiri Sword")
            {
                return Settings.ShuffleKokiriSword;
            }

            if (location.VanillaItem == "Ocarina")
            {
                return Settings.ShuffleOcarinas;
            }

            if (location.VanillaItem == "Giants Knife")
            {
                return Settings.ShuffleExpensiveMerchants;
            }

            if (location.LocationName == "Market Bombchu Bowling Bombchus" ||
                location.LocationName == "Market Bombchu Bowling Bomb")
            {
                return false;
            }

            if (new Regex(@"Bombchus( \(\d+\))").IsMatch(location.VanillaItem))
            {
                return location.LocationName != "Wasteland Bombchu Salesman" ||
                       Settings.ShuffleExpensiveMerchants;
            }

            if (location.VanillaItem == "Blue Potion")
            {
                return Settings.ShuffleExpensiveMerchants;
            }

            if (location.VanillaItem == "Milk")
            {
                return Settings.ShuffleCows;
            }

            if (location.VanillaItem == "Gerudo Membership Card")
            {
                return Settings.ShuffleGerudoCard && Settings.GerudoFortress != "open";
            }

            if (location.VanillaItem == "Buy Magic Bean")
            {
                return Settings.ShuffleBeans;
            }

            if (location.LocationName.StartsWith("ZR Frogs ") &&
                location.VanillaItem == "Rupees (50)")
            {
                return Settings.ShuffleFrogSongRupees;
            }

            if (location.LocationName == "LH Loach Fishing")
            {
                return Settings.ShuffleLoach != "off";
            }

            if (TrackerConfig.AdultTradeItems.Contains(location.VanillaItem))
            {
                if (!Settings.FullAdultTradeShuffle)
                {
                    return location.VanillaItem == "Pocket Egg" && Settings.AdultTradeItemStart.Any();
                }
                if (Settings.AdultTradeItemStart.Contains(location.VanillaItem))
                {
                    return true;
                }

                return location.VanillaItem == "Pocket Egg" && Settings.AdultTradeItemStart.Contains("Pocket Cucco");
            }

            if (TrackerConfig.ChildTradeItems.Contains(location.VanillaItem))
            {
                if (location.VanillaItem == "Weird Egg" && Settings.SkipChildZelda())
                {
                    return false;
                }
                if (!Settings.ChildTradeEarliestItem.Any())
                {
                    return false;
                }
                if (Settings.ChildTradeEarliestItem.Contains(location.VanillaItem))
                {
                    return true;
                }
                return location.VanillaItem == "Weird Egg" &&
                       Settings.ChildTradeEarliestItem.Contains("Chicken");
            }

            if (location.VanillaItem == "Small Key (Thieves Hideout)")
            {
                return Settings.ShuffleHideoutKeys != "vanilla";
            }

            if (location.LocationName.StartsWith("Market Treasure Chest Game ") &&
                location.VanillaItem != "Piece of Heart (Treasure Chest Game)")
            {
                return Settings.ShuffleTreasureChestGameKeys != "vanilla";
            }

            if (location.Type == "ActorOverride" ||
                location.Type == "Freestanding" ||
                location.Type == "Rupee Tower")
            {
                if (Settings.ShuffleFreestandingItems == "all")
                {
                    return true;
                }
                if (Settings.ShuffleFreestandingItems == "dungeon" && location.IsDungeon)
                {
                    return true;
                }
                if (Settings.ShuffleFreestandingItems == "dungeon" && !location.IsDungeon)
                {
                    return true;
                }
                return false;
            }
            if (location.Type == "Pot" || location.Type == "Flying Pot")
            {
                if (Settings.ShufflePots == "all")
                {
                    return true;
                }
                if (Settings.ShufflePots == "dungeon" && location.IsDungeon)
                {
                    return true;
                }
                if (Settings.ShufflePots == "overworld" && !location.IsDungeon)
                {
                    return true;
                }
                return false;
            }
            if (location.Type == "Crate" || location.Type == "Small Crate")
            {
                if (Settings.ShuffleCrates == "all")
                {
                    return true;
                }
                if (Settings.ShuffleCrates == "dungeon" && location.IsDungeon)
                {
                    return true;
                }
                if (Settings.ShuffleCrates == "overworld" && !location.IsDungeon)
                {
                    return true;
                }
                return false;
            }

            if (location.Type == "Beehive")
            {
                return Settings.ShuffleBeehives;
            }

            if (location.IsDungeon)
            {
                if (location.VanillaItem.StartsWith("Boss Key"))
                {
                    // Always show the boss key location even if they are vanilla.
                    return true;
                }

                if (location.VanillaItem.StartsWith("Map") || location.VanillaItem.StartsWith("Compass"))
                {
                    // Show vanilla Maps and Compasses if they give info
                    return Settings.ShuffleMapAndCompass != "vanilla" || Settings.MapAndCompassGiveInfo;
                }

                if (location.VanillaItem.StartsWith("Small Key"))
                {
                    // Always show small key chests
                    return true;
                }

                if (location.Type == "SilverRupee")
                {
                    return Settings.ShuffleSilverRupees != "vanilla";
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
            var dungeonsMQ = Settings.MQDungeons;

            ActiveLocations = ResetActiveLocations(dungeonsMQ, Locations);
            ActiveLocationsByRegion.Clear();
            foreach (var location in ActiveLocations)
            {
                ActiveLocationsByRegion.GetOrNew(location.Value.ParentRegion)[location.Key] = location.Value;
            }

            ActiveSkullsLocations = ResetActiveLocations(dungeonsMQ, SkullsLocations);

            ActiveDropLocations = ResetActiveLocationsSets(dungeonsMQ, DropLocations, (key, drop) => drop.VanillaItem);
            ActiveEvents = ResetActiveLocationsSets(dungeonsMQ, Events, (key, evt) => key);
            ActiveExits = ResetActiveLocationsMap(dungeonsMQ, Exits);
        }
        ConcurrentDictionary<string, T> ResetActiveLocations<T>(ISet<string> dungeonsMQ, TripleConcurrentDictionary<T> source)
        {
            var destination = new ConcurrentDictionary<string, T>();
            foreach (var dungeon in TrackerConfig.Dungeons)
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

        ConcurrentDictionary<string, ISet<T>> ResetActiveLocationsSets<T>(ISet<string> dungeonsMQ, TripleConcurrentDictionary<T> source,
            Func<string, T, string> getKey)
        {
            var destination = new ConcurrentDictionary<string, ISet<T>>();
            foreach (var dungeon in TrackerConfig.Dungeons)
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

        DoubleConcurrentDictionary<T> ResetActiveLocationsMap<T>(ISet<string> dungeonsMQ, TripleConcurrentDictionary<T> source)
        {
            var destination = new DoubleConcurrentDictionary<T>();

            foreach (var dungeon in TrackerConfig.Dungeons)
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
                var hintRegion = RegionMap[location.Value.ParentRegion];
                map.GetOrAdd(hintRegion, h => new ConcurrentDictionary<string, RuleData>())[location.Value.LocationName] = location.Value;
            }
            return map;
        }
    }
}
