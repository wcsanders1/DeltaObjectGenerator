using DeltaObjectGenerator.Extensions;
using DeltaObjectGenerator.Models;
using DeltaObjectGeneratorTests.TestModels;
using System.Collections.Generic;
using Xunit;

namespace DeltaObjectGeneratorTests.Unit
{
    public class GenericExtensionsUnitTests
    {
        [Trait("Category", "Unit")]
        public class GetDeltaObjects
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
            }
        }

    }
}
