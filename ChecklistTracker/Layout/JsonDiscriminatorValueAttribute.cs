using System;

namespace ChecklistTracker.Layout
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    internal class JsonDiscriminatorValueAttribute : Attribute
    {
        internal string DiscriminatorValue { get; }
        public JsonDiscriminatorValueAttribute(string discriminatorValue = null)
        {
            DiscriminatorValue = discriminatorValue;
        }
    }
}
