using DeltaObjectGenerator.Models;
using System;

namespace DeltaObjectGenerator.Attributes
{
    /// <summary>
    /// Sets the value of <see cref="DeltaObject.PropertyAlias"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// public class Customer
    /// {
    ///     [DeltaObjectAlias("last_name")]
    ///     public string LastName { get; set; }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DeltaObjectAliasAttribute : Attribute
    {
        internal string Alias { get; }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public DeltaObjectAliasAttribute(string alias)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Alias = alias;
        }
    }
}
