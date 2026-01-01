using ChecklistTracker.Layout;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Layout.GossipNotebook.Elements
{
    [JsonDiscriminatorValue("element")]
    public record Element
    {
        [JsonInclude]
        public string elementId { get; set; } = string.Empty;
        [JsonInclude]
        public int[] size { get; set; } = { 25, 25 };
        [JsonInclude]
        public string[] icons { get; set; } = { };
        [JsonInclude]
        public int[] position { get; set; } = { };
    }
}
