using System.Collections.Generic;

namespace DeltaObjectGenerator.Models
{
    /// <summary>
    /// A class containing a <see cref="List{DeltaObject}"/> for successfully converted properties 
    /// and a <see cref="List{DeltaObject}"/> for unsuccessfully converted properties.
    /// </summary>
    public class DeltaGroup
    {
        /// <summary>
        /// Indicates whether all, some, or none of the properties were successfully converted 
        /// into their associated types.
        /// </summary>
        public GroupConversionStatus ConversionStatus { get; internal set; }

        /// <summary>
        /// Contains <see cref="DeltaObject"/>s with <see cref="ConversionStatus.Valid"/>.
        /// </summary>
        public List<DeltaObject> DeltaObjectsConversionSuccess { get; internal set; }

        /// <summary>
        /// Contains <see cref="DeltaObject"/>s with <see cref="ConversionStatus.Invalid"/>.
        /// </summary>
        public List<DeltaObject> DeltaObjectsConversionFail { get; internal set; }

        internal DeltaGroup()
        {}
    }
}
