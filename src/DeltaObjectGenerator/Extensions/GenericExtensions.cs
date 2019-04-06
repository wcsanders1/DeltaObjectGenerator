using DeltaObjectGenerator.Generators;
using DeltaObjectGenerator.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Extensions
{
    /// <summary>
    /// A class offering extension methods to generate a <see cref="List{DeltaObject}"/>.
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// Returns a <see cref="List{DeltaObject}"/>, which will be empty if there are no deltas.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="originalObject">A type of <typeparamref name="T"/>.</param>
        /// <param name="newObject">A type of <typeparamref name="T"/>.</param>
        /// <returns><see cref="List{DeltaObject}"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when either arguments are <c>null</c>.</exception>
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

        /// <summary>
        /// Returns a <see cref="DeltaGroup"/>, with empty collections of <see cref="DeltaObject"/> 
        /// if there are no deltas.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="originalObject">A type of <typeparamref name="T"/>.</param>
        /// <param name="jObject">A type of <see cref="JObject"/>.</param>
        /// <returns><see cref="DeltaGroup"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when either arguments are <c>null</c>.</exception>
        public static DeltaGroup GetDeltaObjects<T>(this T originalObject, JObject jObject)
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