using DeltaObjectGenerator.Attributes;
using System;

namespace DeltaObjectGeneratorTests.TestModels
{
    class TestCustomerWithNullableIgnoreOnDefault
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        [DeltaObjectIgnoreOnDefault]
        public decimal? Salary { get; set; }

        public DateTime DateOfBirth { get; set; }
        public TestAccount Account { get; set; }
    }
}
