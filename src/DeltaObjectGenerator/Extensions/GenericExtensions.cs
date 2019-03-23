using DeltaObjectGenerator.Generators;
using DeltaObjectGenerator.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Extensions
{
    public static class GenericExtensions
    {
        public static List<DeltaObject> GetDeltaObjects<T>(this T originalObject, T newObject)
        {
            if (originalObject == null)
            {
                throw new ArgumentNullException(nameof(originalObject));
            }

            if (newObject == null)
            {
                throw new ArgumentNullException(nameof(newObject));
            }

            return DeltaObjectFromObjectGenerator.GetDeltaObjects(originalObject, newObject);
        }

        public static List<DeltaObject> GetDeltaObjects<T>(this T originalObject, JObject jObject)
        {
            if (originalObject == null)
            {
                throw new ArgumentNullException(nameof(originalObject));
            }

            if (jObject == null)
            {
                throw new ArgumentNullException(nameof(jObject));
            }

            return DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalObject, jObject);
        }
    }
}