namespace DeltaObjectGenerator.Models
{
    /// <summary>
    /// Provides the status of the conversion attempt of a value from one type to another.
    /// </summary>
    public enum ValueConversionStatus
    {
        /// <summary>
        /// Indicates that the value can be converted into a specific type.
        /// </summary>
        Success = 0,

        /// <summary>
        /// Indicates that the value cannot be converted into a specific type.
        /// </summary>
        Fail = 1
    }
}
