using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Layout.HashFrog
{
    public class LayoutConfig
    {
        [JsonInclude]
        public string name;
        [JsonInclude]
        public string backgroundColor;
        [JsonInclude]
        public int width;
        [JsonInclude]
        public int height;
        [JsonInclude]
        public string fontFamilty;
        [JsonInclude]
        public int? fontSize;
        [JsonInclude]
        public string fontColor;
    }
}
