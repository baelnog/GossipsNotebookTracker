using System.Collections.Generic;

namespace ChecklistTracker.Layout.HashFrog.Elements
{
    [JsonDiscriminatorValue("table")]
    public record ElementTable : Element
    {
        public int columns { get; set; } = 1;
        public IEnumerable<string> elements { get; set; } = Enumerable.Empty<string>();
        public int[] elementsSize { get; set; } = [];
        public string padding { get; set; } = "0px";

        public IEnumerable<string> quickFillLabels { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> quickFillImages { get; set; } = Enumerable.Empty<string>();
    }
}
