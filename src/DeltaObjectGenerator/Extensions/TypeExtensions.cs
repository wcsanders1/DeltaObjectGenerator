using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeltaObjectGenerator.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsDeltaInclude(this Type type, List<Type> acceptedNonPrimitives)
        {
#if NETSTANDARD1_6
            var typeInfo = type.GetTypeInfo();

            return 
                Nullable.GetUnderlyingType(type) != null || 
                typeInfo.IsPrimitive ||
                typeInfo.IsEnum ||
                acceptedNonPrimitives.Contains(type);
#else
            return
                Nullable.GetUnderlyingType(type) != null ||
                type.IsPrimitive ||
                type.IsEnum ||
                acceptedNonPrimitives.Contains(type);
#endif
        }

        public static IComparable GetComparableDefaultValue(this Type type)
        {
#if NETSTANDARD1_6
            if (!type.GetTypeInfo().IsValueType)
#else
            if (!type.IsValueType)
#endif
            {
                return null;
            }

            return Activator.CreateInstance(type) as IComparable;
        }

        public static List<PropertyInfo> GetTypeProperties(this Type type)
        {
#if NETSTANDARD1_6
            return type.GetTypeInfo().DeclaredProperties.ToList();
#else
            return type.GetProperties().ToList();
#endif
        }

        public static bool HasAttribute<T>(this Type type)
            where T : Attribute
        {
#if NETSTANDARD1_6
            return type.GetTypeInfo().GetCustomAttribute(typeof(T)) != null;
#else
            return type.GetCustomAttribute<T>() != null;
#endif
        }
    }
}
