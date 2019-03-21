using DeltaObjectGenerator.Caches;
using DeltaObjectGenerator.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

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

            var deltaProperties = TypeCache.GetDeltaPropertyInfo<T>();
            var propertiesToIgnoreOnDefault = TypeCache.GetPropertiesToIgnoreOnDefault<T>();
            var ignorePropertiesOnDefault = TypeCache.IgnorePropertiesOnDefault<T>();

            return null;
        }

        private static DeltaObject GetDeltaObject<T>(DeltaProperty deltaProperty, T originalObject,
            JObject jObject, List<PropertyInfo> propertiesToIgnoreOnDefault, bool ignorePropertiesOnDefault)
        {
            var propertyInfo = deltaProperty.PropertyInfo;

            if (!jObject.TryGetValue(propertyInfo.Name, out var newVal))
            {
                return null;
            }

            var typeConverter = TypeCache.GetTypeConverter<T>();

            if (!typeConverter.CanConvertFrom(typeof(T)))
            {
                // Maybe throw a custom exception here.
                // Or, put a conversion status on the model.
            }
            
            return null;
        }
    }
}
