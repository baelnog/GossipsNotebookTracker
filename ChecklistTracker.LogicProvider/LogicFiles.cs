using ChecklistTracker.Config;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.LogicProvider.DataFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChecklistTracker.LogicProvider
{
    public class LogicFiles
    {
        private static Regex HashComments = new Regex(@"(#|(//)).*\n");
        private static Regex MultilineString = new Regex(@"\n\s+");

        internal IDictionary<string, string> LogicHelpers { get; private set; }
        internal IDictionary<string, string> LogicHelpersAdditional { get; private set; }
        internal IDictionary<string, IEnumerable<Region>> DungeonFiles { get; private set; }
        internal IDictionary<string, IEnumerable<Region>> DungeonFilesAdditional { get; private set; }
        internal IEnumerable<Region> BossesFile { get; private set; }
        internal IEnumerable<Region> BossesFileAdditional { get; private set; }
        internal IEnumerable<Region> OverworldFile { get; private set; }
        internal IEnumerable<Region> OverworldFileAdditional { get; private set; }

        private LogicFiles() { }

        public static async Task<LogicFiles> LoadLogicFiles(
            DirectoryInfo dataDirectory)
        {
            var logicHelpers = LoadLogicFile<IDictionary<string, string>>(new FileInfo(Path.Combine(dataDirectory.FullName, "LogicHelpers.json")));
            var worldDirectory = new DirectoryInfo(Path.Combine(dataDirectory.FullName, "World"));
            var addtionalDirectory = new DirectoryInfo(Path.Combine(TrackerConfig.ProgramDir, "config/ootr/additional-logic/v8.0"));
            var addtionalWorldDirectory = new DirectoryInfo(Path.Combine(addtionalDirectory.FullName, "World"));
            var bossFile = LoadLogicFile<IEnumerable<Region>>(new FileInfo(Path.Combine(worldDirectory.FullName, "Bosses.json")));
            var overworldFile = LoadLogicFile<IEnumerable<Region>>(new FileInfo(Path.Combine(worldDirectory.FullName, "Overworld.json")));

            var dungeonFiles = worldDirectory
                .EnumerateFiles()
                .Where(f => f.Name != "Bosses.json" && f.Name != "Overworld.json")
                .ToDictionary(f => Path.GetFileNameWithoutExtension(f.Name), async f => await LoadLogicFile<IEnumerable<Region>>(f).ConfigureAwait(false));

            var logicHelpersAdditional = LoadLogicFile<IDictionary<string, string>>(new FileInfo(Path.Combine(addtionalDirectory.FullName, "LogicHelpers.json")));
            var dungeonFilesAdditional = addtionalWorldDirectory
                .EnumerateFiles()
                .Where(f => f.Name != "Bosses.json" && f.Name != "Overworld.json")
                .ToDictionary(f => Path.GetFileNameWithoutExtension(f.Name), async f => await LoadLogicFile<IEnumerable<Region>>(f).ConfigureAwait(false));

            var bossFileAdditional = LoadLogicFile<IEnumerable<Region>>(new FileInfo(Path.Combine(addtionalWorldDirectory.FullName, "Bosses.json")));
            var overworldFileAdditional = LoadLogicFile<IEnumerable<Region>>(new FileInfo(Path.Combine(addtionalWorldDirectory.FullName, "Overworld.json")));

            var tasks = new List<Task> { logicHelpers, bossFile, overworldFile, bossFileAdditional, overworldFileAdditional };
            tasks.AddRange(dungeonFiles.Values);

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return new LogicFiles
            {
                LogicHelpers = await logicHelpers.ConfigureAwait(false),
                LogicHelpersAdditional = await logicHelpersAdditional.ConfigureAwait(false),
                BossesFile = await bossFile.ConfigureAwait(false),
                BossesFileAdditional = await bossFileAdditional.ConfigureAwait(false),
                OverworldFile = await overworldFile.ConfigureAwait(false),
                OverworldFileAdditional = await overworldFileAdditional.ConfigureAwait(false),
                DungeonFiles = dungeonFiles.ToDictionary(kv => kv.Key, kv => kv.Value.Result),
                DungeonFilesAdditional = dungeonFilesAdditional.ToDictionary(kv => kv.Key, kv => kv.Value.Result),
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
                    AllowTrailingCommas = true,
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
