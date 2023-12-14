using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Layout.HashFrog.Elements
{
    [JsonDiscriminatorValue("label")]
    public record Label : Element
    {
        [JsonInclude]
        public string text;
        [JsonInclude]
        public int fontSize;
        [JsonInclude]
        public string color = "#FFFFFF";
        [JsonInclude]
        public string backgroundColor = "000000";
        [JsonInclude]
        public string padding = "0px 0px";

        internal double Split(string v)
        {
            throw new NotImplementedException();
        }
    }
}
