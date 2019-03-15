using System;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsDeltaInclude(this Type type, List<Type> acceptedNonPrimitives)
        {
            return
                Nullable.GetUnderlyingType(type) != null ||
                type.IsPrimitive ||
                type.IsEnum ||
                acceptedNonPrimitives.Contains(type);
        }

        // TODO: Maybe this should just return the default value rather than IComparable
        public static IComparable GetComparableDefaultValue(this Type type)
        {
            if (!type.IsValueType)
            {
                return null;
            }

            return Activator.CreateInstance(type) as IComparable;
        }
    }
}
