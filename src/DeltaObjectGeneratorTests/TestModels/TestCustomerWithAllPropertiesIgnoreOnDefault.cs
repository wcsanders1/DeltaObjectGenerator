using DeltaObjectGenerator.Attributes;
using System;

namespace DeltaObjectGeneratorTests.TestModels
{
    public class TestCustomerWithAllPropertiesIgnoreOnDefault
    {
        [DeltaObjectIgnoreOnDefault]
        public string FirstName { get; set; }

        [DeltaObjectIgnoreOnDefault]
        public string LastName { get; set; }

        [DeltaObjectIgnoreOnDefault]
        public int Age { get; set; }

        [DeltaObjectIgnoreOnDefault]
        public TestEnum SomeStuff { get; set; }

        [DeltaObjectIgnoreOnDefault]
        public TestFlagEnum SomeFlagStuff { get; set; }

        [DeltaObjectIgnoreOnDefault]
        public DateTime DateOfBirth { get; set; }

        [DeltaObjectIgnoreOnDefault]
        public DateTime? StartDate { get; set; }
    }
}
