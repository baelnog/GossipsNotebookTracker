using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ChecklistTracker.ANTLR;
using ChecklistTracker.Config;
using ChecklistTracker.Config.SettingsTypes;
using ChecklistTracker.CoreUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Access = ChecklistTracker.LogicProvider.AccessibilityExtensions;

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
            _IsLocationAvailableMemoized = Cache.Memoize<string, string?, Accessibility>("IsLocationAvailable", _IsLocationAvailable);
            _EvalNodeMemoized = Cache.Memoize<IParseTree, string, string, Accessibility>("EvalNode", _EvalNode, onReentrance: () => Accessibility.None);
            EvalAccessRuleAge = Cache.Memoize<LocationsData.AccessRule, string, Accessibility>("EvalAccessRuleAge", _EvalAccessRuleAge);
            EvalIdentifier = Cache.Memoize<string, string?, Accessibility>("EvalIdentifier", _EvalIdentifier);
            EvalEvent = Cache.Memoize<string, Accessibility>("EvalEvent", _EvalEvent);
            EvalCall = Cache.Memoize<string, Python3Parser.ArgumentContext[], Accessibility>("EvalCall", _EvalCall);
            EvalLookup = Cache.Memoize<string, Python3Parser.Subscript_Context[], Accessibility>("EvalLookup", _EvalLookup);
            EvalRuleAlias = Cache.Memoize<string, string?, Accessibility>("EvalRuleAlias", _EvalRuleAlias);
            CanBuy = Cache.Memoize<string, string?, Accessibility>("CanBuy", _CanBuy);
            CanAccessDrop = Cache.Memoize<string, string?, Accessibility>("CanAccessDrop", _CanAccessDrop);
            HasBottle = Cache.Memoize<string?, Accessibility>("HasBottle", _HasBottle);
            CanPlay = Cache.Memoize<string, string?, Accessibility>("CanPlay", _CanPlay);
            CanUse = Cache.Memoize<string, string?, Accessibility>("CanUse", _CanUse);
            EvalBinaryComparison = Cache.Memoize<string, string, string, string, Accessibility>("EvalBinaryComparison", _EvalBinaryComparison);
            EvalCountCheck = Cache.Memoize<string, string, Accessibility>("EvalCountCheck", _EvalCountCheck);
            HasItemMemoized = Cache.Memoize<string, int, Accessibility>("HasItem", _HasItem);
            HasMedallions = Cache.Memoize<string, Accessibility>("HasMedallions", _HasMedallions);
            HasStones = Cache.Memoize<string, Accessibility>("HasStones", _HasStones);
            HasDungeonRewards = Cache.Memoize<string, Accessibility>("HasDungeonRewards", _HasDungeonRewards);
            HasHearts = Cache.Memoize<string, Accessibility>("HasHearts", _HasHearts);
            HasProjectile = Cache.Memoize<string, Accessibility>("HasProjectile", _HasProjectile);
            RegionHasShortcuts = Cache.Memoize<string, Accessibility>("RegionHasShortcuts", _RegionHasShortcuts);
            HasAllNotesForSong = Cache.Memoize<string, Accessibility>("HasAllNotesForSong", _HasAllNotesForSong);
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


        Func<IParseTree, string, string, bool, Accessibility> _EvalNodeMemoized;
        public Accessibility EvalNode(IParseTree tree, string here, string age, bool allowReentrance = false)
        {
            return _EvalNodeMemoized(tree, here, age, allowReentrance);
        }

        public Accessibility _EvalNode(IParseTree tree, string here, string age)
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

        private Accessibility EvalAdultTradeItem(string name, string item, string checkName)
        {
            var conditions = new List<Func<Accessibility>>
            {
                () => HasItem(name)
            };

            if (!SeedSettings.FullAdultTradeShuffle || !SeedSettings.AdultTradeItemStartLogic.Contains(item))
            {
                conditions.Add(() => IsLocationAvailable(checkName));
            }

            return conditions.Or();
        }

        Func<string, string?, Accessibility> EvalIdentifier;
        Accessibility _EvalIdentifier(string name, string? age)
        {
            using var indent = Logging.Indented();
            Logging.WriteLine($"EvalIdentifier({name}, {age})");
            switch (name)
            {
                case "True":
                    return true.ToAccessibility();
                case "False":
                    return false.ToAccessibility();
                case "peek":
                    return Accessibility.Peekable;
                case "accessible":
                    return Accessibility.SequenceBreak;
                case "chest_appearance":
                    return (SeedSettings.CAMC != "off").ToAccessibility();
                case "chest_size":
                    return (SeedSettings.CAMC == "both" || SeedSettings.CAMC == "classic").ToAccessibility();
                case "had_night_start":
                    return this.SeedSettings.StartingTimeOfDay.IsNight().ToAccessibility();
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
                    return (age == "child").ToAccessibility();
                case "is_adult":
                    return (age == "adult").ToAccessibility();
                case "Zeldas_Letter":
                    return Access.Or(
                        () => SeedSettings.SkipChildZelda().ToAccessibility(),
                        () => HasItem(name),
                        () => IsLocationAvailable("HC Zeldas Letter")
                    );
                case "Keaton_Mask":
                    return Access.Or(
                        () => HasItem(name),
                        () => IsLocationAvailable("Market Mask Shop Item 6")
                    );
                case "Skull_Mask":
                    return Access.Or(
                        () => HasItem(name),
                        () => IsLocationAvailable("Market Mask Shop Item 5")
                    );
                case "Spooky_Mask":
                    return Access.Or(
                        () => HasItem(name),
                        () => IsLocationAvailable("Market Mask Shop Item 8")
                    );
                case "Bunny_Mask":
                    return Access.Or(
                        () => HasItem(name),
                        () => IsLocationAvailable("Market Mask Shop Item 7")
                    );
                case "Mask_of_Truth":
                    return Access.Or(
                        () => HasItem(name),
                        () => IsLocationAvailable("Market Mask Shop Item 3")
                    );
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
                return ((bool)RenamedAttributes[name]).ToAccessibility();
            }

            if (SeedSettings.ContainsKey(name))
            {
                Logging.WriteLine($"({name}, {age}) -> Setting enabed [{SeedSettings.IsEnabled(name)}]");
                return SeedSettings.IsEnabled(name).ToAccessibility();
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
                return SeedSettings.EnabledTricks.Contains(name) ? Accessibility.All : Accessibility.SequenceBreak;
            }

            // Assume access to time passage is always available.
            if (name.StartsWith("at_"))
            {
                return true.ToAccessibility();
            }

            if (name.StartsWith("Buy_"))
            {
                return CanBuy(name, age);
            }

            var dropId = unquotedName.Replace(" ", "_");
            if (Locations.ActiveDropLocationsByItem.ContainsKey(dropId))
            {
                return CanAccessDrop(dropId, age);
            }

            throw new NotImplementedException($"Unknown identifier: {name}");
        }
        Func<string, Python3Parser.Subscript_Context[], Accessibility> EvalLookup;
        Accessibility _EvalLookup(string name, Python3Parser.Subscript_Context[] args)
        {
            switch (name)
            {
                case "skipped_trials":
                    return (SeedSettings.TrialsCount == 0 || SeedSettings.TrialsRandomCount).ToAccessibility();
            }

            throw new NotImplementedException($"Unknown function: {name}");
        }

        Func<string, Python3Parser.ArgumentContext[], Accessibility> EvalCall;
        Accessibility _EvalCall(string name, Python3Parser.ArgumentContext[] args)
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

        Func<string, string?, Accessibility> EvalRuleAlias;
        Accessibility _EvalRuleAlias(string name, string? age)
        {
            using var indent = Logging.Indented();

            Logging.WriteLine($"EvalRuleAlias({name}, {age})");
            return RuleAliases[name].Accept(this);
        }

        public Func<string, Accessibility> EvalEvent;
        Accessibility _EvalEvent(string name)
        {
            using var indent = Logging.Indented();
            var escaped = name.Replace("_", " ").Replace("'", "");
            if (escaped != name)
            {
                return EvalEvent(escaped);
            }
            if (!Locations.ActiveEvents.ContainsKey(name))
            {
                return false.ToAccessibility();
            }
            if (name == "Eyeball Frog Access")
            {
                if (SeedSettings.FullAdultTradeShuffle)
                {
                    return HasItem("Eyeball_Frog");
                }

                var potentialItems = new List<Func<Accessibility>>()
                {
                    () => HasItem("Prescription")
                };

                if ((bool)RenamedAttributes["disable_trade_revert"])
                {
                    potentialItems.Add(() => HasItem("Eyedrops"));
                    potentialItems.Add(() => HasItem("Eyeball_Frog"));
                }

                if (SeedSettings.AdultTradeItemStart.Contains(AdultTradeItem.BrokenSword))
                {
                    potentialItems.Add(() => EvalEvent("Prescription Access"));
                }

                return Access.And(
                    () => EvalEvent("King Zora Thawed"),
                    () => potentialItems.Or()
                );
            }

            return Locations.ActiveEvents[name].AccessRules.Or(EvalAccessRuleAnyAge);
        }

        Accessibility AtNight(string age)
        {
            // Hardcoded for now.
            return Accessibility.All;
        }

        Accessibility At(string spotToCheck, ParserRuleContext arg1, string age)
        {
            return Access.Or(
                () => Access.And(() => IsRegionAccessible(spotToCheck, "child"), () => EvalNode(arg1, spotToCheck, "child")),
                () => Access.And(() => IsRegionAccessible(spotToCheck, "adult"), () => EvalNode(arg1, spotToCheck, "adult"))
            );
        }

        Accessibility Here(Python3Parser.ArgumentContext[] args)
        {
            return Access.Or(
                () => At(VisitingHere.Peek(), args[0], "child"),
                () => At(VisitingHere.Peek(), args[0], "adult")
            );
        }

        Func<string, string?, Accessibility> CanBuy;
        Accessibility _CanBuy(string itemName, string? age)
        {

            var conditions = new List<Func<Accessibility>>();
            if (new HashSet<string> { "Buy_Arrows_50", "Buy_Fish", "Buy_Goron_Tunic", "Buy_Bombchu_20", "Buy_Bombs_30" }.Contains(itemName))
            {
                conditions.Add(() => HasItem("Progressive_Wallet"));
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
                    conditions.Add(() => Access.Or(
                        () => HasItem("Bombchus"),
                        () => HasItem("Bombchus_5"),
                        () => HasItem("Bombchus_10"),
                        () => HasItem("Bombchus_20")
                    ));
                }
                else
                {
                    conditions.Add(() => HasItem("Bomb_Bag"));
                }
            }
            if (itemName == "Buy_Zora_Tunic" || itemName == "Buy_Blue_Fire")
            {
                if (age != "adult")
                {
                    return Accessibility.None;
                }

                conditions.Add(() => HasItem("Progressive_Wallet", 2));
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
                conditions.Add(() => HasBottle(age));
            }

            return conditions.And();
        }

        Func<string, string?, Accessibility> CanAccessDrop;
        Accessibility _CanAccessDrop(string dropName, string? age)
        {
            using var indent = Logging.Indented();
            Logging.WriteLine($"CanAccessDrop({dropName}, {age})");

            return Locations.ActiveDropLocationsByItem[dropName].AccessRules.Or(EvalAccessRuleAnyAge);
        }

        Func<string?, Accessibility> HasBottle;
        Accessibility _HasBottle(string? age)
        {
            return HasItem("Bottle") |
                   HasItem("Rutos_Letter") & IsLocationAvailable("Deliver Rutos Letter");
        }

        Func<string, string?, Accessibility> CanPlay;
        Accessibility _CanPlay(string songName, string? age)
        {
            var hasOcarina = HasItem("Ocarina");
            if (songName == "Scarecrow_Song")
            {
                if (age != "adult")
                {
                    return Accessibility.None;
                }
                if (SeedSettings.FreeScarecrow)
                {
                    return hasOcarina;
                }
                return hasOcarina & HasAllNotesForSong(songName) & IsLocationAvailable("Pierre", "adult");
            }
            return hasOcarina & HasItem(songName) & HasAllNotesForSong(songName);
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

        Func<string, string?, Accessibility> CanUse;
        Accessibility _CanUse(string itemName, string? age)
        {
            if (itemName == "Scarecrow")
            {
                return Access.And(
                    () => HasItem("Progressive_Hookshot"),
                    () => CanPlay("Scarecrow_Song", age)
                );
            }
            if (itemName == "Distant_Scarecrow")
            {
                return Access.And(
                    () => HasItem("Progressive_Hookshot", 2),
                    () => CanPlay("Scarecrow_Song", age)
                );
            }
            if (SeedSettings.BlueFireArrows && itemName == "Blue_Fire_Arrows")
            {
                itemName = "Ice_Arrows";
            }

            var conditions = new List<Func<Accessibility>>();
            if (RuleAliases.ContainsKey(itemName))
            {
                conditions.Add(() => EvalRuleAlias(itemName, age));
            }
            else
            {
                conditions.Add(() => HasItem(itemName));
            }

            var isChildItem = CHILD_ITEMS.Contains(itemName);
            var isAdultItem = ADULT_ITEMS.Contains(itemName);
            var isMagicItem = MAGIC_ITEMS.Contains(itemName);
            var isMagicArrows = MAGIC_ARROWS.Contains(itemName);

            if (isChildItem)
            {
                conditions.Add(() => Access.Or(
                    () => (age == null).ToAccessibility(),
                    () => EvalIdentifier("is_child", age))
                );
            }
            else if (isAdultItem)
            {
                conditions.Add(() => Access.Or(
                    () => (age == null).ToAccessibility(),
                    () => EvalIdentifier("is_adult", age))
                );
            }
            else if (isMagicItem)
            {
                conditions.Add(() => HasItem("Magic_Meter"));
            }
            else if (isMagicArrows)
            {
                conditions.Add(() => HasItem("Magic_Meter"));
                conditions.Add(() => CanUse("Bow", age));
            }
            else
            {
                throw new NotImplementedException();
            }
            return conditions.And();
        }

        Func<string, string, string, string, Accessibility> EvalBinaryComparison;
        Accessibility _EvalBinaryComparison(string left, string right, string op, string age)
        {
            left = left.Replace("'", "");
            right = right.Replace("'", "");

            switch (op)
            {
                case "==":
                    if (SeedSettings.ContainsKey(left))
                    {
                        return SeedSettings.IsSettingEqual(left, right).ToAccessibility();
                    }
                    else if (left == "selected_adult_trade_item")
                    {
                        return HasItem(right);
                    }
                    else if (left == "age" && right == "starting_age")
                    {
                        return true.ToAccessibility();
                    }
                    break;
                case "!=":
                    var result = EvalBinaryComparison(left, right, "==", age);
                    if (result.HasFlag(Accessibility.InLogic))
                    {
                        return Accessibility.None;
                    }
                    else
                    {
                        return Accessibility.All;
                    }
                case "<":
                    if (SeedSettings.ContainsKey(left))
                    {
                        return (SeedSettings.GetSetting<int>(left) < int.Parse(right)).ToAccessibility();
                    }
                    break;
                case "in":
                    if (SeedSettings.ContainsKey(right))
                    {
                        return SeedSettings.SettingHas(right, left).ToAccessibility();
                    }
                    break;
            }
            throw new NotImplementedException($"Unable to eval BinaryExpression: {left} {op} {right}");
        }

        Func<string, string, Accessibility> EvalCountCheck;
        Accessibility _EvalCountCheck(string left, string right)
        {
            if (SeedSettings.ContainsKey(right))
            {
                return HasItem(left, SeedSettings.GetSetting<int>(right));
            }

            var itemName = left;
            var itemCount = int.Parse(right);

            return HasItem(itemName, itemCount);
        }

        Func<string, int, Accessibility> HasItemMemoized;
        Accessibility HasItem(string itemName, int count = 1)
        {
            return HasItemMemoized(itemName, count);
        }
        Accessibility _HasItem(string itemName, int count)
        {
            if (!SeedSettings.ShuffleOcarinaNotes && itemName.StartsWith("Ocarina_"))
            {
                return Accessibility.All;
            }

            if (!SeedSettings.ShuffleGerudoCard && itemName == "Gerudo_Membership_Card")
            {
                return Access.Or(() => HasItemCountRaw(itemName, 1), () => IsLocationAvailable("Hideout Gerudo Membership Card"));
            }

            if (!SeedSettings.ShuffleBeans && itemName == "Magic_Bean")
            {
                return IsLocationAvailable("ZR Magic Bean Salesman");
            }

            var conditions = new List<Func<Accessibility>>
            {
                () => HasItemCountRaw(itemName, count)
            };

            if (itemName.StartsWith("Small_Key_"))
            {
                // account for removed locked door in Fire Temple when keysanity is off
                if (!SeedSettings.ShuffleSmallKeys.IsShuffled() && itemName == "Small_Key_Fire_Temple")
                {
                    count--;
                }

                if (itemName.StartsWith("Small_Key_Treasure_Chest_Game"))
                {
                    if (!SeedSettings.ShuffleTreasureChestGameKeys.IsShuffled())
                    {
                        conditions.Add(() => CanUse("Lens_of_Truth", null));
                    }
                }
                // if Small Keys mode is Keysy, ignore small key requirements
                else if (SeedSettings.ShuffleSmallKeys == ShuffleDungeonItemType.Remove)
                {
                    return Accessibility.All;
                }
            }

            if (itemName.StartsWith("Boss_Key_"))
            {
                if (SeedSettings.KeyRingsGiveBK)
                {
                    conditions.Add(() => HasItem(itemName.Replace("Boss_", "Small_"), 1));
                }
                if (itemName == "Boss_Key_Ganons_Castle")
                {
                    if (SeedSettings.ShuffleGanonsBK == ShuffleGanonsBKType.Vanilla)
                    {
                        conditions.Add(() => IsLocationAvailable("Ganons Tower Boss Key Chest"));
                    }
                    else if (SeedSettings.ShuffleGanonsBK == ShuffleGanonsBKType.Remove ||
                             SeedSettings.ShuffleGanonsBK == ShuffleGanonsBKType.OwnDungeon)
                    {
                        return Accessibility.All;
                    }
                }
                else
                {
                    if (SeedSettings.ShuffleBossKeys == ShuffleDungeonItemType.Vanilla)
                    {
                        var dungeon = itemName.Replace("Boss_Key_", "").Replace("_", " ");
                        conditions.Add(() => IsLocationAvailable($"{dungeon} Boss Key Chest"));
                    }
                    else if (SeedSettings.ShuffleBossKeys == ShuffleDungeonItemType.Remove ||
                             SeedSettings.ShuffleBossKeys == ShuffleDungeonItemType.OwnDungeon)
                    {
                        return Accessibility.All;
                    }
                }
            }

            if (itemName == "Bottle_with_Big_Poe")
            {
                count = SeedSettings.BigPoeRandomCount ? 10 : SeedSettings.BigPoeCount;
            }

            conditions.Add(() => HasItemCountRaw(itemName, count));

            return conditions.Or();
        }

        private Accessibility HasItemCountRaw(string itemName, int count)
        {
            if (Items.ContainsKey(itemName) && Items[itemName] >= count)
            {
                return Accessibility.All;
            }
            if (!itemName.StartsWith(SyntheticTag))
            {
                return HasItemCountRaw(Synthetic(itemName), count) & Accessibility.SyntheticAssumed;
            }
            return Accessibility.None;
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
        Func<string, Accessibility> HasMedallions;
        Accessibility _HasMedallions(string conditionSetting)
        {
            if (!SeedSettings.ContainsKey(conditionSetting))
            {
                throw new NotImplementedException($"has_medallions({conditionSetting})");
            }

            int requiredCount = SeedSettings.GetSetting<int>(conditionSetting);

            return MEDALLIONS.MostAccessible(r => HasItem(r), requiredCount);
        }

        private static ISet<string> STONES = new HashSet<string>
        {
            "Kokiri_Emerald",
            "Goron_Ruby",
            "Zora_Sapphire",
        };
        Func<string, Accessibility> HasStones;
        Accessibility _HasStones(string conditionSetting)
        {
            if (!SeedSettings.ContainsKey(conditionSetting))
            {
                throw new NotImplementedException($"has_stones({conditionSetting})");
            }

            int requiredCount = SeedSettings.GetSetting<int>(conditionSetting);

            return STONES.MostAccessible(r => HasItem(r), requiredCount);
        }

        Func<string, Accessibility> HasDungeonRewards;
        Accessibility _HasDungeonRewards(string conditionSetting)
        {
            if (!SeedSettings.ContainsKey(conditionSetting))
            {
                throw new NotImplementedException($"has_stones({conditionSetting})");
            }

            int requiredCount = SeedSettings.GetSetting<int>(conditionSetting);

            return MEDALLIONS.Union(STONES).MostAccessible(r => HasItem(r), requiredCount);
        }

        Func<string, Accessibility> HasHearts;
        Accessibility _HasHearts(string conditionSetting)
        {
            //if (!SeedSettings.ContainsKey(conditionSetting))
            //{
            //    throw new NotImplementedException($"has_hearts({conditionSetting})");
            //}

            //int requiredCount = SeedSettings.GetSetting<int>(conditionSetting);

            //return HasItem("Hearts", requiredCount);
            // TODO: figure out health tracking.
            return Accessibility.All;
        }

        Func<string, Accessibility> HasProjectile;
        Accessibility _HasProjectile(string forAge)
        {
            var canChildProjectile = HasItem("Slingshot") | HasItem("Boomerang");
            var canAdultProjectile = HasItem("Bow") | HasItem("Progressive_Hookshot");

            Accessibility hasProjectile;
            switch (forAge)
            {
                case "child":
                    hasProjectile = canChildProjectile;
                    break;
                case "adult":
                    hasProjectile = canAdultProjectile;
                    break;
                case "both":
                    hasProjectile = canChildProjectile & canAdultProjectile;
                    break;
                case "either":
                    hasProjectile = canChildProjectile | canAdultProjectile;
                    break;
                default:
                    throw new NotImplementedException($"has_projectile({forAge})");
            }

            return hasProjectile |
                    EvalRuleAlias("has_explosives", "child") |
                    EvalRuleAlias("has_explosives", "adult");
        }

        Func<string, Accessibility> RegionHasShortcuts;
        Accessibility _RegionHasShortcuts(string regionName)
        {
            if (!Locations.RegionMap.ContainsKey(regionName.Replace("'", "")))
            {
                throw new NotImplementedException($"region_has_shortcuts({regionName})");
            }

            return SeedSettings.DungeonShortcuts.Contains(regionName).ToAccessibility();
        }

        Func<string, Accessibility> HasAllNotesForSong;
        Accessibility _HasAllNotesForSong(string name)
        {
            if (!SeedSettings.ShuffleOcarinaNotes)
            {
                return Accessibility.All;
            }
            // TODO: Figure out how to track custom song notes
            switch (name)
            {
                case "Minuet_of_Forest":
                    return (
                        HasItem("Ocarina_A_Button") &
                        HasItem("Ocarina_C_up_Button") &
                        HasItem("Ocarina_C_left_Button") &
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Bolero_of_Fire":
                    return (
                        HasItem("Ocarina_A_Button") &
                        HasItem("Ocarina_C_down_Button") &
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Serenade_of_Water":
                    return (
                        HasItem("Ocarina_A_Button") &
                        HasItem("Ocarina_C_down_Button") &
                        HasItem("Ocarina_C_left_Button") &
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Requiem_of_Spirit":
                    return (
                        HasItem("Ocarina_A_Button") &
                        HasItem("Ocarina_C_down_Button") &
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Nocturne_of_Shadow":
                    return (
                        HasItem("Ocarina_A_Button") &
                        HasItem("Ocarina_C_down_Button") &
                        HasItem("Ocarina_C_left_Button") &
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Prelude_of_Light":
                    return (
                        HasItem("Ocarina_C_up_Button") &
                        HasItem("Ocarina_C_left_Button") &
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Zeldas_Lullaby":
                    return (
                        HasItem("Ocarina_C_up_Button") &
                        HasItem("Ocarina_C_left_Button") &
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Eponas_Song":
                    return (
                        HasItem("Ocarina_C_up_Button") &
                        HasItem("Ocarina_C_left_Button") &
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Sarias_Song":
                    return (
                        HasItem("Ocarina_C_down_Button") &
                        HasItem("Ocarina_C_left_Button") &
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Suns_Song":
                    return (
                        HasItem("Ocarina_C_up_Button") &
                        HasItem("Ocarina_C_down_Button") &
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Song_of_Time":
                    return (
                        HasItem("Ocarina_A_Button") &
                        HasItem("Ocarina_C_down_Button") &
                        HasItem("Ocarina_C_right_Button")
                    );
                case "Song_of_Storms":
                    return (
                        HasItem("Ocarina_A_Button") &
                        HasItem("Ocarina_C_up_Button") &
                        HasItem("Ocarina_C_down_Button")
                    );
                case "Scarecrow_Song":
                    var buttons = new Accessibility[]
                    {
                        HasItem("Ocarina_A_Button"),
                        HasItem("Ocarina_C_up_Button"),
                        HasItem("Ocarina_C_down_Button"),
                        HasItem("Ocarina_C_left_Button"),
                        HasItem("Ocarina_C_right_Button")
                    };

                    return buttons.MostAccessible(a => a, 2);

                default:
                    throw new NotImplementedException($"has_all_notes_for_song({name})");
            }
        }
    }

}
