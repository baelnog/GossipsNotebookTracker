using ChecklistTracker.Layout;

namespace ChecklistTracker.Config.Layout.GossipNotebook.Elements
{
    public interface IHintTable : ITextStyle
    {
        public HintType hintType { get; set; }
        public int width { get; set; }
        public int hintNumber { get; set; }
        public int columns { get; set; }
        public string labels { get; set; }
        public string[]? labelsSet { get; set; }
        public int[] itemSize { get; set; }
        public bool showCounter { get; set; }

        public string itemIconSet { get; set; }
        public string bossIconSet { get; set; }

        public bool inverted { get; set; }

        public string placeholderText { get; set; }
    }

    public enum HintType
    {
        Sometimes,
        Location,
        Entrance
    }
}
