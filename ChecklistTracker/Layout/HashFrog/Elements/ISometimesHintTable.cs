using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistTracker.Layout.HashFrog.Elements
{
    internal interface ISometimesHintTable : IHintTable
    {
        public bool showIcon { get; set; }
        public bool dual { get; set; }
    }
}
