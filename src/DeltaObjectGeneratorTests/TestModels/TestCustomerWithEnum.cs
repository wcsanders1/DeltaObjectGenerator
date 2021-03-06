﻿using System;

namespace DeltaObjectGeneratorTests.TestModels
{
    class TestCustomerWithEnum
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public TestEnum SomeEnum { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TestAccount Account { get; set; }
    }
}
