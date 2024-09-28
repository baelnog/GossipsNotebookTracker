namespace ChecklistTracker.Layout.HashFrog.Elements
{
    [JsonDiscriminatorValue("hinttable")]
    public record HintTable : Element, ISometimesHintTable, ILocationHintTable, IEntranceTable
    {
        public HintType hintType { get; set; }
        public int width { get; set; }
        public int hintNumber { get; set; }
        public int columns { get; set; }
        public string padding { get; set; } = "0px";
        public string labels { get; set; } = "sometimes";
        public string color { get; set; } = "FFFFFF";
        public string backgroundColor { get; set; } = "333333";
        public string itemIconSet { get; set; } = "sometimes";
        public string bossIconSet { get; set; } = "bosses";
        public string[] bossIcons { get; set; }
        public bool showIcon { get; set; } = true;
        public bool inverted { get; set; } = false;
        public bool showBoss { get; set; } = true;
        public int bossCount { get; set; } = 1;
        public bool showItems { get; set; } = true;
        public int itemCount { get; set; } = 4;
        public int[] itemSize { get; set; } = { 24, 24 };
        public bool dual { get; set; } = false;
        public bool allowScroll { get; set; } = false;
        public string placeholderText { get; set; } = "";
    }
}
