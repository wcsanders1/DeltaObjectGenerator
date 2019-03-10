using DeltaObjectGenerator.Caches;
using DeltaObjectGeneratorTests.TestModels;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace DeltaObjectGeneratorTests.Unit
{
    public class TypeCacheUnitTests
    {
        [Trait("Category", "Unit")]
        public class GetPropertyInfo
        {
            private List<Type> AcceptedNonPrimitiveTypes { get; }

            public GetPropertyInfo()
            {
                AcceptedNonPrimitiveTypes = typeof(TypeCache)
                    .GetField("AcceptedNonPrimitiveTypes", 
                        BindingFlags.NonPublic | 
                        BindingFlags.Static)
                    .GetValue(null) as List<Type>;
            }

            [Fact]
            public void DoesNotIncludeNonAcceptedTypes_WhenProvidedTypeIncludedNonAcceptedTypes()
            {
                var customerProperties = TypeCache.GetPropertyInfo<TestCustomer>();

                Assert.NotNull(customerProperties);
                Assert.IsType<List<PropertyInfo>>(customerProperties);
                Assert.NotEmpty(customerProperties);
                Assert.True(customerProperties.TrueForAll(p =>
                    p.PropertyType.IsPrimitive || 
                    p.PropertyType.IsEnum || 
                    AcceptedNonPrimitiveTypes.Contains(p.PropertyType)));
            }
        }
    }
}
