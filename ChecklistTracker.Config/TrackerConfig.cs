using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace ChecklistTracker.Config
{
    public class TrackerConfig
    {
        private static string ProgramDir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory?.FullName ?? "wtf";

        public ItemTable ItemTable { get; private set; }
        public IDictionary<string, ISet<string>> HintRegions { get; private set; }
        public IDictionary<string, string> HintRegionShortNames { get; private set; }
        public IDictionary<string, LocationData> LocationTable { get; private set; }
        public IList<string> ChildTradeItems { get; private set; }
        public IList<string> AdultTradeItems { get; private set; }
        public IList<string> Dungeons { get; private set; }

        public IDictionary<string, int> DefaultInventory { get; private set; }

        private TrackerConfig(
            ItemTable itemTable,
            IDictionary<string, ISet<string>> hintRegions,
            IDictionary<string, string> hintRegionShortNames,
            IDictionary<string, LocationData> locationTable,
            IList<string> childTradeItems,
            IList<string> adultTradeItems,
            IList<string> dungeons,
            IDictionary<string, int> defaultInventory
        )
        {
            ItemTable = itemTable;
            HintRegions = hintRegions;
            HintRegionShortNames = hintRegionShortNames;
            LocationTable = locationTable;
            ChildTradeItems = childTradeItems;
            AdultTradeItems = adultTradeItems;
            Dungeons = dungeons;
            DefaultInventory = defaultInventory;
        }

        public static async Task<TrackerConfig> Init()
        {
            var itemTable = await LoadItemTable().ConfigureAwait(false);

            var hintRegions = await LoadHintRegions().ConfigureAwait(false);
            var hintRegionShortNames = await LoadHintRegionShortNames().ConfigureAwait(false);

            var locationTable = await LoadLocationTable().ConfigureAwait(false);

            var childTradeItems = await LoadChildTradeItems().ConfigureAwait(false);
            var adultTradeItems = await LoadAdultTradeItems().ConfigureAwait(false);

            var defaultInventory = await LoadDefaultInventory().ConfigureAwait(false);

            var dungeons = await LoadDungeons().ConfigureAwait(false);

            return new TrackerConfig(
                itemTable: itemTable,
                hintRegions: hintRegions,
                hintRegionShortNames: hintRegionShortNames,
                locationTable: locationTable,
                childTradeItems: childTradeItems,
                adultTradeItems: adultTradeItems,
                dungeons: dungeons,
                defaultInventory: defaultInventory
            );
        }

        private static async Task<ItemTable> LoadItemTable()
        {
            return await ParseJson<ItemTable>($"{ProgramDir}/config/image-table.json").ConfigureAwait(false);
        }

        private static async Task<IDictionary<string, ISet<string>>> LoadHintRegions()
        {
            return await ParseJson<IDictionary<string, ISet<string>>>($"{ProgramDir}/config/hint-regions.json").ConfigureAwait(false);
        }

        private static async Task<IDictionary<string, string>> LoadHintRegionShortNames()
        {
            return await ParseJson<IDictionary<string, string>>($"{ProgramDir}/config/hint-regions-short-names.json").ConfigureAwait(false);
        }

        private static async Task<IList<string>> LoadChildTradeItems()
        {
            return await ParseJson<IList<string>>($"{ProgramDir}/config/child-trade-items.json").ConfigureAwait(false);
        }

        private static async Task<IList<string>> LoadAdultTradeItems()
        {
            return await ParseJson<IList<string>>($"{ProgramDir}/config/trade-items.json").ConfigureAwait(false);
        }

        private static async Task<IList<string>> LoadDungeons()
        {
            return await ParseJson<IList<string>>($"{ProgramDir}/config/dungeons.json").ConfigureAwait(false);
        }

        private static async Task<IDictionary<string, int>> LoadDefaultInventory()
        {
            return await ParseJson<IDictionary<string, int>>($"{ProgramDir}/config/default-items.json").ConfigureAwait(false);
        }

        private static async Task<IDictionary<string, LocationData>> LoadLocationTable()
        {
            var locationTable = await ParseJson<IDictionary<string, IList<string>>>($"{ProgramDir}/config/location-table.json").ConfigureAwait(false);
            return locationTable.ToDictionary(
                entry => entry.Key,
                entry => new LocationData
                {
                    Name = entry.Key,
                    Type = entry.Value[0],
                    VanillaItem = entry.Value[1],
                }
            );
        }

        public static async Task<T> ParseJson<T>(string file)
        {
            using Stream stream = File.Open(file, FileMode.Open, FileAccess.Read);

            if (stream == null)
            {
                throw new IOException("Failed to load image-table.json");
            }

            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                PropertyNameCaseInsensitive = true,
                Converters = {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
                }
            };

            var result = await JsonSerializer.DeserializeAsync<T>(stream, options).ConfigureAwait(false);

            return result != null ? result : throw new IOException($"Failed to parse {file}");
        }
    }
}
