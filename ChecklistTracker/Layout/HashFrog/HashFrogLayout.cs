using ChecklistTracker.Layout.HashFrog.Elements;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Layout.HashFrog
{
    internal class HashFrogLayout
    {
        [JsonInclude]
        public string id;
        [JsonInclude]
        public LayoutConfig layoutConfig;
        [JsonInclude]
        public IEnumerable<Element> components;
    }
}
