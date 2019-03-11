using DeltaObjectGenerator.Geneators;
using DeltaObjectGenerator.Models;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Extensions
{
    public static class GenericExtensions
    {
        public static List<DeltaObject> GetDeltaObject<T>(this T thisObject, T otherObject)
        {
            return DeltaObjectFromObjectGenerator.GetDeltaObject(thisObject, otherObject);
        }
    }
}