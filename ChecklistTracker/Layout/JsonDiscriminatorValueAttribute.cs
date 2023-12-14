using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistTracker.Layout
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    internal class JsonDiscriminatorValueAttribute : Attribute {
        internal string DiscriminatorValue { get; }
        public JsonDiscriminatorValueAttribute(string discriminatorValue = null)
        {
            DiscriminatorValue = discriminatorValue;
        }
    }
}
