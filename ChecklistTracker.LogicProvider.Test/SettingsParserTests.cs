using ChecklistTracker.Config;
using ChecklistTracker.Config.Settings.SettingsTypes;
using System.Text.Json;

namespace ChecklistTracker.LogicProvider.Test
{
    [TestClass]
    public class SettingsParserTests
    {
        [TestMethod]
        public void ParseAdultTradeItems()
        {
            foreach (var item in AdultTradeItemExtensions.Items)
            {
                var json = JsonSerializer.Serialize(item, TrackerConfig.JsonSerializerOptions);

                var deserialized = JsonSerializer.Deserialize<AdultTradeItem>(json);

                Assert.AreEqual(item, deserialized);
            }
        }
    }
}
