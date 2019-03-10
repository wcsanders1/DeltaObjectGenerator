using DeltaObjectGenerator.Geneators;
using DeltaObjectGeneratorTests.TestModels;
using Xunit;

namespace DeltaObjectGeneratorTests.Unit
{
    public class DeltaObjectFromObjectGeneratorUnitTests
    {
        [Trait("Category", "Unit")]
        public class GetDeltaObject
        {
            [Fact]
            public void ReturnsObject_WithProperValues()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "original name"
                };

                var newCustomer = new TestCustomer
                {
                    FirstName = "new name"
                };

                var deltaObject = DeltaObjectFromObjectGenerator.GetDeltaObject(originalCustomer, newCustomer);

                Assert.Single(deltaObject);
                Assert.Equal(newCustomer.FirstName, deltaObject[nameof(TestCustomer.FirstName)]);
            }

            [Fact]
            public void ReturnsObject_WithoutValueToIgnoreOnDefault()
            {
                var originalCustomer = new TestCustomerWithIgnoreOnDefaultAttribute
                {
                    FirstName = "original first name",
                    LastName = "original last name"
                };

                var newCustomer = new TestCustomerWithIgnoreOnDefaultAttribute();

                var deltaObject = DeltaObjectFromObjectGenerator.GetDeltaObject(originalCustomer, newCustomer);

                Assert.Single(deltaObject);
                Assert.Null(deltaObject[nameof(TestCustomer.FirstName)]);
            }

            [Fact]
            public void ReturnsObject_WithValueOfNonDefaultNewValue()
            {
                var originalCustomer = new TestCustomerWithIgnoreOnDefaultAttribute
                {
                    FirstName = "original first name",
                    LastName = "original last name"
                };

                var newCustomer = new TestCustomerWithIgnoreOnDefaultAttribute
                {
                    LastName = "new last name"
                };

                var deltaObject = DeltaObjectFromObjectGenerator.GetDeltaObject(originalCustomer, newCustomer);

                Assert.Equal(2, deltaObject.Count);
                Assert.Null(deltaObject[nameof(TestCustomerWithIgnoreOnDefaultAttribute.FirstName)]);
                Assert.Equal(newCustomer.LastName, deltaObject[nameof(TestCustomerWithIgnoreOnDefaultAttribute.LastName)]);
            }

            [Fact]
            public void ReturnsObject_WithoutValueToIgnore()
            {
                var originalCustomer = new TestCustomerWithIgnoreDeltaAttribute
                {
                    FirstName = "original first name",
                    LastName = "original last name"
                };

                var newCustomer = new TestCustomerWithIgnoreDeltaAttribute
                {
                    FirstName = "new first name",
                    LastName = "new last name"
                };

                var deltaObject = DeltaObjectFromObjectGenerator.GetDeltaObject(originalCustomer, newCustomer);

                Assert.Single(deltaObject);
                Assert.Equal(newCustomer.LastName, deltaObject[nameof(TestCustomerWithIgnoreOnDefaultAttribute.LastName)]);
            }
        }
    }
}
