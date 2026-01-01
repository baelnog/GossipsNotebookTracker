using ChecklistTracker.Layout;
using System.Reflection;

namespace ChecklistTracker.Config.Layout.GossipNotebook.Elements
{
    public class ElementConverter : PolymorphicJsonConverter<Element>
    {
        public ElementConverter() : base(
            "type",
            GetTypeDiscriminator,
            new List<Type> { typeof(Element), typeof(ElementTable), typeof(HintTable), typeof(Label), typeof(LocationHint), typeof(ScreenshotElement) }
        )
        { }

        private static string GetTypeDiscriminator(Type type)
        {
            return type.GetCustomAttribute<JsonDiscriminatorValueAttribute>()!.DiscriminatorValue;
        }
    }
}
