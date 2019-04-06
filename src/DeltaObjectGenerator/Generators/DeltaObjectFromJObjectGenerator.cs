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
        public static DeltaGroup GetDeltaObjects<T>(T originalObject, JObject jObject)
        {
            var deltaProperties = TypeCache.GetDeltaPropertyInfo<T>();
            var propertiesToIgnoreOnDefault = TypeCache.GetPropertiesToIgnoreOnDefault<T>();
            var ignorePropertiesOnDefault = TypeCache.IgnorePropertiesOnDefault<T>();

            return deltaProperties.Select(deltaProperty =>
                GetDeltaObject(deltaProperty, originalObject, jObject,
                    propertiesToIgnoreOnDefault, ignorePropertiesOnDefault))
                .Aggregate(new DeltaGroup(), (deltaGroup, deltaObject) =>
                {
                    if (deltaObject == null)
                    {
                        return deltaGroup;
                    }

                    switch (deltaObject.ValueConversionStatus)
                    {
                        case ValueConversionStatus.Success:
                            deltaGroup.DeltaObjects.Add(deltaObject);
                            return deltaGroup;
                        case ValueConversionStatus.Fail:
                            deltaGroup.DeltaObjectsValueConversionFail.Add(deltaObject);
                            return deltaGroup;
                        default:
                            return deltaGroup;
                    }
                });
        }

        private static DeltaObject GetDeltaObject<T>(DeltaProperty deltaProperty, T originalObject,
            JObject jObject, List<PropertyInfo> propertiesToIgnoreOnDefault, bool ignorePropertiesOnDefault)
        {
            var propertyInfo = deltaProperty.PropertyInfo;

            if (!jObject.TryGetValue(propertyInfo.Name,
#if NETSTANDARD1_6
                StringComparison.CurrentCultureIgnoreCase,
#else
                StringComparison.InvariantCultureIgnoreCase,
#endif
                out var newValue))
            {
                return null;
            }

            var originalValue = deltaProperty.PropertyInfo.GetValue(originalObject);
            var typeConverter = TypeCache.GetTypeConverter(propertyInfo.PropertyType);
            var stringifiedNewValue = newValue.Type != JTokenType.Null ? 
                newValue.ToString() : 
                null;
            
            object convertedNewValue;
            try
            {
                convertedNewValue = stringifiedNewValue != null ?
                    typeConverter.ConvertFromString(stringifiedNewValue) :
                    null;
            }
            catch (Exception)
            {
                return new DeltaObject
                {
                    ValueConversionStatus = ValueConversionStatus.Fail,
                    PropertyName = propertyInfo.Name,
                    PropertyAlias = deltaProperty.Alias,
                    OriginalValue = originalValue,
                    NewValue = stringifiedNewValue,
                    StringifiedOriginalValue = originalValue?.ToString(),
                    StringifiedNewValue = stringifiedNewValue
                };
            }

            if (propertyInfo.IgnoreDeltaBecauseDefault(propertiesToIgnoreOnDefault,
                convertedNewValue, ignorePropertiesOnDefault))
            {
                return null;
            }

            if (deltaProperty.HasDelta(originalValue, convertedNewValue))
            {
                return new DeltaObject
                {
                    ValueConversionStatus = ValueConversionStatus.Success,
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
