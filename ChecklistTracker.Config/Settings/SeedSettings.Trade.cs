using ChecklistTracker.Config.Settings.SettingsTypes;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Settings;

public partial class SeedSettings
{
    [JsonPropertyName("complete_mask_quest")]
    public bool CompleteMaskQuest { get; set; }

    [JsonPropertyName("shuffle_child_trade")]
    public ISet<ChildTradeItem> ChildTradeItemStart { get; set; } = new HashSet<ChildTradeItem>();

    public ISet<string> ChildTradeItemStartLogic { get => ChildTradeItemStart.Select(i => i.ToLogicString()).ToHashSet(); }

    [JsonPropertyName("adult_trade_shuffle")]
    public bool FullAdultTradeShuffle { get; set; }

    [JsonPropertyName("adult_trade_start")]
    public ISet<AdultTradeItem> AdultTradeItemStart { get; set; } = new HashSet<AdultTradeItem>();

    public ISet<string> AdultTradeItemStartLogic { get => AdultTradeItemStart.Select(i => i.ToLogicString()).ToHashSet(); }
}
