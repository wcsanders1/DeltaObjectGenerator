using System;

namespace DeltaObjectGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DeltaObjectAliasAttribute : Attribute
    {
        public string Alias { get; }
        public DeltaObjectAliasAttribute(string alias)
        {
            Alias = alias;
        }
    }
}
