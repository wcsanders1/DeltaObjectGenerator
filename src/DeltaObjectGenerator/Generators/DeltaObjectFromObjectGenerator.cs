using DeltaObjectGenerator.Caches;
using DeltaObjectGenerator.Extensions;
using System;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Geneators
{
    internal static class DeltaObjectFromObjectGenerator
    {
        public static Dictionary<string, string> GetDeltaObject<T>(T originalObject, T newObject)
        {
            var properties = TypeCache.GetPropertyInfo<T>();
            var propertiesToNotUpdateWhenDefault = TypeCache.GetPropertiesToNotUpdateWhenDefault<T>();
            var deltaObject = new Dictionary<string, string>();

            foreach (var property in properties)
            {
                var newValueStr = property.GetValue(newObject)?.ToString();
                if (!property.CanUpdate(propertiesToNotUpdateWhenDefault, newValueStr))
                {
                    continue;
                }

                var originalValueStr = property.GetValue(originalObject)?.ToString();
                if (!string.Equals(originalValueStr, newValueStr, StringComparison.InvariantCulture))
                {
                    deltaObject.Add(property.Name, newValueStr);
                }
            }

            return deltaObject;
        }
    }
}
