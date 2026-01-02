using ChecklistTracker.Config.Layout.GossipNotebook.Components;
using ChecklistTracker.Layout;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Layout.GossipNotebook.Elements
{
    [JsonDiscriminatorValue("element")]
    public record Element : IRegion
    {
        [JsonInclude]
        public Size size { get; set; } = new Size { Width = 25, Height = 25 };
        [JsonInclude]
        public string[] icons { get; set; } = { };
        [JsonInclude]
        public Position position { get; set; } = new Position();
        [JsonInclude]
        public string padding = "0px 0px";
    }
}
