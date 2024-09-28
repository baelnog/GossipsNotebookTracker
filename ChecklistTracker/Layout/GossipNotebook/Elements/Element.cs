using System.Text.Json.Serialization;

namespace ChecklistTracker.Layout.GossipNotebook.Elements
{
    [JsonDiscriminatorValue("element")]
    public record Element
    {
        [JsonInclude]
        public string ElementId { get; set; } = string.Empty;
        [JsonInclude]
        public string[] Label { get; set; } = { };
        [JsonInclude]
        public int LabelStartingIndex { get; set; } = 0;
        [JsonInclude]
        public int[] Size { get; set; } = { 25, 25 };
        [JsonInclude]
        public string[] Icons { get; set; } = { };
        [JsonInclude]
        public int SelectedStartingIndex { get; set; } = 0;
        [JsonInclude]
        public int[] CountConfig { get; set; } = { 0, 5 };
        [JsonInclude]
        public bool Receiver { get; set; } = false;
        [JsonInclude]
        public bool DragCurrent { get; set; } = false;
        [JsonInclude]
        public string[] Items { get; set; } = { };
        [JsonInclude]
        public int[] Position { get; set; } = { };

        public int LabelCurrentIndex;
        public string LabelCurrent { get { return Label[LabelCurrentIndex]; } }
    }
}
