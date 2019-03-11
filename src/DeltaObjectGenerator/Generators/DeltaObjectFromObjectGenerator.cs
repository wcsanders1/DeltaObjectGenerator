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
            var properties = TypeCache.GetDeltaPropertyInfo<T>();
            var propertiesToIgnoreOnDefault = TypeCache.GetPropertiesToIgnoreOnDefault<T>();
            var deltaObjects = new List<DeltaObject>();

            foreach (var property in properties)
            {
                var newValueStr = property.GetValue(newObject)?.ToString();
                if (property.IgnoreDelta(propertiesToIgnoreOnDefault, newValueStr))
                {
                    continue;
                }

                var originalValueStr = property.GetValue(originalObject)?.ToString();
                if (!string.Equals(originalValueStr, newValueStr, StringComparison.InvariantCulture))
                {
                    deltaObjects.Add(new DeltaObject
                    {
                        PropertyName = property.Name,
                        
                        //TODO1: Make this respect a custom attribute
                        PropertyAlias = property.Name,

                        OriginalValue = originalValueStr,
                        NewValue = newValueStr
                    });
                }
            }

            return deltaObjects;
        }
    }
}
