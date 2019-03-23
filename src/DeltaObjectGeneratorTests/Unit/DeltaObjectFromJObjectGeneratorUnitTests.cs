using DeltaObjectGenerator.Generators;
using DeltaObjectGenerator.Models;
using DeltaObjectGeneratorTests.TestModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Xunit;

namespace DeltaObjectGeneratorTests.Unit
{
    public class DeltaObjectFromJObjectGeneratorUnitTests
    {
        [Trait("Category", "Unit")]
        public class GetDeltaObjects
        {
            [Fact]
            public void ReturnsSingleStringDeltaObject_WhenSingleStringDeltaExists()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "originalFirstName",
                    LastName = "originalLastName"
                };

                var newCustomer = new
                {
                    FirstName = "newFirstName",
                    LastName = "originalLastName"
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaObjects = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaObjects);
                Assert.Single(deltaObjects);
                Assert.Equal(ConversionStatus.Valid, deltaObjects[0].ConversionStatus);
                Assert.Equal(newCustomer.FirstName, deltaObjects[0].NewValue);
            }

            [Fact]
            public void ReturnsSingleIntDeltaObject_WhenSingleIntDeltaExists()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "originalFirstName",
                    Age = 20
                };

                var newCustomer = new
                {
                    FirstName = "originalFirstName",
                    Age = 25
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaObjects = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaObjects);
                Assert.Single(deltaObjects);
                Assert.Equal(ConversionStatus.Valid, deltaObjects[0].ConversionStatus);
                Assert.Equal(newCustomer.Age, deltaObjects[0].NewValue);
            }

            [Fact]
            public void ReturnsDeltaWithInvalidConversionStatus_WhenCannotConvertType()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "originalFirstName",
                    Age = 20
                };

                var newCustomer = new
                {
                    FirstName = "originalFirstName",
                    Age = "twenty-five"
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaObjects = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaObjects);
                Assert.Single(deltaObjects);
                Assert.Equal(ConversionStatus.Invalid, deltaObjects[0].ConversionStatus);
                Assert.Equal(newCustomer.Age, deltaObjects[0].NewValue);
            }

            [Fact]
            public void DetectsDateDelta_WhenDatesUnequal()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "originalFirstName",
                    Age = 20,
                    DateOfBirth = new DateTime(1940, 2, 3)
                };

                var newCustomer = new
                {
                    FirstName = "newFirstName",
                    Age = 20,
                    DateOfBirth = new DateTime(1940, 2, 4)
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaObjects = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaObjects);
                Assert.Equal(2, deltaObjects.Count);
                Assert.Equal(ConversionStatus.Valid, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.FirstName)).ConversionStatus);
                Assert.Equal(ConversionStatus.Valid, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.DateOfBirth)).ConversionStatus);
                Assert.Equal(newCustomer.FirstName, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.FirstName)).NewValue);
                Assert.Equal(newCustomer.DateOfBirth, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.DateOfBirth)).NewValue);
            }

            [Fact]
            public void DecectsDateDelta_WhenDateIsStringFormat()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "originalFirstName",
                    Age = 20,
                    DateOfBirth = new DateTime(1940, 2, 3)
                };

                var newCustomer = new
                {
                    FirstName = "newFirstName",
                    Age = 20,
                    DateOfBirth = "December 8, 1979"
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaObjects = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaObjects);
                Assert.Equal(2, deltaObjects.Count);
                Assert.Equal(ConversionStatus.Valid, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.FirstName)).ConversionStatus);
                Assert.Equal(ConversionStatus.Valid, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.DateOfBirth)).ConversionStatus);
                Assert.Equal(newCustomer.FirstName, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.FirstName)).NewValue);
                Assert.Equal(new DateTime(1979, 12, 8), deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.DateOfBirth)).NewValue);
            }

            [Fact]
            public void ReturnsDeltaWithInvalidConversionStatus_WhenCannotConvertDate()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "originalFirstName",
                    Age = 20,
                    DateOfBirth = new DateTime(1940, 2, 3)
                };

                var newCustomer = new
                {
                    FirstName = "newFirstName",
                    Age = 20,
                    DateOfBirth = "some long ago era"
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaObjects = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaObjects);
                Assert.Equal(2, deltaObjects.Count);
                Assert.Equal(ConversionStatus.Valid, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.FirstName)).ConversionStatus);
                Assert.Equal(ConversionStatus.Invalid, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.DateOfBirth)).ConversionStatus);
                Assert.Equal(newCustomer.FirstName, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.FirstName)).NewValue);
                Assert.Equal(newCustomer.DateOfBirth, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.DateOfBirth)).NewValue);
            }

            [Fact]
            public void DetectsDelta_WhenPropertyChangedToDefault()
            {
                var originalCustomer = new TestCustomer
                {
                    DateOfBirth = new DateTime(1949, 3, 3),
                    Age = 20
                };

                var newCustomer = new
                {
                    DateOfBirth = default(DateTime),
                    Age = default(int)
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaObjects = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaObjects);
                Assert.Equal(2, deltaObjects.Count);
                Assert.Equal(ConversionStatus.Valid, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.DateOfBirth)).ConversionStatus);
                Assert.Equal(newCustomer.DateOfBirth, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.DateOfBirth)).NewValue);
                Assert.Equal(ConversionStatus.Valid, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.Age)).ConversionStatus);
                Assert.Equal(newCustomer.Age, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.Age)).NewValue);
            }

            [Fact]
            public void DetectsStringDelta_WhenStringIsNull()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "originalFirstName"
                };

                var newCustomer = new
                {
                    FirstName = default(string)
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaObjects = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaObjects);
                Assert.Single(deltaObjects);
                Assert.Null(deltaObjects[0].NewValue);
                Assert.Equal(ConversionStatus.Valid, deltaObjects[0].ConversionStatus);
            }

            [Fact]
            public void DetectsStringDelta_WhenStringIsEmpty()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "originalFirstName"
                };

                var newCustomer = new
                {
                    FirstName = string.Empty
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaObjects = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaObjects);
                Assert.Single(deltaObjects);
                Assert.Equal(string.Empty, deltaObjects[0].NewValue);
                Assert.Equal(ConversionStatus.Valid, deltaObjects[0].ConversionStatus);
            }

            [Fact]
            public void ReturnsZeroDeltaObjects_WhenPropertiesAreNull()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = default(string)
                };

                var newCustomer = new
                {
                    FirstName = default(string)
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaObjects = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaObjects);
                Assert.Empty(deltaObjects);
            }

            [Fact]
            public void ReturnsZeroDeltaObjects_WhenPropertyDefaultAndIgnoreOnDefault()
            {
                var originalCustomer = new TestCustomerWithIgnoreOnDefaultAttributeOnClass
                {
                    FirstName = "originalFirstName"
                };

                var newCustomer = new
                {
                    FirstName = default(string)
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaObjects = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaObjects);
                Assert.Empty(deltaObjects);
            }

            [Fact]
            public void DetectsDelta_WhenStringEmptyAndIgnoreOnDefault()
            {
                var originalCustomer = new TestCustomerWithIgnoreOnDefaultAttributeOnClass
                {
                    FirstName = "originalFirstName"
                };

                var newCustomer = new
                {
                    FirstName = string.Empty
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaObjects = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaObjects);
                Assert.Single(deltaObjects);
                Assert.Equal(string.Empty, deltaObjects[0].NewValue);
                Assert.Equal(ConversionStatus.Valid, deltaObjects[0].ConversionStatus);
            }
        }
    }
}
