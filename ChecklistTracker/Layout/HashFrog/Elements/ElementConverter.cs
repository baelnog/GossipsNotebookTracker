using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistTracker.Layout.HashFrog.Elements
{
    public class ElementConverter : PolymorphicJsonConverter<Element> {
        public ElementConverter() : base(
            "type",
            GetTypeDiscriminator,
            new List<Type> { typeof(Element), typeof(ElementTable), typeof(HintTable), typeof(Label), typeof(LocationHint) }
        ) { }

        private static string GetTypeDiscriminator(Type type)
        {
            return type.GetCustomAttribute<JsonDiscriminatorValueAttribute>().DiscriminatorValue;
        }
    }
}
