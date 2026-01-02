using ChecklistTracker.Layout;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Layout.GossipNotebook.Elements
{
    [JsonDiscriminatorValue("hinttable")]
    public record HintTable : Element, ISometimesHintTable, ILocationHintTable, IEntranceTable, ITextStyle
    {
        public HintType hintType { get; set; }
        public int width { get; set; }
        public int hintNumber { get; set; }
        public int columns { get; set; } = 1;
        public string labels { get; set; } = "sometimes";
        public string[]? labelsSet { get; set; } = null;
        public string itemIconSet { get; set; } = "sometimes";
        public string bossIconSet { get; set; } = "bosses";
        public string[]? bossIcons { get; set; }
        public bool showIcon { get; set; } = true;
        public bool inverted { get; set; } = false;
        public bool showBoss { get; set; } = false;
        public int bossCount { get; set; } = 1;
        public bool showCounter { get; set; } = false;
        public bool showItems { get; set; } = true;
        public int itemCount { get; set; } = 4;
        public int[] itemSize { get; set; } = { 24, 24 };
        public bool dual { get; set; } = false;
        public bool allowScroll { get; set; } = false;
        public string placeholderText { get; set; } = "";

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
        public string? FontWeight { get; set; }

        [JsonPropertyName("TextBackgroundOpacity")]
        public double? TextBackgroundOpacity { get; set; }
    }
}
