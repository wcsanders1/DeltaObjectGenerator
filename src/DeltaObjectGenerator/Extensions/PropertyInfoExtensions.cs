using DeltaObjectGenerator.Attributes;
using DeltaObjectGenerator.Caches;
using System.Collections.Generic;
using System.Reflection;

namespace DeltaObjectGenerator.Extensions
{
    internal static class PropertyInfoExtensions
    {
        public static bool IgnoreDelta(this PropertyInfo propertyInfo,
            List<PropertyInfo> propertiesToNotUpdateWhenDefault, string stringifiedNewValue)
        {
            if (propertiesToNotUpdateWhenDefault.Contains(propertyInfo))
            {
                return TypeCache.GetStringifiedDefaultValueForType(propertyInfo.PropertyType) == stringifiedNewValue;
            }

            return false;
        }

        public static string GetAlias(this PropertyInfo propertyInfo)
        {
            return propertyInfo
                .GetCustomAttribute<DeltaObjectAliasAttribute>()
                ?.Alias;
        }
    }
}
