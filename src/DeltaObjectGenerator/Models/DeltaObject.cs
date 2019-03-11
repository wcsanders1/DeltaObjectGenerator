namespace DeltaObjectGenerator.Models
{
    public class DeltaObject
    {
        public string PropertyName { get; set; }
        public string PropertyAlias { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
    }
}
