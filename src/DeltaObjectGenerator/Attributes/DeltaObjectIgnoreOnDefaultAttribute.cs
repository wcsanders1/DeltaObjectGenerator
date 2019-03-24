using System;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Attributes
{
    /// <summary>
    /// Ignores a property whose value is its default for purposes of generating a <see cref="List{DeltaObject}"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, 
        AllowMultiple = false, Inherited = true)]
    public class DeltaObjectIgnoreOnDefaultAttribute : Attribute
    { }
}
