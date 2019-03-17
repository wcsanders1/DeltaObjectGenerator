using DeltaObjectGenerator.Geneators;
using DeltaObjectGenerator.Models;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Extensions
{
    public static class GenericExtensions
    {
        public static List<DeltaObject> GetDeltaObjects<T>(this T thisObject, T otherObject)
        {
            return DeltaObjectFromObjectGenerator.GetDeltaObjects(thisObject, otherObject);
        }
    }
}