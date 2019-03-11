using DeltaObjectGenerator.Attributes;
using System;

namespace DeltaObjectGeneratorTests.TestModels
{
    public class TestCustomerWithDeltaObjectIgnoreOnDefaultAttribute
    {
        public string FirstName { get; set; }

        [DeltaObjectIgnoreOnDefault]
        public string LastName { get; set; }
        
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TestAccount Account { get; set; }
    }
}
