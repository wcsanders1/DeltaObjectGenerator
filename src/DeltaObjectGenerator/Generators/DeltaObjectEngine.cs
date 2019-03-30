using DeltaObjectGenerator.Extensions;
using DeltaObjectGenerator.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DeltaObjectGenerator.Generators
{
    /// <summary>
    /// Interface offering methods returning <see cref="List{DeltaObject}"./>
    /// </summary>
    public interface IDeltaObjectEngine
    {
        /// <summary>
        /// Returns a <see cref="List{DeltaObject}"/>, which will be empty if there are no deltas.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="originalObject">A type of <typeparamref name="T"/>.</param>
        /// <param name="newObject">A type of <typeparamref name="T"/>.</param>
        /// <returns><see cref="List{DeltaObject}"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when either arguments are <c>null</c>.</exception>
        List<DeltaObject> GetDeltaObjects<T>(T originalObject, T newObject);

        /// <summary>
        /// Returns a <see cref="List{DeltaObject}"/>, which will be empty if there are no deltas.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="originalObject">A type of <typeparamref name="T"/>.</param>
        /// <param name="jObject">A type of <see cref="JObject"/>.</param>
        /// <returns><see cref="List{DeltaObject}"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when either arguments are <c>null</c>.</exception>
        List<DeltaObject> GetDeltaObjects<T>(T originalObject, JObject jObject);
    }

    /// <summary>
    /// A class offering functionality to generate a <see cref="List{DeltaObject}"/>
    /// </summary>
    public class DeltaObjectEngine : IDeltaObjectEngine
    {
        /// <summary>
        /// Returns a <see cref="List{DeltaObject}"/>, which will be empty if there are no deltas.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="originalObject">A type of <typeparamref name="T"/>.</param>
        /// <param name="newObject">A type of <typeparamref name="T"/>.</param>
        /// <returns><see cref="List{DeltaObject}"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when either arguments are <c>null</c>.</exception>
        public List<DeltaObject> GetDeltaObjects<T>(T originalObject, T newObject)
        {
            return originalObject.GetDeltaObjects(newObject);
        }

        /// <summary>
        /// Returns a <see cref="List{DeltaObject}"/>, which will be empty if there are no deltas.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="originalObject">A type of <typeparamref name="T"/>.</param>
        /// <param name="jObject">A type of <see cref="JObject"/>.</param>
        /// <returns><see cref="List{DeltaObject}"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when either arguments are <c>null</c>.</exception>
        public List<DeltaObject> GetDeltaObjects<T>(T originalObject, JObject jObject)
        {
            return originalObject.GetDeltaObjects(jObject);
        }
    }
}
