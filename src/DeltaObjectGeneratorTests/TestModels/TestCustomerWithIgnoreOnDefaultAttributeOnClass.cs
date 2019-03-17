using DeltaObjectGenerator.Attributes;
using System;

namespace DeltaObjectGeneratorTests.TestModels
{
    [DeltaObjectIgnoreOnDefault]
    public class TestCustomerWithIgnoreOnDefaultAttributeOnClass
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public TestEnum SomeStuff { get; set; }
        public TestFlagEnum SomeFlagStuff { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
