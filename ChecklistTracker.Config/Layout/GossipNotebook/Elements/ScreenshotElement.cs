using ChecklistTracker.Layout;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Layout.GossipNotebook.Elements
{

    [JsonDiscriminatorValue("screenshot")]
    public record ScreenshotElement : Element
    {
        [JsonInclude]
        public int[] screenshotSize { get; set; } = { 324, 574 };

        [JsonInclude]
        public int[][] clipRegion { get; set; } = [
            [0,0],
            [100,100]
        ];
    }
}
