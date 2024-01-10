using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.LogicProvider.DataFiles
{
    public class Region
    {
        [JsonPropertyName("region_name")]
        public string RegionName { get; set; }

        [JsonPropertyName("scene")]
        public string Scene { get; set; } = null;

        [JsonPropertyName("dungeon")]
        public string Dungeon { get; set; } = null;

        [JsonPropertyName("hint")]
        public string Hint { get; set; } = null;

        [JsonPropertyName("alt_hint")]
        public string AltHint { get; set; } = null;

        [JsonPropertyName("is_boss_room")]
        public bool IsBossRoom { get; set; } = false;

        [JsonPropertyName("time_passes")]
        public bool TimePasses { get; set; } = false;

        [JsonPropertyName("save_warp")]
        public string SaveWarp { get; set; } = null;

        [JsonPropertyName("events")]
        public IDictionary<string, string> Events { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("locations")]
        public IDictionary<string, string> Locations { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("exits")]
        public IDictionary<string, string> Exits { get; set; } = new Dictionary<string, string>();
    }
}
