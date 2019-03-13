using DeltaObjectGenerator.Geneators;
using DeltaObjectGeneratorTests.TestModels;
using System.Linq;
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

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObject(originalCustomer, newCustomer);

                Assert.Single(deltaObjects);
                Assert.Equal(nameof(TestCustomer.FirstName), deltaObjects.First().PropertyName);
                Assert.Equal(nameof(TestCustomer.FirstName), deltaObjects.First().PropertyAlias);
                Assert.Equal(originalCustomer.FirstName, deltaObjects.First().OriginalValue);
                Assert.Equal(newCustomer.FirstName, deltaObjects.First().NewValue);
            }

            [Fact]
            public void ReturnsObject_WithoutValueToIgnoreOnDefault()
            {
                var originalCustomer = new TestCustomerWithDeltaObjectIgnoreOnDefaultAttribute
                {
                    FirstName = "original first name",
                    LastName = "original last name"
                };

                var newCustomer = new TestCustomerWithDeltaObjectIgnoreOnDefaultAttribute();

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObject(originalCustomer, newCustomer);

                Assert.Single(deltaObjects);
                Assert.Equal(nameof(TestCustomerWithDeltaObjectIgnoreOnDefaultAttribute.FirstName),
                    deltaObjects.First().PropertyName);
                Assert.Equal(nameof(TestCustomerWithDeltaObjectIgnoreOnDefaultAttribute.FirstName),
                    deltaObjects.First().PropertyAlias);
                Assert.Equal(originalCustomer.FirstName, deltaObjects.First().OriginalValue);
                Assert.Null(deltaObjects.First().NewValue);
            }

            [Fact]
            public void ReturnsObject_WithValueOfNonDefaultNewValue()
            {
                var originalCustomer = new TestCustomerWithDeltaObjectIgnoreOnDefaultAttribute
                {
                    FirstName = "original first name",
                    LastName = "original last name"
                };

                var newCustomer = new TestCustomerWithDeltaObjectIgnoreOnDefaultAttribute
                {
                    LastName = "new last name"
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObject(originalCustomer, newCustomer);

                Assert.Equal(2, deltaObjects.Count);
                Assert.Null(deltaObjects.First(o => o.PropertyName ==
                    nameof(TestCustomerWithDeltaObjectIgnoreOnDefaultAttribute.FirstName)).NewValue);
                Assert.Equal(originalCustomer.FirstName, deltaObjects.First(o => o.PropertyName ==
                    nameof(TestCustomerWithDeltaObjectIgnoreOnDefaultAttribute.FirstName)).OriginalValue);
                Assert.Equal(originalCustomer.LastName, deltaObjects.First(o => o.PropertyName ==
                    nameof(TestCustomerWithDeltaObjectIgnoreOnDefaultAttribute.LastName)).OriginalValue);
                Assert.Equal(newCustomer.LastName, deltaObjects.First(o => o.PropertyName ==
                    nameof(TestCustomerWithDeltaObjectIgnoreOnDefaultAttribute.LastName)).NewValue);
            }

            [Fact]
            public void ReturnsObject_WithoutValueToIgnore()
            {
                var originalCustomer = new TestCustomerWithDeltaObjectIgnoreAttribute
                {
                    FirstName = "original first name",
                    LastName = "original last name"
                };

                var newCustomer = new TestCustomerWithDeltaObjectIgnoreAttribute
                {
                    FirstName = "new first name",
                    LastName = "new last name"
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObject(originalCustomer, newCustomer);

                Assert.Single(deltaObjects);
                Assert.Equal(originalCustomer.LastName, deltaObjects.First(o => o.PropertyName ==
                    nameof(TestCustomerWithDeltaObjectIgnoreAttribute.LastName)).OriginalValue);
                Assert.Equal(newCustomer.LastName, deltaObjects.First(o => o.PropertyName ==
                    nameof(TestCustomerWithDeltaObjectIgnoreAttribute.LastName)).NewValue);
            }

            [Fact]
            public void ReturnsObject_WithNullableWhenValueChanged()
            {
                var originalCustomer = new TestCustomerWithNullable
                {
                    Age = 30,
                    Salary = 100000.45M
                };

                var newCustomer = new TestCustomerWithNullable
                {
                    Age = 40,
                    Salary = 100000.46M
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObject(originalCustomer, newCustomer);

                Assert.Equal(2, deltaObjects.Count);
                Assert.Equal(newCustomer.Salary.ToString(), deltaObjects.First(o => o.PropertyName ==
                    nameof(TestCustomerWithNullable.Salary)).NewValue);
                Assert.Equal(newCustomer.Age.ToString(), deltaObjects.First(o => o.PropertyName ==
                    nameof(TestCustomerWithNullable.Age)).NewValue);
            }

            [Fact]
            public void ReturnsObject_WithNullableNullWhenValueChangedToNull()
            {
                var originalCustomer = new TestCustomerWithNullable
                {
                    Age = 30,
                    Salary = 100000.45M
                };

                var newCustomer = new TestCustomerWithNullable
                {
                    Age = 40,
                    Salary = null
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObject(originalCustomer, newCustomer);

                Assert.Equal(2, deltaObjects.Count);
                Assert.Null(deltaObjects.First(o => o.PropertyName ==
                    nameof(TestCustomerWithNullable.Salary)).NewValue);
                Assert.Equal(newCustomer.Age.ToString(), deltaObjects.First(o => o.PropertyName ==
                    nameof(TestCustomerWithNullable.Age)).NewValue);
            }

            [Fact]
            public void ReturnsObjectWithoutNullable_WhenNullableIgnoredOnDefault()
            {
                var originalCustomer = new TestCustomerWithNullableIgnoreOnDefault
                {
                    Age = 30,
                    Salary = 100000M
                };

                var newCustomer = new TestCustomerWithNullableIgnoreOnDefault
                {
                    Age = 40,
                    Salary = null
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObject(originalCustomer, newCustomer);

                Assert.Single(deltaObjects);
                Assert.Equal(newCustomer.Age.ToString(), deltaObjects.First(o => o.PropertyName ==
                    nameof(TestCustomerWithNullable.Age)).NewValue);
            }

            [Fact]
            public void ReturnsObjectWithAlias_WhenPropertyAliased()
            {
                var originalCustomer = new TestCustomerWithAlias
                {
                    FirstName = "original first name",
                    LastName = "original last name"
                };

                var newCustomer = new TestCustomerWithAlias
                {
                    FirstName = "new first name",
                    LastName = "new last name"
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObject(originalCustomer, newCustomer);

                Assert.Equal(2, deltaObjects.Count);
                Assert.Equal("last_name", deltaObjects.First(o => o.PropertyName == 
                    (nameof(TestCustomerWithAlias.LastName))).PropertyAlias);
                Assert.Equal(nameof(TestCustomerWithAlias.FirstName), deltaObjects.First(o => o.PropertyName ==
                    (nameof(TestCustomerWithAlias.FirstName))).PropertyAlias);
            }
        }
    }
}
