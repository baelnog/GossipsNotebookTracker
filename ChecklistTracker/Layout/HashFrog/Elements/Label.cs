using System;
using System.Text.Json.Serialization;

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
