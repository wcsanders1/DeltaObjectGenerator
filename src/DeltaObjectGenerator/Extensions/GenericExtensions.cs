using DeltaObjectGenerator.Generators;
using DeltaObjectGenerator.Models;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Extensions
{
    public static class GenericExtensions
    {
        public static List<DeltaObject> GetDeltaObjects<T>(this T originalObject, T newObject)
        {
            return DeltaObjectFromObjectGenerator.GetDeltaObjects(originalObject, newObject);
        }
    }
}