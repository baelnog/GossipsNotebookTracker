namespace ChecklistTracker.Config
{
    class Item
    {
        public CollectionType collection { get; set; } = CollectionType.Default;
        public ItemType type { get; set; } = ItemType.Default;
        public string[] images { get; set; } = { };
        public int? max_count { get; set; }
    }
}
