using DeltaObjectGenerator.Caches;
using DeltaObjectGeneratorTests.TestModels;
using KellermanSoftware.CompareNetObjects;
using System;
using System.Collections.Concurrent;
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
                        BindingFlags.NonPublic | BindingFlags.Static)
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

            [Fact]
            public void CachesPropertyInfo_WhenNewTypeProvided()
            {
                var propertyInfoCache = typeof(TypeCache)
                    .GetProperty("PropertyInfoByType", 
                        BindingFlags.NonPublic | BindingFlags.Static)
                    .GetValue(null) as ConcurrentDictionary<Type, List<PropertyInfo>>;

                Assert.NotNull(propertyInfoCache);
                Assert.Empty(propertyInfoCache);

                var customerPropertiesFirstCall = TypeCache.GetPropertyInfo<TestCustomer>();

                Assert.Single(propertyInfoCache);
                Assert.True(propertyInfoCache.TryGetValue(typeof(TestCustomer), out _));

                var customerPropertiesSecondCall = TypeCache.GetPropertyInfo<TestCustomer>();

                Assert.Single(propertyInfoCache);
                Assert.True(propertyInfoCache.TryGetValue(typeof(TestCustomer), out _));

                var comparedResult = GetCompareLogic().Compare(customerPropertiesFirstCall,
                    customerPropertiesSecondCall);

                Assert.True(comparedResult.AreEqual);

                var accountProperties = TypeCache.GetPropertyInfo<TestAccount>();

                Assert.Equal(2, propertyInfoCache.Count);
                Assert.True(propertyInfoCache.TryGetValue(typeof(TestCustomer), out _));
                Assert.True(propertyInfoCache.TryGetValue(typeof(TestAccount), out _));
            }
        }

        private static CompareLogic GetCompareLogic()
        {
            return new CompareLogic(new ComparisonConfig
            {
                MaxDifferences = 100
            });
        }
    }
}
