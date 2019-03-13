using DeltaObjectGenerator.Extensions;
using DeltaObjectGeneratorTests.TestModels;
using Xunit;

namespace DeltaObjectGeneratorTests.Unit
{
    public class PropertyInfoExtensionsUnitTests
    {
        [Trait("Category", "Unit")]
        public class GetAlias
        {
            [Fact]
            public void ReturnsAlias_WhenAliasExists()
            {
                var result = typeof(TestCustomerWithAlias)
                    .GetProperty(nameof(TestCustomerWithAlias.LastName))
                    .GetAlias();

                Assert.Equal("last_name", result);
            }

            [Fact]
            public void ReturnsNull_WhenAliasDoesNotExist()
            {
                var result = typeof(TestCustomerWithAlias)
                    .GetProperty(nameof(TestCustomerWithAlias.FirstName))
                    .GetAlias();

                Assert.Null(result);
            }
        }
    }
}
