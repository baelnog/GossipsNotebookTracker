namespace ChecklistTracker.Config.Layout.GossipNotebook.Elements
{
    public interface ILocationHintTable : IHintTable
    {
        public bool showBoss { get; set; }
        public string[]? bossIcons { get; set; }
        public int bossCount { get; set; }
        public bool showItems { get; set; }
        public int itemCount { get; set; }
        public bool allowScroll { get; set; }
    }
}
