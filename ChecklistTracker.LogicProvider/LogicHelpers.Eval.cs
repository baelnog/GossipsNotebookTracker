using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ChecklistTracker.ANTLR;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.LogicProvider.DataFiles.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChecklistTracker.LogicProvider
{
    internal partial class LogicHelpers
    {
        private const string SyntheticTag = "Synthetic_";

        private string Synthetic(string item)
        {
            return SyntheticTag + item;
        }

        private void RegisterCaches()
        {
            _IsLocationAvailableMemoized = Cache.Memoize<string, string, bool>("IsLocationAvailable", _IsLocationAvailable);
            _EvalNodeMemoized = Cache.Memoize<IParseTree, string, string, bool>("EvalNode", _EvalNode, onReentrance: () => false);
            EvalIdentifier = Cache.Memoize<string, string, bool>("EvalIdentifier", _EvalIdentifier);
            EvalEvent = Cache.Memoize<string, bool>("EvalEvent", _EvalEvent);
            EvalCall = Cache.Memoize<string, Python3Parser.ArgumentContext[], bool>("EvalCall", _EvalCall);
            EvalLookup = Cache.Memoize<string, Python3Parser.Subscript_Context[], bool>("EvalLookup", _EvalLookup);
            EvalRuleAlias = Cache.Memoize<string, string, bool>("EvalRuleAlias", _EvalRuleAlias);
            CanBuy = Cache.Memoize<string, string, bool>("CanBuy", _CanBuy);
            CanAccessDrop = Cache.Memoize<string, string, bool>("CanAccessDrop", _CanAccessDrop);
            HasBottle = Cache.Memoize<string, bool>("HasBottle", _HasBottle);
            CanPlay = Cache.Memoize<string, string, bool>("CanPlay", _CanPlay);
            CanUse = Cache.Memoize<string, string?, bool>("CanUse", _CanUse);
            EvalBinaryComparison = Cache.Memoize<string, string, string, string, bool>("EvalBinaryComparison", _EvalBinaryComparison);
            EvalCountCheck = Cache.Memoize<string, string, bool>("EvalCountCheck", _EvalCountCheck);
            HasItemMemoized = Cache.Memoize<string, int, bool>("HasItem", _HasItem);
            HasMedallions = Cache.Memoize<string, bool>("HasMedallions", _HasMedallions);
            HasStones = Cache.Memoize<string, bool>("HasStones", _HasStones);
            HasDungeonRewards = Cache.Memoize<string, bool>("HasDungeonRewards", _HasDungeonRewards);
            HasHearts = Cache.Memoize<string, bool>("HasHearts", _HasHearts);
            HasProjectile = Cache.Memoize<string, bool>("HasProjectile", _HasProjectile);
            RegionHasShortcuts = Cache.Memoize<string, bool>("RegionHasShortcuts", _RegionHasShortcuts);
            HasAllNotesForSong = Cache.Memoize<string, bool>("HasAllNotesForSong", _HasAllNotesForSong);
            HasAvailableDungeonKeysMemoized = Cache.Memoize<string, int, bool>("HasAvailableDungeonKeys", _HasAvailableDungeonKeys, onReentrance: () => false);

        }
        private Stack<string> VisitingHere = new Stack<string>();
        private Stack<string> VisitingAge = new Stack<string>();

        private class StackScope : IDisposable
        {
            private Stack<string> AgeStack;
            public StackScope(Stack<string> ageStack, string age) { AgeStack = ageStack; AgeStack.Push(age); }
            public void Dispose() { AgeStack.Pop(); }
        }

        internal IDisposable PushAge(string age) { return new StackScope(VisitingAge, age); }
        internal IDisposable PushHere(string here) { return new StackScope(VisitingHere, here); }


        Func<IParseTree, string, string, bool, bool> _EvalNodeMemoized;
        public bool EvalNode(IParseTree tree, string here, string age, bool allowReentrance = false)
        {
            return _EvalNodeMemoized(tree, here, age, allowReentrance);
        }

        public bool _EvalNode(IParseTree tree, string here, string age)
        {
            using (PushAge(age))
            {
                using (PushHere(here))
                {
                    var result = tree.Accept(this);
                    return result;
                }
            }
        }

        private bool EvalAdultTradeItem(string name, string item, string checkName)
        {
            if (HasItem(name))
            {
                return true;
            }

            if (!SeedSettings.FullAdultTradeShuffle || !SeedSettings.AdultTradeItemStart.Contains(item))
            {
                return IsLocationAvailable(checkName);
            }

            return false;
        }

        Func<string, string, bool> EvalIdentifier;
        bool _EvalIdentifier(string name, string age)
        {
            using var indent = Logging.Indented();
            Logging.WriteLine($"EvalIdentifier({name}, {age})");
            switch (name)
            {
                case "True":
                    return true;
                case "False":
                    return false;
                case "had_night_start":
                    return this.SeedSettings.StartingTimeOfDay.IsNight();
                case "has_bottle":
                    return HasBottle(age);
                case "Big_Poe":
                    return CanAccessDrop("Big_Poe", age);
                case "Bombchu_Drop":
                    return IsLocationAvailable("Market Bombchu Bowling Bombchus", age);
                case "Deliver_Letter":
                    return IsLocationAvailable("Deliver Rutos Letter");
                case "Time_Travel":
                    return IsLocationAvailable("Master Sword Pedestal", age);
                case "is_child":
                    return age == "child";
                case "is_adult":
                    return age == "adult";
                case "Zeldas_Letter":
                    return HasItem(name) ||
                            SeedSettings.SkipChildZelda() ||
                            IsLocationAvailable("HC Zeldas Letter");
                case "Keaton_Mask":
                    return HasItem(name) || IsLocationAvailable("Market Mask Shop Item 6");
                case "Skull_Mask":
                    return HasItem(name) || IsLocationAvailable("Market Mask Shop Item 5");
                case "Spooky_Mask":
                    return HasItem(name) || IsLocationAvailable("Market Mask Shop Item 8");
                case "Bunny_Mask":
                    return HasItem(name) || IsLocationAvailable("Market Mask Shop Item 7");
                case "Mask_of_Truth":
                    return HasItem(name) || IsLocationAvailable("Market Mask Shop Item 3");
                case "Odd_Mushroom":
                    return EvalAdultTradeItem(name, "Odd Mushroom", "LW Trade Cojiro");
                case "Odd_Potion":
                    return EvalAdultTradeItem(name, "Odd Potion", "Kak Granny Trade Odd Mushroom");
                case "Poachers_Saw":
                    return EvalAdultTradeItem(name, "Poachers Saw", "LW Trade Odd Potion");
                case "Broken_Sword":
                    return EvalAdultTradeItem(name, "Broken Sword", "GV Trade Poachers Saw");
                case "Prescription":
                    return EvalAdultTradeItem(name, "Prescription", "DMT Trade Broken Sword");
                case "Eyeball_Frog":
                    return EvalAdultTradeItem(name, "Eyeball Frog", "ZD Trade Prescription");
                case "Eyedrops":
                    return EvalAdultTradeItem(name, "Eyedrops", "LH Trade Eyeball Frog");
                case "Claim_Check":
                    return EvalAdultTradeItem(name, "Claim Check", "DMT Trade Eyedrops");
            }

            var unquotedName = name;
            if (name.Contains("'"))
            {
                unquotedName = name.Replace("'", "").Replace(" ", "_");
            }
            if (Items.ContainsKey(unquotedName))
            {
                return EvalCountCheck(unquotedName, "1");
            }

            if (RenamedAttributes.ContainsKey(name))
            {
                return (bool)RenamedAttributes[name];
            }

            if (SeedSettings.ContainsKey(name))
            {
                Logging.WriteLine($"({name}, {age}) -> Setting enabed [{SeedSettings.IsEnabled(name)}]");
                return SeedSettings.IsEnabled(name);
            }

            if (RuleAliases.ContainsKey(name))
            {
                return EvalRuleAlias(name, age);
            }

            var eventId = unquotedName.Replace("_", " ");
            if (Locations.ActiveEvents.ContainsKey(eventId))
            {
                return EvalEvent(eventId);
            }

            if (name.StartsWith("logic_"))
            {
                return SeedSettings.EnabledTricks.Contains(name);
            }

            // Assume access to time passage is always available.
            if (name.StartsWith("at_"))
            {
                return true;
            }

            if (name.StartsWith("Buy_"))
            {
                return CanBuy(name, age);
            }

            var dropId = unquotedName.Replace(" ", "_");
            if (Locations.ActiveDropLocations.ContainsKey(dropId))
            {
                return CanAccessDrop(dropId, age);
            }

            throw new NotImplementedException($"Unknown identifier: {name}");
        }
        Func<string, Python3Parser.Subscript_Context[], bool> EvalLookup;
        bool _EvalLookup(string name, Python3Parser.Subscript_Context[] args)
        {
            switch (name)
            {
                case "skipped_trials":
                    return SeedSettings.TrialsCount == 0 || SeedSettings.TrialsRandomCount;
            }

            throw new NotImplementedException($"Unknown function: {name}");
        }

        Func<string, Python3Parser.ArgumentContext[], bool> EvalCall;
        bool _EvalCall(string name, Python3Parser.ArgumentContext[] args)
        {
            var age = VisitingAge.Peek();
            switch (name)
            {
                case "at":
                    return At(args[0].GetText().Replace("'", ""), args[1], age);
                case "at_night":
                    return AtNight(age);
                case "can_play":
                    return CanPlay(args[0].GetText(), age);
                case "can_use":
                    return CanUse(args[0].GetText(), age);
                case "has_dungeon_rewards":
                    return HasDungeonRewards(args[0].GetText());
                case "has_hearts":
                    return HasHearts(args[0].GetText());
                case "has_medallions":
                    return HasMedallions(args[0].GetText());
                case "has_projectile":
                    return HasProjectile(args[0].GetText());
                case "has_stones":
                    return HasStones(args[0].GetText());
                case "here":
                    return Here(args);
                case "region_has_shortcuts":
                    return RegionHasShortcuts(args[0].GetText());
                case "has_all_notes_for_song":
                    return HasAllNotesForSong(args[0].GetText());
            }
            throw new NotImplementedException($"Unknown function: {name}");
        }

        Func<string, string, bool> EvalRuleAlias;
        bool _EvalRuleAlias(string name, string age)
        {
            using var indent = Logging.Indented();

            Logging.WriteLine($"EvalRuleAlias({name}, {age})");
            return RuleAliases[name].Accept(this);
        }

        public Func<string, bool> EvalEvent;
        bool _EvalEvent(string name)
        {
            using var indent = Logging.Indented();
            var escaped = name.Replace("_", " ").Replace("'", "");
            if (escaped != name)
            {
                return EvalEvent(escaped);
            }
            if (!Locations.ActiveEvents.ContainsKey(name))
            {
                return false;
            }
            if (name == "Eyeball Frog Access")
            {
                if (!EvalEvent("King Zora Thawed"))
                {
                    return false;
                }
                if (!(bool)RenamedAttributes["disable_trade_revert"] && (HasItem("Eyedrops") || HasItem("Eyebal_Frog")) || // Can revert to prescription
                    HasItem("Prescription") || // Has prescription
                    (!SeedSettings.FullAdultTradeShuffle || SeedSettings.AdultTradeItemStart.Contains("Broken Sword")) && EvalEvent("Prescription Access")) // Can get prescription
                {
                    return true;
                }
                return false;
            }

            return Locations.ActiveEvents[name].Any(evt =>
            {
                var parentRegion = evt.ParentRegion;
                var rule = evt.Rule;

                return IsRegionAccessible(parentRegion, "child") && EvalNode(rule, parentRegion, "child") ||
                       IsRegionAccessible(parentRegion, "adult") && EvalNode(rule, parentRegion, "adult");
            });
        }

        bool AtNight(string age)
        {
            // Hardcoded for now.
            return true;
        }

        bool At(string spotToCheck, ParserRuleContext arg1, string age)
        {
            return IsRegionAccessible(spotToCheck, "child") && EvalNode(arg1, spotToCheck, "child") ||
                   IsRegionAccessible(spotToCheck, "adult") && EvalNode(arg1, spotToCheck, "adult");
        }

        bool Here(Python3Parser.ArgumentContext[] args)
        {
            return At(VisitingHere.Peek(), args[0], "child") || At(VisitingHere.Peek(), args[0], "adult");
        }

        Func<string, string, bool> CanBuy;
        bool _CanBuy(string itemName, string age)
        {
            var rules = new HashSet<string>(); 
            if (new HashSet<string> { "Buy_Arrows_50", "Buy_Fish", "Buy_Goron_Tunic", "Buy_Bombchu_20", "Buy_Bombs_30" }.Contains(itemName))
            {
                if (!HasItem("Progressive_Wallet"))
                {
                    return false;
                }
            }
            if (itemName == "Buy_Zora_Tunic" || itemName == "Buy_Blue_Fire")
            {
                if (age != "adult" || !HasItem("Progressive_Wallet", 2))
                {
                    return false;
                }
            }

            if (new HashSet<string> {
                "Buy_Blue_Fire",
                "Buy_Blue_Potion",
                "Buy_Bottle_Bug",
                "Buy_Fish",
                "Buy_Green_Potion",
                "Buy_Poe",
                "Buy_Red_Potion_for_30_Rupees",
                "Buy_Red_Potion_for_40_Rupees",
                "Buy_Red_Potion_for_50_Rupees",
                "Buy_Fairy's_Spirit", }.Contains(itemName))
            {
                if (!HasBottle(age))
                {
                    return false;
                }
            }

            if (itemName == "Buy_Bombchu_5" ||
                itemName == "Buy_Bombchu_10" ||
                itemName == "Buy_Bombchu_20")
            {
                // The hashfrog implementation calls found_bombchus, but that seems to infinite loop...
                // Make our own logic with blackjack and hardcoding
                //if (!EvalRuleAlias("found_bombchus", age))
                //{
                //    return false;
                //}
                // "(free_bombchu_drops and (Bombchus or Bombchus_5 or Bombchus_10 or Bombchus_20)) or (not free_bombchu_drops and Bomb_Bag)"
                if (SeedSettings.BombchuBags)
                {
                    if (!HasItem("Bombchus") &&
                        !HasItem("Bombchus_5") &&
                        !HasItem("Bombchus_10") &&
                        !HasItem("Bombchus_20"))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!HasItem("Bomb_Bag"))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        Func<string, string, bool> CanAccessDrop;
        bool _CanAccessDrop(string dropName, string age)
        {
            using var indent = Logging.Indented();
            Logging.WriteLine($"CanAccessDrop({dropName}, {age})");

            return Locations.ActiveDropLocations[dropName].Any(drop =>
            {
                var parentRegion = drop.ParentRegion;
                var rule = drop.Rule;

                return IsRegionAccessible(parentRegion, "child") && EvalNode(rule, parentRegion, "child", allowReentrance: true) ||
                        IsRegionAccessible(parentRegion, "adult") && EvalNode(rule, parentRegion, "adult", allowReentrance: true);
            });
        }

        Func<string, bool> HasBottle;
        bool _HasBottle(string age)
        {
            if (HasItem("Bottle"))
            {
                return true;
            }

            if (HasItem("Rutos_Letter") && IsLocationAvailable("Deliver Rutos Letter"))
            {
                return true;
            }

            return false;
        }

        Func<string, string, bool> CanPlay;
        bool _CanPlay(string songName, string age)
        {
            if (!HasItem("Ocarina"))
            {
                return false;
            }
            if (songName == "Scarecrow_Song")
            {
                if (age != "adult")
                {
                    return false;
                }
                if (SeedSettings.FreeScarecrow)
                {
                    return true;
                }
                return HasAllNotesForSong(songName) && IsLocationAvailable("Pierre", "adult");
            }
            return HasItem(songName) && HasAllNotesForSong(songName);
        }

        private static ISet<string> CHILD_ITEMS = new HashSet<string>()
        {
            "Slingshot", "Boomerang", "Kokiri_Sword", "Sticks", "Deku_Shield"
        };
        private static ISet<string> ADULT_ITEMS = new HashSet<string>()
        {
            "Bow",
            "Megaton_Hammer",
            "Iron_Boots",
            "Hover_Boots",
            "Hookshot",
            "Longshot",
            "Silver_Gauntlets",
            "Golden_Gauntlets",
            "Goron_Tunic",
            "Zora_Tunic",
            "Mirror_Shield",
        };
        private static ISet<string> MAGIC_ITEMS = new HashSet<string>()
        {
            "Dins_Fire", "Farores_Wind", "Nayrus_Love", "Lens_of_Truth"
        };
        private static ISet<string> MAGIC_ARROWS = new HashSet<string>()
        {
            "Fire_Arrows", "Light_Arrows", "Ice_Arrows"
        };

        Func<string, string?, bool> CanUse;
        bool _CanUse(string itemName, string? age)
        {
            if (itemName == "Scarecrow")
            {
                return HasItem("Progressive_Hookshot") &&
                       CanPlay("Scarecrow_Song", age);
            }
            if (itemName == "Distant_Scarecrow")
            {
                return HasItem("Progressive_Hookshot", 2) &&
                       CanPlay("Scarecrow_Song", age);
            }
            if (SeedSettings.BlueFireArrows && itemName == "Blue_Fire_Arrows")
            {
                itemName = "Ice_Arrows";
            }

            if (RuleAliases.ContainsKey(itemName) && !EvalRuleAlias(itemName, age))
            {
                return false;
            }

            if (!HasItem(itemName) &&
                (!RuleAliases.ContainsKey(itemName) || !EvalRuleAlias(itemName, age)))
            {

                return false;
            }

            var isChildItem = CHILD_ITEMS.Contains(itemName);
            var isAdultItem = ADULT_ITEMS.Contains(itemName);
            var isMagicItem = MAGIC_ITEMS.Contains(itemName);
            var isMagicArrows = MAGIC_ARROWS.Contains(itemName);

            if (isChildItem)
            {
                return age == null || EvalIdentifier("is_child", age);
            }
            if (isAdultItem)
            {
                return age == null || EvalIdentifier("is_adult", age);
            }
            if (isMagicItem)
            {
                return HasItem("Magic_Meter");
            }
            if (isMagicArrows)
            {
                return HasItem("Magic_Meter") && CanUse("Bow", age);
            }
            
            throw new NotImplementedException();
        }

        Func<string, string, string, string, bool> EvalBinaryComparison;
        bool _EvalBinaryComparison(string left, string right, string op, string age)
        {
            left = left.Replace("'", "");
            right = right.Replace("'", "");
            switch (op)
            {
                case "==":
                    if (SeedSettings.ContainsKey(left))
                    {
                        return SeedSettings.IsSettingEqual(left, right);
                    }
                    else if (left == "selected_adult_trade_item")
                    {
                        return HasItem(right);
                    }
                    else if (left == "age" && right == "starting_age")
                    {
                        return true;
                    }
                    break;
                case "!=":
                    return !EvalBinaryComparison(left, right, "==", age);
                case "<":
                    if (SeedSettings.ContainsKey(left))
                    {
                        return SeedSettings.GetSetting<int>(left) < int.Parse(right);
                    }
                    break;
                case "in":
                    if (SeedSettings.ContainsKey(right))
                    {
                        return SeedSettings.SettingHas(right, left);
                    }
                    break;
            }
            throw new NotImplementedException($"Unable to eval BinaryExpression: {left} {op} {right}");
        }

        Func<string, string, bool> EvalCountCheck;
        bool _EvalCountCheck(string left, string right)
        {
            if (SeedSettings.ContainsKey(right))
            {
                return HasItem(left, SeedSettings.GetSetting<int>(right));
            }

            var itemName = left;
            var itemCount = int.Parse(right);

            return HasItem(itemName, itemCount);
        }

        Func<string, int, bool> HasItemMemoized;
        bool HasItem(string itemName, int count = 1)
        {
            return HasItemMemoized(itemName, count);
        }
        bool _HasItem(string itemName, int count)
        {
            if (!SeedSettings.ShuffleOcarinaNotes && itemName.StartsWith("Ocarina_"))
            {
                return true;
            }

            if (!SeedSettings.ShuffleGerudoCard && itemName == "Gerudo_Membership_Card")
            {
                return HasItemCountRaw(itemName, 1) || IsLocationAvailable("Hideout Gerudo Membership Card");
            }

            if (!SeedSettings.ShuffleBeans && itemName == "Magic_Bean")
            {
                return IsLocationAvailable("ZR Magic Bean Salesman");
            }

            if (itemName.StartsWith("Small_Key_"))
            {
                // account for removed locked door in Fire Temple when keysanity is off
                if (!SeedSettings.ShuffleSmallKeys.DungeonItemShuffleEnabled() && itemName == "Small_Key_Fire_Temple")
                {
                    count--;
                }

                if (HasItemCountRaw(itemName, count))
                {
                    return true;
                }
                if (itemName.StartsWith("Small_Key_Treasure_Chest_Game"))
                {
                    if (SeedSettings.ShuffleTreasureChestGameKeys == "vanilla")
                    {
                        return CanUse("Lens_of_Truth", null);
                    }
                }
                else
                {

                    // if Small Keys mode is Keysy, ignore small key requirements
                    if (SeedSettings.ShuffleSmallKeys == "remove")
                    {
                        return true;
                    }
                }
            }

            if (itemName.StartsWith("Boss_Key_"))
            {
                if (HasItemCountRaw(itemName, count))
                {
                    return true;
                }
                if (SeedSettings.KeyRingsGiveBK)
                {
                    return HasItem(itemName.Replace("Boss_", "Small_"), 1);
                }
                if (itemName == "Boss_Key_Ganons_Castle")
                {
                    if (SeedSettings.ShuffleGanonsBK == "vanilla")
                    {
                        return IsLocationAvailable("Ganons Tower Boss Key Chest");
                    }
                    return SeedSettings.ShuffleGanonsBK == "remove" || SeedSettings.ShuffleGanonsBK == "dungeon";
                }
                else
                {
                    if (SeedSettings.ShuffleBossKeys == "vanilla")
                    {
                        var dungeon = itemName.Replace("Boss_Key_", "").Replace("_", " ");

                        return IsLocationAvailable($"{dungeon} Boss Key Chest");
                    }
                    return SeedSettings.ShuffleBossKeys == "remove" || SeedSettings.ShuffleBossKeys == "dungeon";
                }
            }

            // if Silver Rupee shuffle is off, ignore small key requirements
            if (SeedSettings.ShuffleSilverRupees == "vanilla" && itemName.StartsWith("Silver_Rupee_"))
            {
                if (HasItemCountRaw(itemName, count))
                {
                    return true;
                }
            }

            if (itemName == "Bottle_with_Big_Poe")
            {
                count = SeedSettings.BigPoeRandomCount ? 10 : SeedSettings.BigPoeCount;
            }

            return HasItemCountRaw(itemName, count);
        }

        private bool HasItemCountRaw(string itemName, int count)
        {
            if (Items.ContainsKey(itemName) && Items[itemName] >= count)
            {
                return true;
            }
            if (!itemName.StartsWith(SyntheticTag))
            {
                return HasItemCountRaw(Synthetic(itemName), count);
            }
            return false;
        }

        Func<string, int, bool, bool> HasAvailableDungeonKeysMemoized;
        bool HasAvailableDungeonKeys(string dungeon, int count)
        {
            return HasAvailableDungeonKeysMemoized(dungeon, count, true);
        }
        private bool _HasAvailableDungeonKeys(string dungeon, int count)
        {
            var accessibleDungeonRegions = Regions.Values
                .SelectMany(regionLocs => regionLocs.ToList())
                .Where(region => Locations.RegionMap[region] == dungeon)
                .ToHashSet();

            var dungeonLocations = Locations.ActiveLocations.Values.Where(loc => accessibleDungeonRegions.Contains(loc.ParentRegion)).ToList();

            var availableLocations = dungeonLocations
                .Where(loc =>
                {
                    try
                    {
                        return Locations.IsProgessLocation(loc) && IsLocationAvailable(loc.LocationName);
                    }
                    catch (InvalidOperationException e)
                    {
                        return false;
                    }
                })
                .Take(count)
                .Count();
            return availableLocations >= count;
        }

        private static ISet<string> MEDALLIONS = new HashSet<string>
        {
            "Light_Medallion",
            "Forest_Medallion",
            "Fire_Medallion",
            "Water_Medallion",
            "Shadow_Medallion",
            "Spirit_Medallion",
        };
        Func<string, bool> HasMedallions;
        bool _HasMedallions(string conditionSetting)
        {
            if (!SeedSettings.ContainsKey(conditionSetting))
            {
                throw new NotImplementedException($"has_medallions({conditionSetting})");
            }

            int requiredCount = SeedSettings.GetSetting<int>(conditionSetting);

            int actualCount = MEDALLIONS.Where(med => HasItem(med)).Count();

            return actualCount >= requiredCount;
        }

        private static ISet<string> STONES = new HashSet<string>
        {
            "Kokiri_Emerald",
            "Goron_Ruby",
            "Zora_Sapphire",
        };
        Func<string, bool> HasStones;
        bool _HasStones(string conditionSetting)
        {
            if (!SeedSettings.ContainsKey(conditionSetting))
            {
                throw new NotImplementedException($"has_stones({conditionSetting})");
            }

            int requiredCount = SeedSettings.GetSetting<int>(conditionSetting);

            int actualCount = STONES.Where(stone => HasItem(stone)).Count();

            return actualCount >= requiredCount;
        }

        Func<string, bool> HasDungeonRewards;
        bool _HasDungeonRewards(string conditionSetting)
        {
            if (!SeedSettings.ContainsKey(conditionSetting))
            {
                throw new NotImplementedException($"has_stones({conditionSetting})");
            }

            int requiredCount = SeedSettings.GetSetting<int>(conditionSetting);

            int actualCount = MEDALLIONS.Union(STONES).Where(stone => HasItem(stone)).Count();

            return actualCount >= requiredCount;
        }

        Func<string, bool> HasHearts;
        bool _HasHearts(string conditionSetting)
        {
            //if (!SeedSettings.ContainsKey(conditionSetting))
            //{
            //    throw new NotImplementedException($"has_hearts({conditionSetting})");
            //}

            //int requiredCount = SeedSettings.GetSetting<int>(conditionSetting);

            //return HasItem("Hearts", requiredCount);
            // TODO: figure out health tracking.
            return true;
        }

        Func<string, bool> HasProjectile;
        bool _HasProjectile(string forAge)
        {
            var canChildProjectile = HasItem("Slingshot") || HasItem("Boomerang");
            var canAdultProjectile = HasItem("Bow") || HasItem("Progressive_Hookshot");

            bool hasProjectile = false;
            switch (forAge)
            {
                case "child":
                    hasProjectile = canChildProjectile;
                    break;
                case "adult":
                    hasProjectile = canAdultProjectile;
                    break;
                case "both":
                    hasProjectile = canChildProjectile && canAdultProjectile;
                    break;
                case "either":
                    hasProjectile = canChildProjectile || canAdultProjectile;
                    break;
                default:
                    throw new NotImplementedException($"has_projectile({forAge})");
            }

            if (hasProjectile)
            {
                return true;
            }

            return EvalRuleAlias("has_explosives", "child") || EvalRuleAlias("has_explosives", "adult");
        }

        Func<string, bool> RegionHasShortcuts;
        bool _RegionHasShortcuts(string regionName)
        {
            if (!Locations.RegionMap.ContainsKey(regionName))
            {
                throw new NotImplementedException($"region_has_shortcuts({regionName})");
            }

            return SeedSettings.DungeonShortcuts.Contains(regionName);
        }

        Func<string, bool> HasAllNotesForSong;
        bool _HasAllNotesForSong(string name)
        {
            if (!SeedSettings.ShuffleOcarinaNotes)
            {
                return true;
            }
            // TODO: Figure out how to track custom song notes
            switch (name)
            {
                case "Minuet_of_Forest":
                    return (
                        HasItem("Ocarina_A_Button") &&
                        HasItem("Ocarina_C_up_Button") &&
                        HasItem("Ocarina_C_left_Button") &&
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Bolero_of_Fire":
                    return (
                        HasItem("Ocarina_A_Button") &&
                        HasItem("Ocarina_C_down_Button") &&
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Serenade_of_Water":
                    return (
                        HasItem("Ocarina_A_Button") &&
                        HasItem("Ocarina_C_down_Button") &&
                        HasItem("Ocarina_C_left_Button") &&
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Requiem_of_Spirit":
                    return (
                        HasItem("Ocarina_A_Button") &&
                        HasItem("Ocarina_C_down_Button") &&
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Nocturne_of_Shadow":
                    return (
                        HasItem("Ocarina_A_Button") &&
                        HasItem("Ocarina_C_down_Button") &&
                        HasItem("Ocarina_C_left_Button") &&
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Prelude_of_Light":
                    return (
                        HasItem("Ocarina_C_up_Button") &&
                        HasItem("Ocarina_C_left_Button") &&
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Zeldas_Lullaby":
                    return (
                        HasItem("Ocarina_C_up_Button") &&
                        HasItem("Ocarina_C_left_Button") &&
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Eponas_Song":
                    return (
                        HasItem("Ocarina_C_up_Button") &&
                        HasItem("Ocarina_C_left_Button") &&
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Sarias_Song":
                    return (
                        HasItem("Ocarina_C_down_Button") &&
                        HasItem("Ocarina_C_left_Button") &&
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Suns_Song":
                    return (
                        HasItem("Ocarina_C_up_Button") &&
                        HasItem("Ocarina_C_down_Button") &&
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Song_of_Time":
                    return (
                        HasItem("Ocarina_A_Button") &&
                        HasItem("Ocarina_C_down_Button") &&
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Song_of_Storms":
                    return (
                        HasItem("Ocarina_A_Button") &&
                        HasItem("Ocarina_C_up_Button") &&
                        HasItem("Ocarina_C_down_Button")
                    );
                case "Scarecrow_Song":
                    return new bool[]
                    {
                        HasItem("Ocarina_A_Button") &&
                        HasItem("Ocarina_C_up_Button") &&
                        HasItem("Ocarina_C_down_Button") &&
                        HasItem("Ocarina_C_left_Button") &&
                        HasItem("Ocarina_C_right_Button")
                    }.Where(v => v).Count() >= 2;
                default:
                    throw new NotImplementedException($"has_all_notes_for_song({name})");
            }
        }
    }

}
