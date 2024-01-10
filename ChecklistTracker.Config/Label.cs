using System.Collections.Generic;

namespace ChecklistTracker.Config
{
    public class Label
    {
        public string name { get; set; } = string.Empty;
        public List<string> alias { get; set; } = new List<string>();
    }
}
