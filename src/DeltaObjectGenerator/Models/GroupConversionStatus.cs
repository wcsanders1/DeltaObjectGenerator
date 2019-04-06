using Newtonsoft.Json.Linq;

namespace DeltaObjectGenerator.Models
{
    /// <summary>
    /// Enum indicating status of conversion of <see cref="JObject"/> values into their associated 
    /// properties.
    /// </summary>
    public enum GroupConversionStatus
    {
        /// <summary>
        /// Indicates all deltas are convertible to the type of their associated property. 
        /// </summary>
        CompleteSuccess,

        /// <summary>
        /// Indicates some deltas are and some are not convertible to the type of their associated property.
        /// </summary>
        Partial,

        /// <summary>
        /// Indicates no deltas are convertible to the type of their associated property.
        /// </summary>
        CompleteFail
    }
}
