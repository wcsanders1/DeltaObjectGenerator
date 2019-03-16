using System;

namespace DeltaObjectGeneratorTests.TestModels
{
    public class TestCustomerWithFlagEnum
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public TestFlagEnum SomeFlagEnum { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TestAccount Account { get; set; }
    }
}
