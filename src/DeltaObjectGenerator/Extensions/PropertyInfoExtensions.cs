using DeltaObjectGenerator.Caches;
using System.Collections.Generic;
using System.Reflection;

namespace DeltaObjectGenerator.Extensions
{
    internal static class PropertyInfoExtensions
    {
        public static bool IgnoreDelta(this PropertyInfo property,
            List<PropertyInfo> propertiesToNotUpdateWhenDefault, string stringifiedNewValue)
        {
            if (propertiesToNotUpdateWhenDefault.Contains(property))
            {
                return TypeCache.GetStringifiedDefaultValueForType(property.PropertyType) == stringifiedNewValue;
            }

            return false;
        }
    }
}
