using ChecklistTracker.Config;
using ChecklistTracker.UI.Config;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker
{
    class ResourceFinder
    {

        private static Lazy<string> ProgramDir = new Lazy<string>(() => new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName);

        private static Lazy<ItemTable> Config = new Lazy<ItemTable>(() =>
        {
            return TrackerConfig.Init().Result.ItemTable;
        });

        private static ConcurrentDictionary<string, BitmapImage> imageCache = new ConcurrentDictionary<string, BitmapImage>();

        private static Lazy<Dictionary<string, string>> itemLookupByElementId = new Lazy<Dictionary<string, string>>(() =>
        {
            using Stream stream = File.Open($"{ProgramDir.Value}/config/ootr/hashfrog-elements.json", FileMode.Open, FileAccess.Read);
            var elements = JsonSerializer.Deserialize<IEnumerable<HashFrogElement>>(stream, new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                PropertyNameCaseInsensitive = true,
                Converters = {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
                }
            });
            return elements!.ToDictionary((element) => element.id, (element) => element.name);
        });

        public static IEnumerable<Item> GetItems()
        {
            return Config.Value.items.Values;
        }

        public static async Task<string> ReadResourceFile(string path)
        {
            return await File.ReadAllTextAsync($"{ProgramDir.Value}/{path}", Encoding.UTF8).ConfigureAwait(false);
        }


        public static Item? FindItem(string itemName)
        {
            Config.Value.items.TryGetValue(itemName, out var value);
            return value;
        }

        public static string? FindItemById(string itemId)
        {
            if (itemLookupByElementId.Value.TryGetValue(itemId, out var item))
            {
                return item;
            }
            return null;
        }

        public static ImageSource? FindItem(string itemName, int count)
        {
            if (Config.Value.items.TryGetValue(itemName, out var item))
            {
                return FindItemImage(item, count);
            }
            return null;
        }

        public static List<string>? GetImageSet(string setName)
        {
            if (Config.Value.image_groups.TryGetValue(setName, out var imageSet))
            {
                return imageSet;
            }
            return null;
        }

        public static BitmapImage FindItemImage(Item item, int count)
        {
            var resourcePath = ProgramDir.Value + "\\items\\" + item.images[Math.Min(count, item.images.Length - 1)];

            return imageCache.GetOrAdd(resourcePath, (path) => new BitmapImage() { UriSource = new Uri($"file://{path}") });
        }

        public static ImageSource FindImageGroupImage(string setName, int index)
        {
            var imageSet = GetImageSet(setName);
            string item;
            if (imageSet != null)
            {
                item = imageSet[Math.Min(index, imageSet.Count - 1)];
            }
            else
            {
                item = setName;
            }

            return FindItem(item, 1);
        }

        internal static int BoundLabelIndex(string labelSet, int startingIndex)
        {
            var length = Config.Value.labels[labelSet].Count;
            while (startingIndex < 0) { startingIndex += length; }
            return startingIndex % length;
        }

        internal static string? GetLabel(string labelSet, int startingIndex)
        {
            return Config.Value.labels[labelSet][startingIndex].name;
        }

        internal static List<Label> GetLabels(string labelSet)
        {
            return Config.Value.labels[labelSet];
        }
    }
}
