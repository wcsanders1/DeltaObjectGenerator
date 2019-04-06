using Newtonsoft.Json.Linq;

namespace DeltaObjectGenerator.Models
{
    /// <summary>
    /// Enum indicating status of conversion of <see cref="JObject"/> values into their associated 
    /// properties.
    /// </summary>
    public enum GroupValueConversionStatus
    {
        /// <summary>
        /// Indicates all delta values are convertible to the type of their associated property. 
        /// This is also the value when there are no deltas.
        /// </summary>
        NoneFailed = 0,

        /// <summary>
        /// Indicates some delta values are and some are not convertible to the type of 
        /// their associated property.
        /// </summary>
        SomeFailed = 1,

        /// <summary>
        /// Indicates no delta values are convertible to the type of their associated property.
        /// </summary>
        AllFailed = 2
    }
}
