using System.Text.Json.Serialization;

namespace ChecklistTracker.Layout.HashFrog.Elements
{
    [JsonDiscriminatorValue("element")]
    public record Element
    {
        [JsonInclude]
        public string elementId { get; set; } = string.Empty;
        [JsonInclude]
        public string displayName { get; set; } = string.Empty;
        [JsonInclude]
        public string[] label { get; set; } = { };
        [JsonInclude]
        public int labelStartingIndex { get; set; } = 0;
        [JsonInclude]
        public int[] size { get; set; } = { 25, 25 };
        [JsonInclude]
        public string[] icons { get; set; } = { };
        [JsonInclude]
        public int selectedStartingIndex { get; set; } = 0;
        [JsonInclude]
        public int[] countConfig { get; set; } = { 0, 5 };
        [JsonInclude]
        public bool receiver { get; set; } = false;
        [JsonInclude]
        public bool dragCurrent { get; set; } = false;
        [JsonInclude]
        public string[] items { get; set; } = { };
        public int[] position { get; set; } = { };

        public int LabelCurrentIndex;
        public string LabelCurrent { get { return label[LabelCurrentIndex]; } }
    }
}
