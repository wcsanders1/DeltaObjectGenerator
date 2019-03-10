using DeltaObjectGenerator.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeltaObjectGenerator.Caches
{
    internal static class TypeCache
    {
        private static ConcurrentDictionary<Type, List<PropertyInfo>> DeltaPropertiesByType { get; }
        private static ConcurrentDictionary<Type, List<PropertyInfo>> PropertiesToIgnoreWhenDefaultByType { get; }

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
            {typeof(ushort), default(ushort).ToString() },
            {typeof(DateTime), default(DateTime).ToString() },
            {typeof(DateTimeOffset), default(DateTimeOffset).ToString() },
            {typeof(TimeSpan), default(TimeSpan).ToString() },
            {typeof(Guid), default(Guid).ToString() },
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
            DeltaPropertiesByType = new ConcurrentDictionary<Type, List<PropertyInfo>>();
            PropertiesToIgnoreWhenDefaultByType = new ConcurrentDictionary<Type, List<PropertyInfo>>();
        }

        public static List<PropertyInfo> GetDeltaPropertyInfo<T>()
        {
            var type = typeof(T);
            if (DeltaPropertiesByType.TryGetValue(type, out var cachedPropertyInfo))
            {
                return cachedPropertyInfo;
            }

            var propertyInfo = type
                .GetProperties()
                .Select(pi => 
                {
                    if (Attribute.IsDefined(pi, typeof(IgnoreDeltaAttribute)))
                    {
                        return null;
                    }

                    var pType = pi.PropertyType;
                    if (pType.IsPrimitive || pType.IsEnum || AcceptedNonPrimitiveTypes.Contains(pType))
                    {
                        return pi;
                    }

                    return null;
                })
                .Where(pi => pi != null)
                .ToList();

            DeltaPropertiesByType.AddOrUpdate(type, propertyInfo, (_, pi) => pi);

            return propertyInfo;
        }

        public static List<PropertyInfo> GetPropertiesToIgnoreOnDefault<T>()
        {
            var type = typeof(T);
            if (PropertiesToIgnoreWhenDefaultByType.TryGetValue(type, out var cachedPropertyInfo))
            {
                return cachedPropertyInfo;
            }

            var propertiesToNotUpdateWhenNull = type
                .GetProperties()
                .Where(pi => Attribute.IsDefined(pi, typeof(IgnoreDeltaOnDefaultAttribute)))
                .ToList();

            PropertiesToIgnoreWhenDefaultByType.AddOrUpdate(type,
                propertiesToNotUpdateWhenNull, (_, pi) => pi);

            return propertiesToNotUpdateWhenNull;
        }

        public static string GetStringifiedDefaultValueForType(Type type)
        {
            if (!type.IsValueType)
            {
                return null;
            }

            if (type.IsEnum)
            {
                return 0.ToString();
            }

            if (StringifiedDefaultValuesByType.TryGetValue(type, out var val))
            {
                return val;
            }

            return null;
        }
    }
}
