using DeltaObjectGenerator.Generators;
using DeltaObjectGeneratorTests.TestModels;
using Newtonsoft.Json.Linq;
using Xunit;

namespace DeltaObjectGeneratorTests.Unit
{
    public class DeltaObjectFromJObjectGeneratorUnitTests
    {
        [Trait("Category", "Unit")]
        public class GetDeltaObjects
        {
            [Fact]
            public void ReturnsDeltaObject_WhenDeltaExists()
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
            }
        }
    }
}
