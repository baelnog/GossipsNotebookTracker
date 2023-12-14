using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Layout.HashFrog.Elements
{
    [JsonDiscriminatorValue("table")]
    public record ElementTable : Element
    {
        public int columns { get; set; }
        public IEnumerable<string> elements { get; set; }
        public int[] elementsSize { get; set; }
        public string padding { get; set; }
    }
}
