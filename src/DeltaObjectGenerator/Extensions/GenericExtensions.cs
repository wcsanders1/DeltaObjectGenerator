using DeltaObjectGenerator.Geneators;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Extensions
{
    public static class GenericExtensions
    {
        public static Dictionary<string, string> GetDeltaObject<T>(this T thisObject, T otherObject)
        {
            return DeltaObjectFromObjectGenerator.GetDeltaObject(thisObject, otherObject);
        }
    }
}