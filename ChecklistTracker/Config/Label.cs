using System.Collections.Generic;

namespace ChecklistTracker.Config
{
    class Label
    {
        public string name { get; set; } = string.Empty;
        public List<string> alias { get; set; } = new List<string>();
    }
}
