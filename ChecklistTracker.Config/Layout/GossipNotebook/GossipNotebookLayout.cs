using ChecklistTracker.Config.Layout.GossipNotebook.Components;
using ChecklistTracker.Config.Layout.GossipNotebook.Elements;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Layout.GossipNotebook;

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
                TypeInfoResolver = GossipNotebokJsonContext.Default,
                Converters = {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
                    new ElementConverter(),
                    new PaddingConverter(),
                    new PositionConverter(),
                    new SizeConverter(),
                }
            }) ?? throw new Exception("Failed to parse layout file.");
    }
}

[JsonSerializable(typeof(GossipNotebookLayout))]
[JsonSerializable(typeof(Element))]
[JsonSerializable(typeof(ElementTable))]
[JsonSerializable(typeof(HintTable))]
[JsonSerializable(typeof(Label))]
[JsonSerializable(typeof(ScreenshotElement))]
[JsonSerializable(typeof(Padding))]
[JsonSerializable(typeof(ConcretePadding))]
[JsonSerializable(typeof(Position))]
[JsonSerializable(typeof(ConcretePosition))]
[JsonSerializable(typeof(Size))]
[JsonSerializable(typeof(ConcreteSize))]
[JsonSerializable(typeof(double[]))]
internal partial class GossipNotebokJsonContext : JsonSerializerContext
{ 
}
