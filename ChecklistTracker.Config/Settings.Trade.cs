using ChecklistTracker.Config.SettingsTypes;
using ChecklistTracker.LogicProvider.DataFiles.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Config;

public partial class Settings
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
