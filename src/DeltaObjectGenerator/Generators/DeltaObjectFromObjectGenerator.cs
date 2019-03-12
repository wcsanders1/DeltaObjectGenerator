using DeltaObjectGenerator.Caches;
using DeltaObjectGenerator.Extensions;
using DeltaObjectGenerator.Models;
using System;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Geneators
{
    internal static class DeltaObjectFromObjectGenerator
    {
        public static List<DeltaObject> GetDeltaObject<T>(T originalObject, T newObject)
        {
            var deltaProperties = TypeCache.GetDeltaPropertyInfo<T>();
            var propertiesToIgnoreOnDefault = TypeCache.GetPropertiesToIgnoreOnDefault<T>();
            var deltaObjects = new List<DeltaObject>();

            foreach (var deltaProperty in deltaProperties)
            {
                var propertyInfo = deltaProperty.PropertyInfo;
                var newValueStr = propertyInfo.GetValue(newObject)?.ToString();
                if (propertyInfo.IgnoreDelta(propertiesToIgnoreOnDefault, newValueStr))
                {
                    continue;
                }

                var originalValueStr = deltaProperty.PropertyInfo.GetValue(originalObject)?.ToString();
                if (!string.Equals(originalValueStr, newValueStr, StringComparison.InvariantCulture))
                {
                    deltaObjects.Add(new DeltaObject
                    {
                        PropertyName = propertyInfo.Name,
                        
                        //TODO1: Make this respect a custom attribute
                        PropertyAlias = propertyInfo.Name,

                        OriginalValue = originalValueStr,
                        NewValue = newValueStr
                    });
                }
            }

            return deltaObjects;
        }
    }
}
