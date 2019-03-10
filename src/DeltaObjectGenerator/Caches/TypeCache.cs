using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeltaObjectGenerator.Caches
{
    internal static class TypeCache
    {
        private static ConcurrentDictionary<string, List<PropertyInfo>> PropertyInfoByTypeName { get; }

        private static readonly List<Type> AcceptedNonPrimitiveTypes = new List<Type>
        {
            typeof(decimal),
            typeof(string),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
        };

        static TypeCache()
        {
            PropertyInfoByTypeName = new ConcurrentDictionary<string, List<PropertyInfo>>();
        }

        public static List<PropertyInfo> GetPropertyInfo<T>()
        {
            var currentType = typeof(T);
            var typeName = currentType.FullName;

            if (PropertyInfoByTypeName.TryGetValue(typeName, out var cachedPropertyInfo))
            {
                return cachedPropertyInfo;
            }

            var propertyInfo = currentType
                .GetProperties()
                .Select(p => 
                {
                    var pType = p.PropertyType;
                    if (pType.IsPrimitive || pType.IsEnum || AcceptedNonPrimitiveTypes.Contains(pType))
                    {
                        return p;
                    }

                    return null;
                })
                .Where(p => p != null)
                .ToList();

            PropertyInfoByTypeName.AddOrUpdate(typeName, propertyInfo, (_, pi) => pi);

            return propertyInfo;
        }
    }
}
