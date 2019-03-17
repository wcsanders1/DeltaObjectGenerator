using System;

namespace DeltaObjectGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, 
        AllowMultiple = false, Inherited = true)]
    public class DeltaObjectIgnoreOnDefaultAttribute : Attribute
    {}
}
