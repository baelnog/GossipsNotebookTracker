using System.Text.Json.Serialization;

namespace ChecklistTracker.Layout.HashFrog.Elements
{

    [JsonDiscriminatorValue("screenshot")]
    public record ScreenshotElement : Element
    {
        [JsonInclude]
        public int[] screenshotSize { get; set; } = { 324, 574 };

        [JsonInclude]
        public int graphicsCardIndex = 0;
        [JsonInclude]
        public int screenIndex = 0;

        [JsonInclude]
        public int[][] clipRegion { get; set; } = [
            [0,0],
            [100,100]
        ];
    }
}
