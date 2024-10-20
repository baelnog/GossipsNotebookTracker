using ChecklistTracker.CoreUtils;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ChecklistTracker.LogicProvider
{
    public class LogicFileCache
    {

        private static Lazy<string> ProgramDir = new Lazy<string>(() => new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName);
        private static Lazy<DirectoryInfo> CachedRoot = new Lazy<DirectoryInfo>(() =>
        {
            return new DirectoryInfo(Path.Combine(ProgramDir.Value, "logic_cache"));
        });

        public static async Task<DirectoryInfo> GetCachedLogicFilesForTagAsync(string tag)
        {
            DirectoryInfo logicCache = new DirectoryInfo(Path.Combine(CachedRoot.Value.FullName, tag));
            if (logicCache.Exists)
            {
                var hash = await HashDirectoryAsync(logicCache).ConfigureAwait(false);
                var expectedHash = GetCachedHash(logicCache);

                if (hash != null && hash == expectedHash)
                {
                    return logicCache;
                }
                logicCache.Delete(recursive: true);
            }
            await DownloadLogicFilesToDirectoryAsync(tag, logicCache).ConfigureAwait(false);
            return logicCache;
        }

        private static async Task<string> HashDirectoryAsync(DirectoryInfo directory)
        {
            using var sha256 = SHA256.Create();

            var fileContents = directory
                .GetFiles("*", SearchOption.AllDirectories)
                .Where(f => f.Name != "stamp.json")
                .OrderBy(f => f.Name)
                .Select(async f => await File.ReadAllBytesAsync(f.FullName).ConfigureAwait(false))
                .ToArray();

            var contentsTask = await Task.WhenAll<byte[]>(fileContents).ConfigureAwait(false);

            int i = 0;
            foreach (var contents in contentsTask)
            {
                if (++i >= contentsTask.Length)
                {
                    var ret = BitConverter.ToString(sha256.TransformFinalBlock(contents, 0, contents.Length));
                    return ret;
                }
                else
                {
                    sha256.TransformBlock(contents, 0, contents.Length, null, 0);
                }
            }
            return null;
        }

        private static string GetCachedHash(DirectoryInfo directory)
        {
            var stamp = Path.Combine(directory.FullName, "stamp.json");
            if (!File.Exists(stamp))
            {
                return null;
            }
            using var stampStream = File.OpenRead(stamp);
            var json = JsonNode.Parse(stampStream) as JsonObject;
            if (!json.TryGetPropertyValue("hash", out var hashNode) || hashNode == null)
            {
                return null;
            }
            return hashNode.AsValue().GetValue<string>();
        }

        static async Task DownloadLogicFilesToDirectoryAsync(string commit, DirectoryInfo cacheDirectory)
        {
            using var wc = new HttpClient();

            var baseUri = $"https://raw.githubusercontent.com/OoTRandomizer/OoT-Randomizer/{commit}/data";
            string[] files = new string[]
            {
                "LogicHelpers.json",
                "World/Bosses.json",
                "World/Overworld.json",
                "World/Deku Tree.json",
                "World/Deku Tree MQ.json",
                "World/Dodongos Cavern.json",
                "World/Dodongos Cavern MQ.json",
                "World/Jabu Jabus Belly.json",
                "World/Jabu Jabus Belly MQ.json",
                "World/Forest Temple.json",
                "World/Forest Temple MQ.json",
                "World/Fire Temple.json",
                "World/Fire Temple MQ.json",
                "World/Water Temple.json",
                "World/Water Temple MQ.json",
                "World/Spirit Temple.json",
                "World/Spirit Temple MQ.json",
                "World/Shadow Temple.json",
                "World/Shadow Temple MQ.json",
                "World/Ganons Castle.json",
                "World/Ganons Castle MQ.json",
                "World/Bottom of the Well.json",
                "World/Bottom of the Well MQ.json",
                "World/Ice Cavern.json",
                "World/Ice Cavern MQ.json",
                "World/Gerudo Training Ground.json",
                "World/Gerudo Training Ground MQ.json",
            };

            var gets = files
                .Select(async file => await DownloadFileAsync(wc, baseUri, file, cacheDirectory).ConfigureAwait(false))
                .ToArray();
            await Task.WhenAll(gets).ConfigureAwait(false);

            await WriteStamp(cacheDirectory).ConfigureAwait(false);
        }

        private static async Task DownloadFileAsync(HttpClient client, string baseUri, string file, DirectoryInfo destination)
        {
            try
            {
                using var wc = new HttpClient();
                var uri = $"{baseUri}/{file}".Replace(" ", "%20");
                Logging.WriteLine($"Get: {uri}");

                var fileContent = await wc.GetStringAsync(uri).ConfigureAwait(false);
                Directory.CreateDirectory(new FileInfo(Path.Combine(destination.FullName, file)).Directory.FullName);
                await File.WriteAllTextAsync(Path.Combine(destination.FullName, file), fileContent).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.ToString());
            }
        }

        private static async Task WriteStamp(DirectoryInfo destination)
        {
            var hashAsync = await HashDirectoryAsync(destination).ConfigureAwait(false);

            var json = new JsonObject();
            json["hash"] = hashAsync;

            await File.WriteAllTextAsync(Path.Combine(destination.FullName, "stamp.json"), json.ToJsonString()).ConfigureAwait(false);
        }


    }
}
