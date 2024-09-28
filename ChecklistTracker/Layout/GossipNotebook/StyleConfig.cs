using System.Text.Json.Serialization;

namespace ChecklistTracker.Layout.GossipNotebook
{
    public class StyleConfig : IStyle
    {
        [JsonInclude]
        public int? Width { get; set; }
        [JsonInclude]
        public int? Height { get; set; }
        [JsonInclude]
        public string? BackgroundColor { get; set; } = "#000000";
        [JsonInclude]
        public string? FontColor { get; set; }
        [JsonInclude]
        public string? FontFamilty { get; set; }
        [JsonInclude]
        public int? FontSize { get; set; }
        [JsonInclude]
        public string? Padding { get; set; }

        [JsonInclude]
        public string? Title { get; set; }
    }
}
