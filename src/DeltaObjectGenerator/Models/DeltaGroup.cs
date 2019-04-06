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
        public GroupValueConversionStatus ValueConversionStatus
        {
            get
            {
                if (DeltaObjectsValueConversionFail.Count == 0)
                {
                    return GroupValueConversionStatus.Success;
                }

                if (DeltaObjectsValueConversionFail.Count > 0 &&
                    DeltaObjects.Count > 0)
                {
                    return GroupValueConversionStatus.Partial;
                }

                return GroupValueConversionStatus.Success;
            }

            internal set
            { }
        }

        /// <summary>
        /// Contains <see cref="DeltaObject"/>s with <see cref="ValueConversionStatus.Success"/>.
        /// </summary>
        public List<DeltaObject> DeltaObjects { get; internal set; }

        /// <summary>
        /// Contains <see cref="DeltaObject"/>s with <see cref="ValueConversionStatus.Fail"/>.
        /// </summary>
        public List<DeltaObject> DeltaObjectsValueConversionFail { get; internal set; }

        internal DeltaGroup()
        {
            DeltaObjects = new List<DeltaObject>();
            DeltaObjectsValueConversionFail = new List<DeltaObject>();
        }
    }
}
