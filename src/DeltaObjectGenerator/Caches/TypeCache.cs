using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeltaObjectGenerator.Caches
{
    internal static class TypeCache
    {
        private static ConcurrentDictionary<Type, List<PropertyInfo>> PropertyInfoByType { get; }

        private static readonly Dictionary<Type, string> StringifiedDefaultValuesByType = 
            new Dictionary<Type, string>
        {
            {typeof(bool), default(bool).ToString() },
            {typeof(byte), default(byte).ToString() },
            {typeof(char), default(char).ToString() },
            {typeof(decimal), default(decimal).ToString() },
            {typeof(double), default(double).ToString() },
            {typeof(float), default(float).ToString() },
            {typeof(int), default(int).ToString() },
            {typeof(long), default(long).ToString() },
            {typeof(sbyte), default(sbyte).ToString() },
            {typeof(short), default(short).ToString() },
            {typeof(uint), default(uint).ToString() },
            {typeof(ulong), default(ulong).ToString() },
            {typeof(ushort), default(ushort).ToString() }
        };

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
            PropertyInfoByType = new ConcurrentDictionary<Type, List<PropertyInfo>>();
        }

        public static List<PropertyInfo> GetPropertyInfo<T>()
        {
            var currentType = typeof(T);
            if (PropertyInfoByType.TryGetValue(currentType, out var cachedPropertyInfo))
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

            PropertyInfoByType.AddOrUpdate(currentType, propertyInfo, (_, pi) => pi);

            return propertyInfo;
        }

        public static T GetDefaultValue<T>()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
