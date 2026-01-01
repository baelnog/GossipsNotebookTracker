using ChecklistTracker.Layout;
using System;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Layout.GossipNotebook.Elements
{
    [JsonDiscriminatorValue("label")]
    public record Label : Element, ITextStyle
    {
        [JsonInclude]
        [JsonPropertyName("text")]
        public string Text { get; set; } = "";

        [JsonInclude]
        [JsonPropertyName("fontSize")]
        public double? FontSize { get; set; }

        [JsonInclude]
        [JsonPropertyName("fontFamily")]
        public string? FontFamily { get; set; }

        [JsonInclude]
        [JsonPropertyName("fontStyle")]
        public string? FontStyle { get; set; }

        [JsonInclude]
        [JsonPropertyName("fontWeight")]
        public string? FontWeight { get; set; }

        [JsonInclude]
        [JsonPropertyName("textColor")]
        public string? TextColor { get; set; }

        [JsonInclude]
        [JsonPropertyName("textBackgroundColor")]
        public string? TextBackgroundColor { get; set; }

        [JsonInclude]
        [JsonPropertyName("textBackgroundOpacity")]
        public double? TextBackgroundOpacity { get; set; }
    }
}
