using DeltaObjectGenerator.Caches;
using DeltaObjectGenerator.Extensions;
using DeltaObjectGenerator.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeltaObjectGenerator.Generators
{
    internal static class DeltaObjectFromObjectGenerator
    {
        public static List<DeltaObject> GetDeltaObjects<T>(T originalObject, T newObject)
        {
            var deltaProperties = TypeCache.GetDeltaPropertyInfo<T>();
            var propertiesToIgnoreOnDefault = TypeCache.GetPropertiesToIgnoreOnDefault<T>();
            var ignorePropertiesOnDefault = TypeCache.IgnorePropertiesOnDefault<T>();
            
            return deltaProperties.Select(deltaProperty => 
                GetDeltaObject(deltaProperty, originalObject, newObject, 
                    propertiesToIgnoreOnDefault, ignorePropertiesOnDefault))
                .Where(d => d != null)
                .ToList();
        }

        private static DeltaObject GetDeltaObject<T>(DeltaProperty deltaProperty, T originalObject, 
            T newObject, List<PropertyInfo> propertiesToIgnoreOnDefault, bool ignorePropertiesOnDefault)
        {
            var propertyInfo = deltaProperty.PropertyInfo;
            var newValue = propertyInfo.GetValue(newObject);
            if (propertyInfo.IgnoreDeltaBecauseDefault(propertiesToIgnoreOnDefault, 
                newValue, ignorePropertiesOnDefault))
            {
                return null;
            }

            var originalValue = deltaProperty.PropertyInfo.GetValue(originalObject);
            if (deltaProperty.HasDelta(originalValue, newValue))
            {
                return new DeltaObject
                {
                    ConversionStatus =ConversionStatus.Valid,
                    PropertyName = propertyInfo.Name,
                    PropertyAlias = deltaProperty.Alias,
                    OriginalValue = originalValue,
                    NewValue = newValue,
                    StringifiedOriginalValue = originalValue?.ToString(),
                    StringifiedNewValue = newValue?.ToString()
                };
            }

            return null;
        }
    }
}
