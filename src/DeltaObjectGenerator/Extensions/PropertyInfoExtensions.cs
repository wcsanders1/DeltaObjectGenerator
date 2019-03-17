using DeltaObjectGenerator.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeltaObjectGenerator.Extensions
{
    internal static class PropertyInfoExtensions
    {
        public static bool IgnoreDeltaBecauseDefault(this PropertyInfo propertyInfo,
            List<PropertyInfo> propertiesToIgnoreOnDefault, object newValue, bool ignorePropertyOnDefault)
        {
            if (!propertiesToIgnoreOnDefault.Contains(propertyInfo) && !ignorePropertyOnDefault)
            {
                return false;
            }

            if (!(newValue is IComparable comparableNewValue) || comparableNewValue == null)
            {
                return true;
            }

            var comparableDefaultValue = propertyInfo.PropertyType.GetComparableDefaultValue();

            return comparableNewValue.CompareTo(comparableDefaultValue) == 0;
        }

        public static bool IsIndexed(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetIndexParameters().Any();
        }

        public static string GetAlias(this PropertyInfo propertyInfo)
        {
            return propertyInfo
                .GetCustomAttribute<DeltaObjectAliasAttribute>()
                ?.Alias;
        }
    }
}
