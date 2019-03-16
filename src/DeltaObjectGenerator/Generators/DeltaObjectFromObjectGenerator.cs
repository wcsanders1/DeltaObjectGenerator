using DeltaObjectGenerator.Caches;
using DeltaObjectGenerator.Extensions;
using DeltaObjectGenerator.Models;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Geneators
{
    internal static class DeltaObjectFromObjectGenerator
    {
        public static List<DeltaObject> GetDeltaObjects<T>(T originalObject, T newObject)
        {
            var deltaProperties = TypeCache.GetDeltaPropertyInfo<T>();
            var propertiesToIgnoreOnDefault = TypeCache.GetPropertiesToIgnoreOnDefault<T>();
            var deltaObjects = new List<DeltaObject>();
            
            foreach (var deltaProperty in deltaProperties)
            {
                var propertyInfo = deltaProperty.PropertyInfo;
                var newValue = propertyInfo.GetValue(newObject, null);
                if (propertyInfo.IgnoreDeltaBecauseDefault(propertiesToIgnoreOnDefault, newValue))
                {
                    continue;
                }

                var originalValue = deltaProperty.PropertyInfo.GetValue(originalObject);
                if (deltaProperty.HasDelta(originalValue, newValue))
                {
                    deltaObjects.Add(new DeltaObject
                    {
                        PropertyName = propertyInfo.Name,
                        PropertyAlias = deltaProperty.Alias,
                        OriginalValue = originalValue,
                        NewValue = newValue,
                        StringifiedOriginalValue = originalValue?.ToString(),
                        StringifiedNewValue = newValue?.ToString()
                    });
                }
            }

            return deltaObjects;
        }
    }
}
