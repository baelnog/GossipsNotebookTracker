using ChecklistTracker.Config;
using ChecklistTracker.Config.SettingsTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
