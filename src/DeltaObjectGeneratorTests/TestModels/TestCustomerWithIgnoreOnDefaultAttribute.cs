using DeltaObjectGenerator.Attributes;
using System;

namespace DeltaObjectGeneratorTests.TestModels
{
    public class TestCustomerWithIgnoreOnDefaultAttribute
    {
        public string FirstName { get; set; }

        [IgnoreDeltaWhenDefault]
        public string LastName { get; set; }
        
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TestAccount Account { get; set; }
    }
}
