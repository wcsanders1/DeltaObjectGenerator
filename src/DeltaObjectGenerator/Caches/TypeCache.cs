using DeltaObjectGenerator.Attributes;
using DeltaObjectGenerator.Extensions;
using DeltaObjectGenerator.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeltaObjectGenerator.Caches
{
    internal static class TypeCache
    {
        private static ConcurrentDictionary<Type, List<DeltaProperty>> DeltaPropertiesByType { get; }
        private static ConcurrentDictionary<Type, List<PropertyInfo>> PropertiesToIgnoreOnDefaultByType { get; }
        private static ConcurrentDictionary<Type, bool> IgnorePropertiesOnDefaultByType { get; }
        
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
            DeltaPropertiesByType = new ConcurrentDictionary<Type, List<DeltaProperty>>();
            PropertiesToIgnoreOnDefaultByType = new ConcurrentDictionary<Type, List<PropertyInfo>>();
            IgnorePropertiesOnDefaultByType = new ConcurrentDictionary<Type, bool>();
        }

        public static bool IgnorePropertiesOnDefault<T>()
        {
            var type = typeof(T);
            if (IgnorePropertiesOnDefaultByType.TryGetValue(type, out var ignore))
            {
                return ignore;
            }

            ignore = type.HasAttribute<DeltaObjectIgnoreOnDefaultAttribute>();

            IgnorePropertiesOnDefaultByType.AddOrUpdate(type, ignore, (_, i) => ignore);

            return ignore;
        }

        public static List<DeltaProperty> GetDeltaPropertyInfo<T>()
        {
            var type = typeof(T);
            if (DeltaPropertiesByType.TryGetValue(type, out var cachedPropertyInfo))
            {
                return cachedPropertyInfo;
            }

            var deltaProperties = type
                .GetTypeProperties()
                .Select(pi => 
                {
                    if (pi.HasAttribute<DeltaObjectIgnoreAttribute>() || pi.IsIndexed())
                    {
                        return null;
                    }

                    return pi.PropertyType.IsDeltaInclude(AcceptedNonPrimitiveTypes) ? 
                        new DeltaProperty
                        {
                            PropertyInfo = pi,
                            Alias = pi.GetAlias() ?? pi.Name
                        } : null;
                })
                .Where(dp => dp != null)
                .ToList();

            DeltaPropertiesByType.AddOrUpdate(type, deltaProperties, (_, dps) => deltaProperties);

            return deltaProperties;
        }

        public static List<PropertyInfo> GetPropertiesToIgnoreOnDefault<T>()
        {
            var type = typeof(T);
            if (PropertiesToIgnoreOnDefaultByType.TryGetValue(type, out var cachedPropertyInfo))
            {
                return cachedPropertyInfo;
            }

            var propertiesToIgnoreOnDefault = type
                .GetTypeProperties()
                .Where(pi => pi.HasAttribute<DeltaObjectIgnoreOnDefaultAttribute>())
                .ToList();

            PropertiesToIgnoreOnDefaultByType.AddOrUpdate(type,
                propertiesToIgnoreOnDefault, (_, pi) => pi);

            return propertiesToIgnoreOnDefault;
        }
    }
}
