using ChecklistTracker.Config.Settings;
using ChecklistTracker.Config.Settings.SettingsTypes;
using ChecklistTracker.CoreUtils;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Config
{
    public partial class TrackerConfig : INotifyPropertyChanged
    {
        public static string ProgramDir = new FileInfo(Environment.ProcessPath).Directory?.FullName ?? "wtf";

#pragma warning disable 67
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 67

        public UserConfig UserConfig { get; private set; }
        public ItemTable ItemTable { get; private set; }
        public IDictionary<string, ISet<string>> HintRegions { get; private set; }
        public IDictionary<string, string> HintRegionShortNames { get; private set; }
        public IDictionary<string, LocationData> LocationTable { get; private set; }
        public IList<string> Dungeons { get; private set; }

        public IDictionary<string, int> DefaultInventory { get; private set; }

        public SeedSettings RandomizerSettings { get; private set; }

        public static JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            Converters = {
                new JsonStringEnumMemberConverter(JsonNamingPolicy.CamelCase),
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            }
        };

        private TrackerConfig(
            UserConfig userConfig,
            SeedSettings randomizerSettings,
            ItemTable itemTable,
            IDictionary<string, ISet<string>> hintRegions,
            IDictionary<string, string> hintRegionShortNames,
            IDictionary<string, LocationData> locationTable,
            IList<string> dungeons,
            IDictionary<string, int> defaultInventory
        )
        {
            UserConfig = userConfig;
            RandomizerSettings = randomizerSettings;
            ItemTable = itemTable;
            HintRegions = hintRegions;
            HintRegionShortNames = hintRegionShortNames;
            LocationTable = locationTable;
            Dungeons = dungeons;
            DefaultInventory = defaultInventory;
        }

        public static async Task<TrackerConfig> Init()
        {
            Logging.WriteLine(Environment.ProcessPath);
            Logging.WriteLine(Environment.CurrentDirectory);
            Logging.WriteLine(ProgramDir);

            var userConfig = await LoadUserConfig().ConfigureAwait(false);
            var userConfigSaveTask = Task.CompletedTask;
            userConfig.PropertyChanged += (o, e) =>
            {
                Logging.WriteLine($"Saving user config change to {e.PropertyName}.");
                userConfigSaveTask.ContinueWith((t, o) => SaveUserConfig(userConfig).ConfigureAwait(false), o);
            };

            var randomizerSettings = await LoadSettings(userConfig.SettingsPath).ConfigureAwait(false);

            var itemTable = await LoadItemTable().ConfigureAwait(false);

            var hintRegions = await LoadHintRegions().ConfigureAwait(false);
            var hintRegionShortNames = await LoadHintRegionShortNames().ConfigureAwait(false);

            var locationTable = await LoadLocationTable().ConfigureAwait(false);

            var defaultInventory = await LoadDefaultInventory().ConfigureAwait(false);

            var dungeons = await LoadDungeons().ConfigureAwait(false);

            return new TrackerConfig(
                userConfig: userConfig,
                randomizerSettings: randomizerSettings,
                itemTable: itemTable,
                hintRegions: hintRegions,
                hintRegionShortNames: hintRegionShortNames,
                locationTable: locationTable,
                dungeons: dungeons,
                defaultInventory: defaultInventory
            );
        }

        private static async Task<ItemTable> LoadItemTable()
        {
            return await ParseJson<ItemTable>($"{ProgramDir}/config/ootr/image-table.json").ConfigureAwait(false);
        }

        private static async Task<UserConfig> LoadUserConfig()
        {
            return await ParseJson<UserConfig>(UserConfig.UserConfigFile, () => new UserConfig())
                .ConfigureAwait(false);
        }

        private static async Task<SeedSettings> LoadSettings(string settingsPath)
        {
            var settings = await ParseJson<SeedSettings>($"{ProgramDir}/{settingsPath}")
                .ConfigureAwait(false);

            // Fix incompatible closed forest
            if (settings.KokiriForest == OpenForestType.Closed)
            {
                if (settings.ShuffleInteriorEntrances.HasFlag(ShuffleEntranceType.Special) ||
                    settings.ShuffleOverworldEntrances ||
                    settings.ShuffleWarpSongs ||
                    settings.ShuffleSpawnLocations.Any())
                {
                    settings.KokiriForest = OpenForestType.ClosedDeku;
                }
            }

            if (settings.TriforceHunt)
            {
                settings.ShuffleGanonsBK = ShuffleGanonsBKType.OnTriforce;
            }

            if (settings.DungeonShortcutsChoice == ChoiceType.All)
            {
                settings.DungeonShortcuts = new HashSet<DungeonChoiceType>()
                {
                    DungeonChoiceType.DekuTree,
                    DungeonChoiceType.DodongosCavern,
                    DungeonChoiceType.JabuJabusBelly,
                    DungeonChoiceType.ForestTemple,
                    DungeonChoiceType.FireTemple,
                    DungeonChoiceType.WaterTemple,
                    DungeonChoiceType.ShadowTemple,
                    DungeonChoiceType.SpiritTemple,
                };
            }

            if (settings.KeyRingsChoice == ChoiceType.All)
            {
                settings.KeyRings = new HashSet<DungeonChoiceType>()
                {
                    DungeonChoiceType.ThievesHideout,
                    DungeonChoiceType.ForestTemple,
                    DungeonChoiceType.FireTemple,
                    DungeonChoiceType.WaterTemple,
                    DungeonChoiceType.ShadowTemple,
                    DungeonChoiceType.SpiritTemple,
                    DungeonChoiceType.BottomOfTheWell,
                    DungeonChoiceType.GerudoTrainingGround,
                    DungeonChoiceType.GanonsCastle,
                };
            }

            if (settings.DungeonMode == MQDungeonModeType.MasterQuest)
            {
                settings.MQDungeons = new HashSet<DungeonChoiceType>()
                {
                    DungeonChoiceType.DekuTree,
                    DungeonChoiceType.DodongosCavern,
                    DungeonChoiceType.JabuJabusBelly,
                    DungeonChoiceType.ForestTemple,
                    DungeonChoiceType.FireTemple,
                    DungeonChoiceType.WaterTemple,
                    DungeonChoiceType.ShadowTemple,
                    DungeonChoiceType.SpiritTemple,
                    DungeonChoiceType.GanonsCastle,
                    DungeonChoiceType.BottomOfTheWell,
                    DungeonChoiceType.IceCavern,
                    DungeonChoiceType.GerudoTrainingGround,
                };
            }

            if (settings.DungeonShortcutsChoice == ChoiceType.Random)
            {
                settings.DungeonShortcuts = new HashSet<DungeonChoiceType>();
            }

            return settings;

        }

        private static async Task SaveUserConfig(UserConfig userConfig)
        {
            await SaveJson(UserConfig.UserConfigFile, userConfig)
                .ConfigureAwait(false);
        }

        private static async Task<IDictionary<string, ISet<string>>> LoadHintRegions()
        {
            return await ParseJson<IDictionary<string, ISet<string>>>($"{ProgramDir}/config/ootr/hint-regions.json").ConfigureAwait(false);
        }

        private static async Task<IDictionary<string, string>> LoadHintRegionShortNames()
        {
            return await ParseJson<IDictionary<string, string>>($"{ProgramDir}/config/ootr/hint-regions-short-names.json").ConfigureAwait(false);
        }

        private static async Task<IList<string>> LoadDungeons()
        {
            return await ParseJson<IList<string>>($"{ProgramDir}/config/ootr/dungeons.json").ConfigureAwait(false);
        }

        private static async Task<IDictionary<string, int>> LoadDefaultInventory()
        {
            return await ParseJson<IDictionary<string, int>>($"{ProgramDir}/config/ootr/default-items.json").ConfigureAwait(false);
        }

        private static async Task<IDictionary<string, LocationData>> LoadLocationTable()
        {
            var locationTable = await ParseJson<IDictionary<string, IList<string>>>($"{ProgramDir}/config/ootr/location-table.json").ConfigureAwait(false);
            return locationTable.ToDictionary(
                entry => entry.Key,
                entry => new LocationData
                {
                    Name = entry.Key,
                    Type = entry.Value[0],
                    VanillaItem = ToLogicItem(entry.Value[1]),
                }
            );
        }


        private static string ToLogicItem(string vanillaItem)
        {
            return vanillaItem
                .Replace(")", "")
                .Replace("(", "")
                .Replace(" ", "_");
        }

        public static async Task<T> ParseJson<T>(string file, Func<T>? defaultFactory = null)
        {
            if (!File.Exists(file))
            {
                if (defaultFactory != null)
                {
                    return defaultFactory.Invoke();
                }
            }
            using Stream stream = File.Open(file, FileMode.Open, FileAccess.Read);

            if (stream == null)
            {
                throw new IOException($"Failed to load {file}");
            }

            var result = await JsonSerializer.DeserializeAsync<T>(stream, JsonSerializerOptions).ConfigureAwait(false);

            return result != null ? result : throw new IOException($"Failed to parse {file}");
        }

        public static async Task SaveJson<T>(string file, T data)
        {
            using var stream = File.Open(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            stream.SetLength(0);
            if (stream == null)
            {
                throw new IOException($"Failed to load {file}");
            }

            await JsonSerializer.SerializeAsync(stream, data, JsonSerializerOptions)
                .ConfigureAwait(false); ;
        }

        public void SetRandomizerSettings(string settingsFile)
        {
            if (UserConfig.SettingsPath != settingsFile)
            {
                UserConfig.SetSettings(settingsFile);
                var queue = DispatcherQueue.GetForCurrentThread();
                Task.CompletedTask
                    .ContinueWith(async t => await LoadSettings(settingsFile))
                    .ContinueWith(t =>
                    {
                        queue.TryEnqueue(() => this.RandomizerSettings = t.Result.Result);
                    }).ConfigureAwait(false);
            }
        }
    }
}
