using System;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsDeltaInclude(this Type type, List<Type> acceptedNonPrimitives)
        {
            return
                Nullable.GetUnderlyingType(type) != null ||
                type.IsPrimitive ||
                type.IsEnum ||
                acceptedNonPrimitives.Contains(type);
        }
    }
}
