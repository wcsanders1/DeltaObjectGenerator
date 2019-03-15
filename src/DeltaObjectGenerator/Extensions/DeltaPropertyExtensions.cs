using DeltaObjectGenerator.Models;
using System;

namespace DeltaObjectGenerator.Extensions
{
    internal static class DeltaPropertyExtensions
    {
        public static bool HasDelta(this DeltaProperty deltaProperty, object originalValue, object newValue)
        {
            var comparableOriginalValue = originalValue as IComparable;
            var comparableNewValue = newValue as IComparable;

            if (comparableOriginalValue == null && comparableNewValue == null)
            {
                return false;
            }

            if (comparableOriginalValue == null && comparableNewValue != null)
            {
                return true;
            }

            if (comparableOriginalValue != null && comparableNewValue == null)
            {
                return true;
            }

            return comparableOriginalValue.CompareTo(comparableNewValue) != 0;
        }
    }
}
