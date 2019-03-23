using DeltaObjectGenerator.Extensions;
using DeltaObjectGenerator.Models;
using DeltaObjectGeneratorTests.TestModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Xunit;

namespace DeltaObjectGeneratorTests.Unit
{
    public class GenericExtensionsUnitTests
    {
        [Trait("Category", "Unit")]
        public class GetDeltaObjects_Object
        {
            [Fact]
            public void ReturnsDeltaObjects_WhenProvidedObjectWithDeltas()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "originalFirstName",
                    LastName = "originalLastName"
                };

                var newCustomer = new TestCustomer
                {
                    FirstName = "newFirstName",
                    LastName = "newLastName"
                };

                var deltaObjects = originalCustomer.GetDeltaObjects(newCustomer);

                Assert.NotNull(deltaObjects);
                Assert.IsType<List<DeltaObject>>(deltaObjects);
                Assert.Equal(2, deltaObjects.Count);
                Assert.True(deltaObjects.TrueForAll(o => o.ConversionStatus == ConversionStatus.Valid));
            }

            [Fact]
            public void ThrowsArgumentNullException_WhenFirstArgNull()
            {
                var originalCustomer = (TestCustomer)null;

                Assert.Throws<ArgumentNullException>(() =>
                    originalCustomer.GetDeltaObjects(new TestCustomer()));
            }

            [Fact]
            public void ThrowsArgumentNullException_WhenSecondArgNull()
            {
                var originalCustomer = new TestCustomer();

                Assert.Throws<ArgumentNullException>(() =>
                    originalCustomer.GetDeltaObjects((TestCustomer)null));
            }
        }

        [Trait("Category", "Unit")]
        public class GetDeltaObjects_JObject
        {
            [Fact]
            public void ReturnsDeltaObjects_WhenProvidedJObject()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "originalFirstName",
                    LastName = "originalLastName"
                };

                var newCustomer = new
                {
                    FirstName = "newFirstName",
                    LastName = "newLastName"
                };

                var newCustomerObj = JObject.FromObject(newCustomer);

                var deltaObjects = originalCustomer.GetDeltaObjects(newCustomerObj);

                Assert.NotNull(deltaObjects);
                Assert.IsType<List<DeltaObject>>(deltaObjects);
                Assert.Equal(2, deltaObjects.Count);
                Assert.True(deltaObjects.TrueForAll(o => o.ConversionStatus == ConversionStatus.Valid));
            }

            [Fact]
            public void ThrowsArgumentNullException_WhenFirstArgNull()
            {
                var originalCustomer = (TestCustomer)null;

                Assert.Throws<ArgumentNullException>(() =>
                    originalCustomer.GetDeltaObjects(new JObject()));
            }

            [Fact]
            public void ThrowsArgumentNullException_WhenSecondArgNull()
            {
                var originalCustomer = new TestCustomer();

                Assert.Throws<ArgumentNullException>(() =>
                    originalCustomer.GetDeltaObjects((JObject)null));
            }
        }
    }
}
