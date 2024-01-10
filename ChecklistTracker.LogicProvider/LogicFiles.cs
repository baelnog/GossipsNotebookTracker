using ChecklistTracker.Config;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.LogicProvider.DataFiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChecklistTracker.LogicProvider
{
    public class LogicFiles
    {
        private static Regex HashComments = new Regex(@"#.*\n");
        private static Regex MultilineString = new Regex(@"\n\s+");

        internal IDictionary<string, string> LogicHelpers { get; private set; }
        internal IDictionary<string, IEnumerable<Region>> DungeonFiles { get; private set; }
        internal IEnumerable<Region> BossesFile { get; private set; }
        internal IEnumerable<Region> OverworldFile { get; private set; }

        private LogicFiles() { }

        public static async Task<LogicFiles> LoadLogicFiles(
            DirectoryInfo dataDirectory)
        {
            var logicHelpers = LoadLogicFile<IDictionary<string, string>>(new FileInfo(Path.Combine(dataDirectory.FullName, "LogicHelpers.json")));
            var worldDirectory = new DirectoryInfo(Path.Combine(dataDirectory.FullName, "World"));
            var bossFile = LoadLogicFile<IEnumerable<Region>>(new FileInfo(Path.Combine(worldDirectory.FullName, "Bosses.json")));
            var overworldFile = LoadLogicFile<IEnumerable<Region>>(new FileInfo(Path.Combine(worldDirectory.FullName, "Overworld.json")));

            var dungeonFiles = worldDirectory
                .EnumerateFiles()
                .Where(f => f.Name != "Bosses.json" && f.Name != "Overworld.json")
                .ToDictionary(f => Path.GetFileNameWithoutExtension(f.Name), async f => await LoadLogicFile<IEnumerable<Region>>(f).ConfigureAwait(false));

            var tasks = new List<Task> { logicHelpers, bossFile, overworldFile };
            tasks.AddRange(dungeonFiles.Values);

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return new LogicFiles
            {
                LogicHelpers = await logicHelpers.ConfigureAwait(false),
                BossesFile = await bossFile.ConfigureAwait(false),
                OverworldFile = await overworldFile.ConfigureAwait(false),
                DungeonFiles = dungeonFiles.ToDictionary(kv => kv.Key, kv => kv.Value.Result)
            };
        }

        private static async Task<T> LoadLogicFile<T>(FileInfo file)
        {
            var contents = await File.ReadAllTextAsync(file.FullName).ConfigureAwait(false);

            contents = HashComments.Replace(contents, "\n");
            contents = MultilineString.Replace(contents, " ");

            try
            {
                var options = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                };
                options.Converters.Add(new JsonBooleanConverter());
                return JsonSerializer.Deserialize<T>(contents, options);
            }
            catch (Exception ex)
            {
                Logging.WriteLine(ex.Message, ex);
                throw;
            }
        }

    }
}
