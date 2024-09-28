using System.Text.Json.Serialization;

namespace ChecklistTracker.Layout.HashFrog
{
    public class TrackerConfig
    {
        [JsonInclude]
        public string? Game { get; set; }
        [JsonInclude]
        public bool EnableLogic = false;
    }
}
