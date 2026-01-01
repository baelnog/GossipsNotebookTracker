using ChecklistTracker.Layout;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Layout.GossipNotebook.Elements
{
    [JsonDiscriminatorValue("table")]
    public record ElementTable : Element, ITextStyle
    {
        public int columns { get; set; } = 1;
        public IEnumerable<string> elements { get; set; } = Enumerable.Empty<string>();
        public int[] elementsSize { get; set; } = [];

        public IEnumerable<string> quickFillLabels { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> quickFillImages { get; set; } = Enumerable.Empty<string>();

        [JsonPropertyName("fontSize")]
        public double? FontSize { get; set; }

        [JsonPropertyName("textColor")]
        public string? TextColor { get; set; }

        [JsonPropertyName("backgroundColor")]
        public string? TextBackgroundColor { get; set; }

        [JsonPropertyName("fontFamily")]
        public string? FontFamily { get; set; }

        [JsonPropertyName("fontStyle")]
        public string? FontStyle { get; set; }

        [JsonPropertyName("fontWeight")]
        public string? FontWeight{ get; set; }

        [JsonPropertyName("TextBackgroundOpacity")]
        public double? TextBackgroundOpacity { get; set; }
    }
}
