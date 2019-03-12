using System.Reflection;

namespace DeltaObjectGenerator.Models
{
    internal class DeltaProperty
    {
        public PropertyInfo PropertyInfo { get; set; }
        public string Alias { get; set; }
    }
}
