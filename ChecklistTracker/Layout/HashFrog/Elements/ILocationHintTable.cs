using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistTracker.Layout.HashFrog.Elements
{
    internal interface ILocationHintTable : IHintTable
    {
        public bool showBoss { get; set; }
        public string[]? bossIcons { get; set; }
        public int bossCount { get; set; }
        public bool showItems { get; set; }
        public int itemCount { get; set; }
        public bool allowScroll { get; set; }
    }
}
