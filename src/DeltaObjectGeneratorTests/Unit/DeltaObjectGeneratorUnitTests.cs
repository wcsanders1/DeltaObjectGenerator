﻿using Xunit;
using DeltaObjectGenerator.Generators;
using DeltaObjectGeneratorTests.TestModels;
using System.Linq;
using Newtonsoft.Json.Linq;
using DeltaObjectGenerator.Models;

namespace DeltaObjectGeneratorTests.Unit
{
    public class DeltaObjectGeneratorUnitTests
    {
        [Trait("Category", "Unit")]
        public class GetDeltaObjects_Object
        {
            [Fact]
            public void ReturnsDeltaObjects_WhenCalled()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "original name"
                };

                var newCustomer = new TestCustomer
                {
                    FirstName = "new name"
                };

                var sut = new DeltaObjectEngine();

                var deltaObjects = sut.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.Single(deltaObjects);
                Assert.Equal(nameof(TestCustomer.FirstName), deltaObjects.First().PropertyName);
                Assert.Equal(nameof(TestCustomer.FirstName), deltaObjects.First().PropertyAlias);
                Assert.Equal(originalCustomer.FirstName, deltaObjects.First().OriginalValue);
                Assert.Equal(newCustomer.FirstName, deltaObjects.First().NewValue);
            }
        }

        [Trait("Category", "Unit")]
        public class GetDeltaObjects_JObject
        {
            [Fact]
            public void ReturnsDeltaObjects_WhenCalled()
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

                var sut = new DeltaObjectEngine();

                var deltaGroup = sut.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.NoneFailed, deltaGroup.ValueConversionStatus);
                Assert.Single(deltaGroup.DeltaObjects);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects[0].ValueConversionStatus);
                Assert.Equal(newCustomer.FirstName, deltaGroup.DeltaObjects[0].NewValue);
            }
        }
    }
}
