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
        public string? TextBackgroundColor { get; set; } = "#000000";
        [JsonInclude]
        public string? TextColor { get; set; } = "#FFFFFF";
        [JsonInclude]
        public string? FontFamily { get; set; } = "Segoe UI";
        [JsonInclude]
        public string? FontStyle { get; set; } = "Normal";
        [JsonInclude]
        public string? FontWeight { get; set; } = "Bold";
        [JsonInclude]
        public double? FontSize { get; set; } = 12;
        [JsonInclude]
        public string? Padding { get; set; }
        [JsonInclude]
        public string? Title { get; set; }
        [JsonInclude]
        public double? TextBackgroundOpacity { get; set; } = 1.0;
    }
}
