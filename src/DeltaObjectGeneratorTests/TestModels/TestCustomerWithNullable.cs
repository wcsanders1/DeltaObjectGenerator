using System;

namespace DeltaObjectGeneratorTests.TestModels
{
    public class TestCustomerWithNullable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public decimal? Salary { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TestAccount Account { get; set; }
    }
}
