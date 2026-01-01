using ChecklistTracker.Config;
using System.Collections.Immutable;

namespace ChecklistTracker.LogicProvider.Test
{
    [TestClass]
    public class LogicEngineTests
    {
        [TestMethod]
        public void Season7Base_SphereZero()
        {
            var config = TrackerConfig.Init().Result;
            var testEngine = new LogicEngine(config, "v8.0");

            var expected = new HashSet<string>
                {
                    "KF Kokiri Sword Chest",
                    "KF Midos Top Left Chest",
                    "KF Midos Top Right Chest",
                    "KF Midos Bottom Left Chest",
                    "KF Midos Bottom Right Chest",
                    "LW Deku Scrub Near Bridge",
                    "LW Ocarina Memory Game",
                    "LH Child Fishing",
                    "Deku Theater Skull Mask",
                    "GV Waterfall Freestanding PoH",
                    "GV Crate Freestanding PoH",
                    "Market Shooting Gallery Reward",
                    "Market Lost Dog",
                    "Kak 10 Gold Skulltula Reward",
                    "Kak Anju as Adult",
                    "Kak Anju as Child",
                    "Kak Impas House Freestanding PoH",
                    "Kak Man on Roof",
                    "Kak Open Grotto Chest",
                    "Kak Windmill Freestanding PoH",
                    "Song from Windmill",
                    "Graveyard Dampe Gravedigging Tour",
                    "Graveyard Shield Grave Chest",
                    "Graveyard Dampe Race Hookshot Chest",
                    "Graveyard Dampe Race Freestanding PoH",
                    "DMT Freestanding PoH",
                    "LLR Freestanding PoH",
                    "LLR Talons Chickens",
                    "Song from Malon",
                    "HF Open Grotto Chest",
                    "Song from Saria",
                    "ZR Open Grotto Chest",
                }.ToHashSet();

            testEngine.CanAccess("Kak Man on Roof");

            AssertExpectedRegions(testEngine, expected);

            Assert.AreEqual(13, testEngine.GetAvailableSkulls().Count());
        }

        [TestMethod]
        public void Season7Base_Bombs()
        {
            var settings = SeedSettings.ReadFromJson(@"C:\Users\ryago\source\repos\OoTRChecklistTracker\ChecklistTracker.Config\Resources\settings\season7-base.json").Result;
            var config = TrackerConfig.Init().Result;
            var testEngine = new LogicEngine(config, "v8.0");

            var locations = testEngine.GetLocations().ToHashSet();
            var inventory = testEngine.Inventory;
            inventory["Bomb_Bag"] = 1;
            testEngine.UpdateItems(inventory);

            var expected = new HashSet<string>()
            {
                "Dodongos Cavern Map Chest",
                "Dodongos Cavern Compass Chest",
                "Dodongos Cavern Bomb Flower Platform Chest",
                "Dodongos Cavern Bomb Bag Chest",
                "Dodongos Cavern End of Bridge Chest",
                "Dodongos Cavern Boss Room Chest",
                "Dodongos Cavern King Dodongo Heart",
                "KF Kokiri Sword Chest",
                "KF Midos Top Left Chest",
                "KF Midos Top Right Chest",
                "KF Midos Bottom Left Chest",
                "KF Midos Bottom Right Chest",
                "LW Ocarina Memory Game",
                "LW Deku Scrub Near Bridge",
                "LW Near Shortcuts Grotto Chest",
                "Deku Theater Skull Mask",
                "LW Deku Scrub Grotto Front",
                "Song from Saria",
                "SFM Wolfos Grotto Chest",
                "HF Southeast Grotto Chest",
                "HF Open Grotto Chest",
                "HF Deku Scrub Grotto",
                "HF Near Market Grotto Chest",
                "LH Child Fishing",
                "GV Waterfall Freestanding PoH",
                "GV Crate Freestanding PoH",
                "Market Shooting Gallery Reward",
                "Market Bombchu Bowling First Prize",
                "Market Bombchu Bowling Second Prize",
                "Market Lost Dog",
                "Kak 10 Gold Skulltula Reward",
                "Kak 20 Gold Skulltula Reward",
                "Kak Anju as Adult",
                "Kak Anju as Child",
                "Kak Man on Roof",
                "Kak Impas House Freestanding PoH",
                "Kak Windmill Freestanding PoH",
                "Song from Windmill",
                "Kak Redead Grotto Chest",
                "Kak Open Grotto Chest",
                "Graveyard Dampe Gravedigging Tour",
                "Graveyard Shield Grave Chest",
                "Graveyard Dampe Race Hookshot Chest",
                "Graveyard Dampe Race Freestanding PoH",
                "DMT Chest",
                "DMT Freestanding PoH",
                "GC Maze Center Chest",
                "GC Maze Right Chest",
                "GC Rolling Goron as Child",
                "GC Rolling Goron as Adult",
                "DMC Wall Freestanding PoH",
                "DMC Upper Grotto Chest",
                "ZR Near Open Grotto Freestanding PoH",
                "ZR Near Domain Freestanding PoH",
                "ZR Open Grotto Chest",
                "Song from Malon",
                "LLR Talons Chickens",
                "LLR Freestanding PoH",
                "LH Adult Fishing",
                "LH Freestanding PoH",
                "Graveyard Freestanding PoH",
            };

            testEngine.CanAccess("GC Pot Freestanding PoH");

            AssertExpectedRegions(testEngine, expected);
            Assert.AreEqual(21, testEngine.GetAvailableSkulls().Count());
        }

        [TestMethod]
        public void Season7Base_Bow()
        {
            var settings = SeedSettings.ReadFromJson(@"C:\Users\ryago\source\repos\OoTRChecklistTracker\ChecklistTracker.Config\Resources\settings\season7-base.json").Result;
            var config = TrackerConfig.Init().Result;
            var testEngine = new LogicEngine(config, "v8.0");

            var inventory = testEngine.Inventory;
            inventory["Bow"] = 1;
            testEngine.UpdateItems(inventory);

            var expected = new HashSet<string>()
            {
                "KF Kokiri Sword Chest",
                "KF Midos Top Left Chest",
                "KF Midos Top Right Chest",
                "KF Midos Bottom Left Chest",
                "KF Midos Bottom Right Chest",
                "LW Ocarina Memory Game",
                "LW Deku Scrub Near Bridge",
                "Deku Theater Skull Mask",
                "Song from Saria",
                "HF Open Grotto Chest",
                "LH Child Fishing",
                "GV Waterfall Freestanding PoH",
                "GV Crate Freestanding PoH",
                "Market Shooting Gallery Reward",
                "Market Lost Dog",
                "Kak 10 Gold Skulltula Reward",
                "Kak Anju as Adult",
                "Kak Anju as Child",
                "Kak Man on Roof",
                "Kak Impas House Freestanding PoH",
                "Kak Windmill Freestanding PoH",
                "Song from Windmill",
                "Kak Shooting Gallery Reward",
                "Kak Open Grotto Chest",
                "Graveyard Dampe Gravedigging Tour",
                "Graveyard Shield Grave Chest",
                "Graveyard Dampe Race Hookshot Chest",
                "Graveyard Dampe Race Freestanding PoH",
                "DMT Freestanding PoH",
                "GC Rolling Goron as Adult",
                "DMC Wall Freestanding PoH",
                "ZR Open Grotto Chest",
                "Song from Malon",
                "LLR Talons Chickens",
                "LLR Freestanding PoH",
            };

            AssertExpectedRegions(testEngine, expected);

            //Assert.AreEqual(34, locations.Count);
            Assert.AreEqual(13, testEngine.GetAvailableSkulls().Count());
        }

        [TestMethod]
        public void Season7Base_Hookshot()
        {
            var settings = SeedSettings.ReadFromJson(@"C:\Users\ryago\source\repos\OoTRChecklistTracker\ChecklistTracker.Config\Resources\settings\season7-base.json").Result;
            var config = TrackerConfig.Init().Result;
            var testEngine = new LogicEngine(config, "v8.0");

            var inventory = testEngine.Inventory;
            inventory["Progressive_Hookshot"] = 1;
            testEngine.UpdateItems(inventory);

            var expected = new HashSet<string>()
            {
                "KF Kokiri Sword Chest",
                "KF Midos Top Left Chest",
                "KF Midos Top Right Chest",
                "KF Midos Bottom Left Chest",
                "KF Midos Bottom Right Chest",
                "LW Ocarina Memory Game",
                "LW Deku Scrub Near Bridge",
                "Deku Theater Skull Mask",
                "Song from Saria",
                "HF Open Grotto Chest",
                "LH Freestanding PoH",
                "LH Child Fishing",
                "LH Adult Fishing",
                "GV Waterfall Freestanding PoH",
                "GV Crate Freestanding PoH",
                "Market Shooting Gallery Reward",
                "Market Lost Dog",
                "Kak 10 Gold Skulltula Reward",
                "Kak Anju as Adult",
                "Kak Anju as Child",
                "Kak Man on Roof",
                "Kak Impas House Freestanding PoH",
                "Kak Windmill Freestanding PoH",
                "Song from Windmill",
                "Kak Open Grotto Chest",
                "Graveyard Dampe Gravedigging Tour",
                "Graveyard Shield Grave Chest",
                "Graveyard Dampe Race Hookshot Chest",
                "Graveyard Dampe Race Freestanding PoH",
                "DMT Freestanding PoH",
                "ZR Open Grotto Chest",
                "Song from Malon",
                "LLR Talons Chickens",
                "LLR Freestanding PoH",
            };

            AssertExpectedRegions(testEngine, expected);

            //Assert.AreEqual(33, locations.Count);

            var skulls = testEngine.GetAvailableSkulls();
            Assert.AreEqual(17, skulls.Count());
        }

        [TestMethod]
        public void Season7Base_ZL_Bomb_Boomerang_Letter()
        {
            var settings = SeedSettings.ReadFromJson(@"C:\Users\ryago\source\repos\OoTRChecklistTracker\ChecklistTracker.Config\Resources\settings\season7-base.json").Result;
            var config = TrackerConfig.Init().Result;
            var testEngine = new LogicEngine(config, "v8.0");

            var expected = new HashSet<string>()
            {
                "Dodongos Cavern Map Chest",
                "Dodongos Cavern Compass Chest",
                "Dodongos Cavern Bomb Flower Platform Chest",
                "Dodongos Cavern Bomb Bag Chest",
                "Dodongos Cavern End of Bridge Chest",
                "Dodongos Cavern Boss Room Chest",
                "Dodongos Cavern King Dodongo Heart",
                "Jabu Jabus Belly Boomerang Chest",
                "Jabu Jabus Belly Map Chest",
                "Jabu Jabus Belly Compass Chest",
                "Jabu Jabus Belly Barinade Heart",
                "Ice Cavern Map Chest",
                "Ice Cavern Compass Chest",
                "Ice Cavern Freestanding PoH",
                "Ice Cavern Iron Boots Chest",
                "Sheik in Ice Cavern",
                "KF Kokiri Sword Chest",
                "KF Midos Top Left Chest",
                "KF Midos Top Right Chest",
                "KF Midos Bottom Left Chest",
                "KF Midos Bottom Right Chest",
                "LW Ocarina Memory Game",
                "LW Deku Scrub Near Bridge",
                "LW Near Shortcuts Grotto Chest",
                "Deku Theater Skull Mask",
                "LW Deku Scrub Grotto Front",
                "Song from Saria",
                "SFM Wolfos Grotto Chest",
                "HF Southeast Grotto Chest",
                "HF Open Grotto Chest",
                "HF Deku Scrub Grotto",
                "HF Near Market Grotto Chest",
                "LH Child Fishing",
                "GV Waterfall Freestanding PoH",
                "GV Crate Freestanding PoH",
                "Market Shooting Gallery Reward",
                "Market Bombchu Bowling First Prize",
                "Market Bombchu Bowling Second Prize",
                "Market Lost Dog",
                "HC Great Fairy Reward",
                "Kak 10 Gold Skulltula Reward",
                "Kak 20 Gold Skulltula Reward",
                "Kak 30 Gold Skulltula Reward",
                "Kak 40 Gold Skulltula Reward",
                "Kak Anju as Adult",
                "Kak Anju as Child",
                "Kak Man on Roof",
                "Kak Impas House Freestanding PoH",
                "Kak Windmill Freestanding PoH",
                "Song from Windmill",
                "Kak Redead Grotto Chest",
                "Kak Open Grotto Chest",
                "Graveyard Dampe Gravedigging Tour",
                "Graveyard Shield Grave Chest",
                "Song from Royal Familys Tomb",
                "Graveyard Dampe Race Hookshot Chest",
                "Graveyard Dampe Race Freestanding PoH",
                "DMT Chest",
                "DMT Freestanding PoH",
                "DMT Great Fairy Reward",
                "GC Maze Center Chest",
                "GC Maze Right Chest",
                "GC Rolling Goron as Child",
                "GC Rolling Goron as Adult",
                "GC Pot Freestanding PoH",
                "DMC Wall Freestanding PoH",
                "DMC Upper Grotto Chest",
                "ZR Near Open Grotto Freestanding PoH",
                "ZR Near Domain Freestanding PoH",
                "ZR Open Grotto Chest",
                "ZD Diving Minigame",
                "ZD Chest",
                "ZD King Zora Thawed",
                "ZF Iceberg Freestanding PoH",
                "ZF Great Fairy Reward",
                "Song from Malon",
                "LLR Talons Chickens",
                "LLR Freestanding PoH",
                "LH Adult Fishing",
                "LH Freestanding PoH",
                "Graveyard Freestanding PoH",
            };

            var inventory = testEngine.Inventory;
            inventory["Bomb_Bag"] = 1;
            inventory["Boomerang"] = 1;
            inventory["Rutos_Letter"] = 1;
            inventory["Zeldas_Lullaby"] = 1;
            testEngine.UpdateItems(inventory);

            Assert.IsTrue(testEngine.CanAccess("Deliver Rutos Letter"), "Deliver Rutos Letter");
            Assert.IsTrue(testEngine.CanAccessRegion("Zoras Fountain", "child"), "Zoras Fountain");
            Assert.IsTrue(testEngine.CanAccessRegion("Jabu Jabus Belly Beginning", "child"), "Jabu Jabus Belly Beginning");
            Assert.IsTrue(testEngine.CanAccessRegion("Jabu Jabus Belly Main", "child"), "Jabu Jabus Belly Main");
            Assert.IsTrue(testEngine.CanAccess("Jabu Jabus Belly Boomerang Chest"), "Jabu Jabus Belly Boomerang Chest");
            Assert.IsTrue(testEngine.CanAccess("Jabu Jabus Belly Map Chest"), "Jabu Jabus Belly Map Chest");

            AssertExpectedRegions(testEngine, expected);


            var skulls = testEngine.GetAvailableSkulls();
            Assert.AreEqual(41, skulls.Count());
        }

        [TestMethod]
        public void Season7Base_ZL_Bomb_Boomerang_Letter_Hookshot()
        {
            var settings = SeedSettings.ReadFromJson(@"C:\Users\ryago\source\repos\OoTRChecklistTracker\ChecklistTracker.Config\Resources\settings\season7-base.json").Result;
            var config = TrackerConfig.Init().Result;
            var testEngine = new LogicEngine(config, "v8.0");

            var expected = new HashSet<string>()
            {
                "Dodongos Cavern Map Chest",
                "Dodongos Cavern Compass Chest",
                "Dodongos Cavern Bomb Flower Platform Chest",
                "Dodongos Cavern Bomb Bag Chest",
                "Dodongos Cavern End of Bridge Chest",
                "Dodongos Cavern Boss Room Chest",
                "Dodongos Cavern King Dodongo Heart",
                "Jabu Jabus Belly Boomerang Chest",
                "Jabu Jabus Belly Map Chest",
                "Jabu Jabus Belly Compass Chest",
                "Jabu Jabus Belly Barinade Heart",
                "Fire Temple Near Boss Chest",
                "Fire Temple Big Lava Room Lower Open Door Chest",
                "Fire Temple Big Lava Room Blocked Door Chest",
                "Ice Cavern Map Chest",
                "Ice Cavern Compass Chest",
                "Ice Cavern Freestanding PoH",
                "Ice Cavern Iron Boots Chest",
                "Sheik in Ice Cavern",
                "KF Kokiri Sword Chest",
                "KF Midos Top Left Chest",
                "KF Midos Top Right Chest",
                "KF Midos Bottom Left Chest",
                "KF Midos Bottom Right Chest",
                "LW Ocarina Memory Game",
                "LW Deku Scrub Near Bridge",
                "LW Near Shortcuts Grotto Chest",
                "Deku Theater Skull Mask",
                "LW Deku Scrub Grotto Front",
                "Song from Saria",
                "SFM Wolfos Grotto Chest",
                "HF Southeast Grotto Chest",
                "HF Open Grotto Chest",
                "HF Deku Scrub Grotto",
                "HF Near Market Grotto Chest",
                "LH Freestanding PoH",
                "LH Child Fishing",
                "LH Adult Fishing",
                "GV Waterfall Freestanding PoH",
                "GV Crate Freestanding PoH",
                "Market Shooting Gallery Reward",
                "Market Bombchu Bowling First Prize",
                "Market Bombchu Bowling Second Prize",
                "Market Lost Dog",
                "HC Great Fairy Reward",
                "Kak 10 Gold Skulltula Reward",
                "Kak 20 Gold Skulltula Reward",
                "Kak 30 Gold Skulltula Reward",
                "Kak 40 Gold Skulltula Reward",
                "Kak 50 Gold Skulltula Reward",
                "Kak Anju as Adult",
                "Kak Anju as Child",
                "Kak Man on Roof",
                "Kak Impas House Freestanding PoH",
                "Kak Windmill Freestanding PoH",
                "Song from Windmill",
                "Kak Redead Grotto Chest",
                "Kak Open Grotto Chest",
                "Graveyard Dampe Gravedigging Tour",
                "Graveyard Shield Grave Chest",
                "Song from Royal Familys Tomb",
                "Graveyard Dampe Race Hookshot Chest",
                "Graveyard Dampe Race Freestanding PoH",
                "DMT Chest",
                "DMT Freestanding PoH",
                "DMT Great Fairy Reward",
                "GC Maze Center Chest",
                "GC Maze Right Chest",
                "GC Rolling Goron as Child",
                "GC Rolling Goron as Adult",
                "GC Pot Freestanding PoH",
                "DMC Wall Freestanding PoH",
                "Sheik in Crater",
                "DMC Upper Grotto Chest",
                "ZR Near Open Grotto Freestanding PoH",
                "ZR Near Domain Freestanding PoH",
                "ZR Open Grotto Chest",
                "ZD Diving Minigame",
                "ZD Chest",
                "ZD King Zora Thawed",
                "ZF Iceberg Freestanding PoH",
                "ZF Great Fairy Reward",
                "Song from Malon",
                "LLR Talons Chickens",
                "LLR Freestanding PoH",
                "Graveyard Freestanding PoH",
            };

            var inventory = testEngine.Inventory;
            inventory["Bomb_Bag"] = 1;
            inventory["Boomerang"] = 1;
            inventory["Rutos_Letter"] = 1;
            inventory["Zeldas_Lullaby"] = 1;
            inventory["Progressive_Hookshot"] = 1;
            testEngine.UpdateItems(inventory);

            AssertExpectedRegions(testEngine, expected);

            var skulls = testEngine.GetAvailableSkulls();
            Assert.AreEqual(50, skulls.Count());
        }

        [TestMethod]
        public void Season7Base_ZL_Bomb_Bow_Boomerang_Letter_Longshot_Str2_Magic_Lens()
        {
            var settings = SeedSettings.ReadFromJson(@"C:\Users\ryago\source\repos\OoTRChecklistTracker\ChecklistTracker.Config\Resources\settings\season7-base.json").Result;
            var config = TrackerConfig.Init().Result;
            var testEngine = new LogicEngine(config, "v8.0");

            var expected = new HashSet<string>()
            {
                "Dodongos Cavern Map Chest",
                "Dodongos Cavern Compass Chest",
                "Dodongos Cavern Bomb Flower Platform Chest",
                "Dodongos Cavern Bomb Bag Chest",
                "Dodongos Cavern End of Bridge Chest",
                "Dodongos Cavern Boss Room Chest",
                "Dodongos Cavern King Dodongo Heart",
                "Jabu Jabus Belly Boomerang Chest",
                "Jabu Jabus Belly Map Chest",
                "Jabu Jabus Belly Compass Chest",
                "Jabu Jabus Belly Barinade Heart",
                "Fire Temple Near Boss Chest",
                "Fire Temple Big Lava Room Lower Open Door Chest",
                "Fire Temple Big Lava Room Blocked Door Chest",
                "Ice Cavern Map Chest",
                "Ice Cavern Compass Chest",
                "Ice Cavern Freestanding PoH",
                "Ice Cavern Iron Boots Chest",
                "Sheik in Ice Cavern",
                "Gerudo Training Ground Lobby Left Chest",
                "Gerudo Training Ground Lobby Right Chest",
                "Gerudo Training Ground Stalfos Chest",
                "Gerudo Training Ground Beamos Chest",
                "Gerudo Training Ground Before Heavy Block Chest",
                "Gerudo Training Ground Heavy Block First Chest",
                "Gerudo Training Ground Heavy Block Second Chest",
                "Gerudo Training Ground Heavy Block Third Chest",
                "Gerudo Training Ground Heavy Block Fourth Chest",
                "Gerudo Training Ground Eye Statue Chest",
                "Gerudo Training Ground Near Scarecrow Chest",
                "Gerudo Training Ground Hammer Room Clear Chest",
                "Gerudo Training Ground Maze Right Side Chest",
                "Gerudo Training Ground Maze Right Central Chest",
                "Gerudo Training Ground Freestanding Key",
                "Gerudo Training Ground Maze Path First Chest",
                "Gerudo Training Ground Hidden Ceiling Chest",
                "Gerudo Training Ground Maze Path Second Chest",
                "Gerudo Training Ground Maze Path Third Chest",
                "Gerudo Training Ground Maze Path Final Chest",
                "Spirit Temple Compass Chest",
                "Spirit Temple Early Adult Right Chest",
                "Spirit Temple Child Climb North Chest",
                "Spirit Temple Child Climb East Chest",
                "Spirit Temple Silver Gauntlets Chest",
                "Spirit Temple First Mirror Left Chest",
                "Spirit Temple First Mirror Right Chest",
                "Spirit Temple Statue Room Hand Chest",
                "Spirit Temple Statue Room Northeast Chest",
                "Spirit Temple Hallway Right Invisible Chest",
                "Spirit Temple Hallway Left Invisible Chest",
                "Spirit Temple Mirror Shield Chest",
                "Spirit Temple Boss Key Chest",
                "Kak 10 Gold Skulltula Reward",
                "Kak 20 Gold Skulltula Reward",
                "Kak 30 Gold Skulltula Reward",
                "Kak 40 Gold Skulltula Reward",
                "Kak 50 Gold Skulltula Reward",
                "Kak Shooting Gallery Reward",
                "KF Kokiri Sword Chest",
                "KF Midos Top Left Chest",
                "KF Midos Top Right Chest",
                "KF Midos Bottom Left Chest",
                "KF Midos Bottom Right Chest",
                "LW Ocarina Memory Game",
                "LW Deku Scrub Near Bridge",
                "LW Near Shortcuts Grotto Chest",
                "Deku Theater Skull Mask",
                "LW Deku Scrub Grotto Front",
                "Song from Saria",
                "SFM Wolfos Grotto Chest",
                "HF Southeast Grotto Chest",
                "HF Open Grotto Chest",
                "HF Deku Scrub Grotto",
                "HF Near Market Grotto Chest",
                "LH Freestanding PoH",
                "LH Child Fishing",
                "LH Adult Fishing",
                "LH Sun",
                "GV Waterfall Freestanding PoH",
                "GV Crate Freestanding PoH",
                "GF Chest",
                "Sheik at Colossus",
                "Colossus Great Fairy Reward",
                "Market Shooting Gallery Reward",
                "Market Bombchu Bowling First Prize",
                "Market Bombchu Bowling Second Prize",
                "Market Treasure Chest Game Reward",
                "Market Lost Dog",
                "HC Great Fairy Reward",
                "Kak Anju as Adult",
                "Kak Anju as Child",
                "Kak Man on Roof",
                "Kak Impas House Freestanding PoH",
                "Kak Windmill Freestanding PoH",
                "Song from Windmill",
                "Kak Redead Grotto Chest",
                "Kak Open Grotto Chest",
                "Graveyard Freestanding PoH",
                "Graveyard Dampe Gravedigging Tour",
                "Graveyard Shield Grave Chest",
                "Song from Royal Familys Tomb",
                "Graveyard Dampe Race Hookshot Chest",
                "Graveyard Dampe Race Freestanding PoH",
                "DMT Chest",
                "DMT Freestanding PoH",
                "DMT Great Fairy Reward",
                "GC Maze Left Chest",
                "GC Maze Center Chest",
                "GC Maze Right Chest",
                "GC Rolling Goron as Child",
                "GC Rolling Goron as Adult",
                "GC Pot Freestanding PoH",
                "DMC Wall Freestanding PoH",
                "Sheik in Crater",
                "DMC Upper Grotto Chest",
                "ZR Near Open Grotto Freestanding PoH",
                "ZR Near Domain Freestanding PoH",
                "ZR Open Grotto Chest",
                "ZD Diving Minigame",
                "ZD Chest",
                "ZD King Zora Thawed",
                "ZF Iceberg Freestanding PoH",
                "ZF Great Fairy Reward",
                "Song from Malon",
                "LLR Talons Chickens",
                "LLR Freestanding PoH",
            };
            var inventory = testEngine.Inventory;
            inventory["Bomb_Bag"] = 1;
            inventory["Bow"] = 1;
            inventory["Boomerang"] = 1;
            inventory["Rutos_Letter"] = 1;
            inventory["Zeldas_Lullaby"] = 1;
            inventory["Progressive_Hookshot"] = 2;
            inventory["Progressive_Strength_Upgrade"] = 2;
            inventory["Magic_Meter"] = 1;
            inventory["Lens_of_Truth"] = 1;
            testEngine.UpdateItems(inventory);
            AssertExpectedRegions(testEngine, expected);
            var skulls = testEngine.GetAvailableSkulls();
            Assert.AreEqual(62, skulls.Count());
        }
        [TestMethod]
        public void Season7Base_WastelandAccess()
        {
            var settings = SeedSettings.ReadFromJson(@"settings\season7-base.json").Result;
            var config = TrackerConfig.Init().Result;
            var testEngine = new LogicEngine(config, "v8.0");

            testEngine.Inventory["Progressive_Hookshot"] = 2;
            testEngine.Inventory["Dins_Fire"] = 1;
            testEngine.Inventory["Magic_Meter"] = 1;
            testEngine.UpdateItems(testEngine.Inventory);
            AssertCanAccessEvent(testEngine, "Hideout 1 Torch Jail Carpenter");
            AssertCanAccessEvent(testEngine, "GF Gate Open");
            AssertCanAccessRegion(testEngine, "GF Outside Gate", "adult");
            AssertCanAccessRegion(testEngine, "Wasteland Near Fortress", "adult");
            AssertCanAccessRegion(testEngine, "Haunted Wasteland", "adult");
            AssertCanAccess(testEngine, "Wasteland GS");
            AssertCanAccess(testEngine, "Wasteland Chest");
        }
        private void AssertCanAccessEvent(LogicEngine engine, string eventName)
        {
            Assert.IsTrue(engine.CanAccessEvent(eventName), eventName);
        }
        private void AssertCanAccess(LogicEngine engine, string location)
        {
            Assert.IsTrue(engine.CanAccess(location), location);
        }
        private void AssertCanAccessRegion(LogicEngine engine, string region, string age)
        {
            Assert.IsTrue(engine.CanAccessRegion(region, age), $"{region} as {age}");
        }
        private void AssertExpectedRegions(LogicEngine engine, ISet<string> expected)
        {
            var locations = engine.GetLocations().ToHashSet();
            var unexpected = locations.Except(expected).ToList();
            Assert.IsFalse(unexpected.Any(), "Unexpected: " + string.Join(",", unexpected));
            var missing = expected.Except(locations).ToList();
            Assert.IsFalse(missing.Any(), "Expected: " + string.Join(",", missing));
            foreach (var location in expected)
            {
                Assert.IsTrue(engine.CanAccess(location), "Expected access to: " + location);
            }
            CollectionAssert.AreEqual(
            expected.ToImmutableHashSet(),
                locations.ToImmutableHashSet());
        }
    }
}
