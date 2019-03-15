namespace DeltaObjectGenerator.Models
{
    public class DeltaObject
    {
        public string PropertyName { get; set; }
        public string PropertyAlias { get; set; }
        public object OriginalValue { get; set; }
        public object NewValue { get; set; }
        public string StringifiedOriginalValue { get; set; }
        public string StringifiedNewValue { get; set; }
    }
}
