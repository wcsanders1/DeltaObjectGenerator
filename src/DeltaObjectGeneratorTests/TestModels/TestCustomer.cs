using System;

namespace DeltaObjectGeneratorTests.TestModels
{
    internal class TestCustomer : TestAbstractCustomer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TestAccount Account { get; set; }

        private string Secret { get; set; }

        public void SetSecret(string secret)
        {
            Secret = secret;
        }
    }
}
