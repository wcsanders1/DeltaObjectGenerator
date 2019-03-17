using DeltaObjectGenerator.Caches;
using DeltaObjectGenerator.Extensions;
using DeltaObjectGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeltaObjectGenerator.Geneators
{
    internal static class DeltaObjectFromObjectGenerator
    {
        public static List<DeltaObject> GetDeltaObjects<T>(T originalObject, T newObject)
        {
            if (originalObject == null)
            {
                throw new ArgumentNullException(nameof(originalObject));
            }

            if (newObject == null)
            {
                throw new ArgumentNullException(nameof(newObject));
            }

            var deltaProperties = TypeCache.GetDeltaPropertyInfo<T>();
            var propertiesToIgnoreOnDefault = TypeCache.GetPropertiesToIgnoreOnDefault<T>();
            
            return deltaProperties.Select(deltaProperty => 
                GetDeltaObject(deltaProperty, originalObject, newObject, propertiesToIgnoreOnDefault))
            .Where(d => d != null)
            .ToList();
        }

        private static DeltaObject GetDeltaObject<T>(DeltaProperty deltaProperty, T originalObject, 
            T newObject, List<PropertyInfo> propertiesToIgnoreOnDefault)
        {
            var propertyInfo = deltaProperty.PropertyInfo;
            var newValue = propertyInfo.GetValue(newObject);
            if (propertyInfo.IgnoreDeltaBecauseDefault(propertiesToIgnoreOnDefault, newValue))
            {
                return null;
            }

            var originalValue = deltaProperty.PropertyInfo.GetValue(originalObject);
            if (deltaProperty.HasDelta(originalValue, newValue))
            {
                return new DeltaObject
                {
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
