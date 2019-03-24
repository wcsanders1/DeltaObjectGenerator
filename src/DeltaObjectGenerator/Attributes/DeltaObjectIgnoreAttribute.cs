using System;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Attributes
{
    /// <summary>
    /// Ignores a property for purposes of generating a <see cref="List{DeltaObject}"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DeltaObjectIgnoreAttribute : Attribute
    { }
}
