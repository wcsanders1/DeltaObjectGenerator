using DeltaObjectGenerator.Attributes;
using System;

namespace DeltaObjectGeneratorTests.TestModels
{
    public class TestCustomerWithIgnoreDeltaAttribute
    {
        [IgnoreDelta]
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TestAccount Account { get; set; }
    }
}
