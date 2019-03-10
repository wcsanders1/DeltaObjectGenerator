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
            }
        }
    }
}
