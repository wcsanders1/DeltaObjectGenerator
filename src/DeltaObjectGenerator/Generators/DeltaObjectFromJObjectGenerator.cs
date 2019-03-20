using DeltaObjectGenerator.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Generators
{
    internal class DeltaObjectFromJObjectGenerator
    {
        public static List<DeltaObject> GetDeltaObjects<T>(T originalObject, JObject jObject)
        {
            if (originalObject == null)
            {
                throw new ArgumentNullException(nameof(originalObject));
            }

            if (jObject == null)
            {
                throw new ArgumentNullException(nameof(jObject));
            }

            return null;
        }
    }
}
