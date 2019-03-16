using System;
using System.Collections.Generic;
using System.Text;

namespace DeltaObjectGeneratorTests.TestModels
{
    public class TestCustomerWithArray
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int[] FavoriteNumbers { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TestAccount Account { get; set; }
    }
}
