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
        /// Indicates all deltas are convertible to the type of their associated property. 
        /// </summary>
        Success = 0,

        /// <summary>
        /// Indicates some deltas are and some are not convertible to the type of their associated property.
        /// </summary>
        Partial = 1,

        /// <summary>
        /// Indicates no deltas are convertible to the type of their associated property.
        /// </summary>
        Fail = 2
    }
}
