using System.Text.Json.Serialization;

namespace ChecklistTracker.Layout.GossipNotebook
{
    public class ChecklistTrackerConfig
    {
        [JsonInclude]
        public string? Game { get; set; }
        [JsonInclude]
        public bool EnableLogic = false;
    }
}
