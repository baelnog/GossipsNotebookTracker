using System.Collections.Generic;

namespace ChecklistTracker.Config
{
    public class ItemTable
    {
        public Dictionary<string, List<Label>> labels { get; set; }
        public Dictionary<string, List<string>> image_groups { get; set; }
        public Dictionary<string, Item> items { get; set; }
    }
}
