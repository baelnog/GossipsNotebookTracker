namespace ChecklistTracker.Layout.HashFrog.Elements
{
    [JsonDiscriminatorValue("locationhint")]
    public record LocationHint : Element
    {
        public int width { get; set; }
        public string color { get; set; } = "#FFFF00";
        public string backgroundColor { get; set; } = "#333333";
        public bool showBoss { get; set; } = true;
        public string[]? bossIcons { get; set; }
        public bool showItems { get; set; }
        public string[]? itemsIcons { get; set; }
    }
}
