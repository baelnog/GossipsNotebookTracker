using ChecklistTracker.Config.Layout.GossipNotebook.Elements;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Layout.GossipNotebook
{
    public class TrackerWindow
    {
        [JsonInclude]
        public StyleConfig Style = new StyleConfig();

        [JsonInclude]
        public IEnumerable<Element> Components = new List<Element>();
    }
}
