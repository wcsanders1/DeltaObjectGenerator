using DeltaObjectGenerator.Attributes;
using System;

namespace DeltaObjectGeneratorTests.TestModels
{
    public class TestCustomerWithAlias
    {
        public string FirstName { get; set; }

        [DeltaObjectAlias("first_name")]
        public string LastName { get; set; }

        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TestAccount Account { get; set; }
    }
}
