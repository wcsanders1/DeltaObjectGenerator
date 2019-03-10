using System;

namespace DeltaObjectGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IgnoreDeltaOnDefaultAttribute : Attribute
    {}
}
