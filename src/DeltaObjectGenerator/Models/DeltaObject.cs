using DeltaObjectGenerator.Attributes;

namespace DeltaObjectGenerator.Models
{
    /// <summary>
    /// A class containing information about a property's deltas.
    /// </summary>
    public class DeltaObject
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        public string PropertyName { get; internal set; }

        /// <summary>
        /// The property's alias as set by <see cref="DeltaObjectAliasAttribute"/>,
        /// or the value of the property name if <see cref="DeltaObjectAliasAttribute"/> 
        /// is not applied.
        /// </summary>
        public string PropertyAlias { get; internal set; }

        /// <summary>
        /// The original value of the property.
        /// </summary>
        public object OriginalValue { get; internal set; }

        /// <summary>
        /// The new value of the property.
        /// </summary>
        public object NewValue { get; internal set; }

        /// <summary>
        /// The original value of the property as a <see cref="string"/>.
        /// </summary>
        public string StringifiedOriginalValue { get; internal set; }

        /// <summary>
        /// The new value of the property as a <see cref="string"/>.
        /// </summary>
        public string StringifiedNewValue { get; internal set; }

        /// <summary>
        /// Has a value of <see cref="ValueConversionStatus.Success"/> when the value of <see cref="NewValue"/>
        /// can be converted into the type of the property on the model, otherwise the value will be
        /// <see cref="ValueConversionStatus.Fail"/>.
        /// </summary>
        public ValueConversionStatus ValueConversionStatus { get; internal set; }

        internal DeltaObject()
        {}
    }
}
