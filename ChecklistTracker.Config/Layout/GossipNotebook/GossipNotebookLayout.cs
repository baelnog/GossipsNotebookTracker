using ChecklistTracker.Config.Layout.GossipNotebook.Elements;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Layout.GossipNotebook
{
    public class GossipNotebookLayout
    {
        [JsonInclude]
        public ChecklistTrackerConfig TrackerConfig = new ChecklistTrackerConfig();

        [JsonInclude]
        public StyleConfig Style = new StyleConfig();

        [JsonInclude]
        public IList<TrackerWindow> Windows = new List<TrackerWindow>();

        public static GossipNotebookLayout ParseLayout(string layoutContent)
        {
            return JsonSerializer.Deserialize<GossipNotebookLayout>(
                layoutContent,
                new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    PropertyNameCaseInsensitive = true,
                    Converters = {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
                        new ElementConverter(),
                    }
                }) ?? throw new Exception("Failed to parse layout file.");
        }
    }
}
