using DeltaObjectGenerator.Caches;
using DeltaObjectGenerator.Extensions;
using DeltaObjectGenerator.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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

            return deltaProperties.Select(deltaProperty =>
                GetDeltaObject(deltaProperty, originalObject, jObject,
                    propertiesToIgnoreOnDefault, ignorePropertiesOnDefault))
            .Where(d => d != null)
            .ToList();
        }

        private static DeltaObject GetDeltaObject<T>(DeltaProperty deltaProperty, T originalObject,
            JObject jObject, List<PropertyInfo> propertiesToIgnoreOnDefault, bool ignorePropertiesOnDefault)
        {
            var propertyInfo = deltaProperty.PropertyInfo;

            if (!jObject.TryGetValue(propertyInfo.Name, out var newValue))
            {
                return null;
            }

            var originalValue = deltaProperty.PropertyInfo.GetValue(originalObject);

            var typeConverter = TypeCache.GetTypeConverter<T>();

            //if (!typeConverter.IsValid(newValue))
            //{
            //    return new DeltaObject
            //    {
            //        ConversionStatus = ConversionStatus.Invalid,
            //        PropertyName = propertyInfo.Name,
            //        PropertyAlias = deltaProperty.Alias,
            //        OriginalValue = originalValue,
            //        NewValue = newValue,
            //        StringifiedOriginalValue = originalValue?.ToString(),
            //        StringifiedNewValue = newValue?.ToString()
            //    };
            //}

            var convertedNewValue = typeConverter.ConvertTo(newValue, propertyInfo.PropertyType);
            if (propertyInfo.IgnoreDeltaBecauseDefault(propertiesToIgnoreOnDefault,
                convertedNewValue, ignorePropertiesOnDefault))
            {
                return null;
            }

            if (deltaProperty.HasDelta(originalValue, convertedNewValue))
            {
                return new DeltaObject
                {
                    ConversionStatus = ConversionStatus.Valid,
                    PropertyName = propertyInfo.Name,
                    PropertyAlias = deltaProperty.Alias,
                    OriginalValue = originalValue,
                    NewValue = convertedNewValue,
                    StringifiedOriginalValue = originalValue?.ToString(),
                    StringifiedNewValue = convertedNewValue?.ToString()
                };
            }

            return null;
        }
    }
}
