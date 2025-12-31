using ChecklistTracker.Layout.HashFrog.Elements;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Layout.HashFrog
{
    internal class HashFrogLayout
    {
        [JsonInclude]
        public TrackerConfig layoutConfig = new TrackerConfig();
        [JsonInclude]
        public IEnumerable<Element> components = new List<Element>();
    }
}
