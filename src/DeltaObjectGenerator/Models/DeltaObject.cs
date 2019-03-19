namespace DeltaObjectGenerator.Models
{
    public class DeltaObject
    {
        public string PropertyName { get; internal set; }
        public string PropertyAlias { get; internal set; }
        public object OriginalValue { get; internal set; }
        public object NewValue { get; internal set; }
        public string StringifiedOriginalValue { get; internal set; }
        public string StringifiedNewValue { get; internal set; }

        internal DeltaObject()
        {}
    }
}
