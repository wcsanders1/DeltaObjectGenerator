using DeltaObjectGenerator.Generators;
using DeltaObjectGenerator.Models;
using DeltaObjectGeneratorTests.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DeltaObjectGeneratorTests.Unit
{
    public class DeltaObjectFromObjectGeneratorUnitTests
    {
        [Trait("Category", "Unit")]
        public class GetDeltaObjects
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

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.Single(deltaObjects);
                Assert.Equal(nameof(TestCustomer.FirstName), deltaObjects.First().PropertyName);
                Assert.Equal(nameof(TestCustomer.FirstName), deltaObjects.First().PropertyAlias);
                Assert.Equal(originalCustomer.FirstName, deltaObjects.First().OriginalValue);
                Assert.Equal(newCustomer.FirstName, deltaObjects.First().NewValue);
            }

            [Fact]
            public void ReturnsZeroDeltaObjects_WhenPropertiesAreNull()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = default(string)
                };

                var newCustomer = new TestCustomer
                {
                    FirstName = default(string)
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

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

                var newCustomer = new TestCustomerWithIgnoreOnDefaultAttributeOnClass
                {
                    FirstName = string.Empty
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.NotNull(deltaObjects);
                Assert.Single(deltaObjects);
                Assert.Equal(string.Empty, deltaObjects[0].NewValue);
                Assert.Equal(ConversionStatus.Valid, deltaObjects[0].ConversionStatus);
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

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

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

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

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

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

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

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.Equal(2, deltaObjects.Count);
                Assert.Equal(newCustomer.Salary, deltaObjects.First(o => o.PropertyName ==
                    nameof(TestCustomerWithNullable.Salary)).NewValue);
                Assert.Equal(newCustomer.Age, deltaObjects.First(o => o.PropertyName ==
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

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.Equal(2, deltaObjects.Count);
                Assert.Null(deltaObjects.First(o => o.PropertyName ==
                    nameof(TestCustomerWithNullable.Salary)).NewValue);
                Assert.Equal(newCustomer.Age, deltaObjects.First(o => o.PropertyName ==
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

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.Single(deltaObjects);
                Assert.Equal(newCustomer.Age, deltaObjects.First(o => o.PropertyName ==
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

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.Equal(2, deltaObjects.Count);
                Assert.Equal("last_name", deltaObjects.First(o => o.PropertyName ==
                    (nameof(TestCustomerWithAlias.LastName))).PropertyAlias);
                Assert.Equal(nameof(TestCustomerWithAlias.FirstName), deltaObjects.First(o => o.PropertyName ==
                    (nameof(TestCustomerWithAlias.FirstName))).PropertyAlias);
            }

            [Fact]
            public void ReturnsEnumDelta_WhenPropertyHasEnumDelta()
            {
                var originalCustomer = new TestCustomerWithEnum
                {
                    SomeEnum = TestEnum.Nothing
                };

                var newCustomer = new TestCustomerWithEnum
                {
                    SomeEnum = TestEnum.Something
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.Single(deltaObjects);
            }

            [Fact]
            public void ReturnsFlagEnumDelta_WhenPropertyHasFlagEnumDelta()
            {
                var originalCustomer = new TestCustomerWithFlagEnum
                {
                    SomeFlagEnum = TestFlagEnum.OneThing
                };

                var newCustomer = new TestCustomerWithFlagEnum
                {
                    SomeFlagEnum = TestFlagEnum.OneThing | TestFlagEnum.SecondThing
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.Single(deltaObjects);
            }

            [Fact]
            public void ReturnsFlagEnumDelta_WhenNewFlagPropertyHasFewerFlags()
            {
                var originalCustomer = new TestCustomerWithFlagEnum
                {
                    SomeFlagEnum = TestFlagEnum.OneThing | TestFlagEnum.SecondThing
                };

                var newCustomer = new TestCustomerWithFlagEnum
                {
                    SomeFlagEnum = TestFlagEnum.OneThing,
                    FirstName = "newName"
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.Equal(2, deltaObjects.Count);
                Assert.Equal(newCustomer.SomeFlagEnum, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithFlagEnum.SomeFlagEnum)).NewValue);
                Assert.Equal(newCustomer.FirstName, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithFlagEnum.FirstName)).NewValue);
            }

            [Fact]
            public void IgnoresArray_WhenPropertyHasArray()
            {
                var originalCustomer = new TestCustomerWithArray
                {
                    FavoriteNumbers = new[] { 4, 7 }
                };

                var newCustomer = new TestCustomerWithArray
                {
                    FavoriteNumbers = new[] { 9, 0, 100 }
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.Empty(deltaObjects);
            }

            [Fact]
            public void IgnoresPrivateProperty_WhenPrivatePropertyHasDelta()
            {
                var originalCustomer = new TestCustomer();
                originalCustomer.SetSecret("original secret");

                var newCustomer = new TestCustomer();
                newCustomer.SetSecret("new secret");

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.Empty(deltaObjects);
            }

            [Fact]
            public void DoesNotThrow_IfString()
            {
                var originalString = "bill";
                var newString = "sam";

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalString, newString);

                Assert.NotNull(deltaObjects);
            }

            [Fact]
            public void DoesNotThrow_IfInt()
            {
                var originalAge = 4;
                var newAge = 5;

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalAge, newAge);

                Assert.NotNull(deltaObjects);
            }

            [Fact]
            public void DoesNotThrow_IfCollection()
            {
                var originalCustomers = new List<TestCustomer>
                {
                    new TestCustomer
                    {
                        FirstName = "goodName"
                    },
                    new TestCustomer
                    {
                        FirstName = "sillyName"
                    }
                };

                var newCustomers = new List<TestCustomer>
                {
                    new TestCustomer
                    {
                        FirstName = "goodNameAgain"
                    },
                    new TestCustomer
                    {
                        FirstName = "sillyNameAgain"
                    },
                    new TestCustomer
                    {
                        FirstName = "doesNotMatter"
                    }
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomers, newCustomers);

                Assert.NotNull(deltaObjects);
            }

            [Fact]
            public void ReturnsEmptyList_WhenNoDeltas()
            {
                var originalCustomer = new TestCustomerWithEnum
                {
                    SomeEnum = TestEnum.Nothing,
                    FirstName = "neat",
                    LastName = "name",
                    Age = 40,
                    DateOfBirth = new DateTime(1979, 12, 8),
                    Account = new TestAccount()
                };

                var newCustomer = new TestCustomerWithEnum
                {
                    SomeEnum = TestEnum.Nothing,
                    FirstName = "neat",
                    LastName = "name",
                    Age = 40,
                    DateOfBirth = new DateTime(1979, 12, 8),
                    Account = new TestAccount
                    {
                        Balance = 23
                    }
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.NotNull(deltaObjects);
                Assert.IsType<List<DeltaObject>>(deltaObjects);
                Assert.Empty(deltaObjects);
            }

            [Fact]
            public void ReturnsDelta_WhenDateTimeDeltaExists()
            {
                var originalCustomer = new TestCustomer
                {
                    DateOfBirth = new DateTime(1979, 12, 8)
                };

                var newCustomer = new TestCustomer
                {
                    DateOfBirth = new DateTime(1979, 12, 7)
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.Single(deltaObjects);
            }

            [Fact]
            public void RecognizesDeltasOfPropertiesOnAbstractClass_WhenModelInheritsFromAbstractClass()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "original name",
                    Address = "original address",
                    Age = 34
                };

                var newCustomer = new TestCustomer
                {
                    FirstName = "new name",
                    Address = "new address",
                    Age = 34
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.Equal(2, deltaObjects.Count);
                Assert.Equal(newCustomer.Address, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.Address)).NewValue);
            }

            [Fact]
            public void RecognizesDeltasOfPropertiesOnPartialClass_WhenClassIsPartial()
            {
                var originalArtist = new TestPartialArtist
                {
                    IsGood = false,
                    Name = "Picaso"
                };

                var newArtist = new TestPartialArtist
                {
                    IsGood = true,
                    Name = "Picaso"
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalArtist, newArtist);

                Assert.Single(deltaObjects);
                Assert.Equal(newArtist.IsGood, deltaObjects[0].NewValue);
            }

            [Fact]
            public void DoesNotGenerateDeltaObjects_WhenAllValuesDefaultAndAllPropertiesHaveIgnoreOnDefaultAttribute()
            {
                var originalCustomer = new TestCustomerWithAllPropertiesIgnoreOnDefault
                {
                    FirstName = "goodName",
                    LastName = "neatName",
                    Age = 432,
                    SomeStuff = TestEnum.SomethingMore,
                    SomeFlagStuff = TestFlagEnum.SecondThing | TestFlagEnum.ThirdThing,
                    DateOfBirth = new DateTime(1919, 10, 10),
                    StartDate = new DateTime(1980, 11, 11)
                };

                var newCustomer = new TestCustomerWithAllPropertiesIgnoreOnDefault();

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.NotNull(deltaObjects);
                Assert.Empty(deltaObjects);
            }

            [Fact]
            public void ReturnsOneDeltaObject_WhenNullableDateTimeDeltaExists()
            {
                var originalCustomer = new TestCustomerWithAllPropertiesIgnoreOnDefault();
                var newCustomer = new TestCustomerWithAllPropertiesIgnoreOnDefault
                {
                    StartDate = new DateTime(1872, 1, 12)
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.NotNull(deltaObjects);
                Assert.Single(deltaObjects);
                Assert.Equal(newCustomer.StartDate, deltaObjects[0].NewValue);
            }

            [Fact]
            public void IgnoresDefaultValues_WhenClassHasIgnoreOnDefaultAttribute()
            {
                var originalCustomer = new TestCustomerWithIgnoreOnDefaultAttributeOnClass
                {
                    FirstName = "goodName",
                    LastName = "neatName",
                    Age = 432,
                    SomeStuff = TestEnum.SomethingMore,
                    SomeFlagStuff = TestFlagEnum.SecondThing | TestFlagEnum.ThirdThing,
                    DateOfBirth = new DateTime(1919, 10, 10),
                    StartDate = new DateTime(1980, 11, 11)
                };

                var newCustomer = new TestCustomerWithIgnoreOnDefaultAttributeOnClass();

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.NotNull(deltaObjects);
                Assert.Empty(deltaObjects);
            }

            [Fact]
            public void ReturnsDeltaObjectsForNonDefaultValues_WhenClassHasIgnoreOnDefaultAttribute()
            {
                var originalCustomer = new TestCustomerWithIgnoreOnDefaultAttributeOnClass
                {
                    FirstName = "goodName",
                    LastName = "neatName",
                    Age = 432,
                    SomeStuff = TestEnum.SomethingMore,
                    SomeFlagStuff = TestFlagEnum.SecondThing | TestFlagEnum.ThirdThing,
                    DateOfBirth = new DateTime(1919, 10, 10),
                    StartDate = new DateTime(1980, 11, 11)
                };

                var newCustomer = new TestCustomerWithIgnoreOnDefaultAttributeOnClass
                {
                    FirstName = "badName",
                    Age = 12,
                    SomeStuff = TestEnum.Nothing
                };

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomer, newCustomer);

                Assert.NotNull(deltaObjects);
                Assert.Equal(3, deltaObjects.Count);
                Assert.Equal(newCustomer.FirstName, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithIgnoreOnDefaultAttributeOnClass.FirstName))
                    .NewValue);
                Assert.Equal(newCustomer.Age, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithIgnoreOnDefaultAttributeOnClass.Age))
                    .NewValue);
                Assert.Equal(newCustomer.SomeStuff, deltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithIgnoreOnDefaultAttributeOnClass.SomeStuff))
                    .NewValue);
            }

            [Fact]
            public void ReturnsEmptyList_WhenObjectHasNoProperties()
            {
                var propertylessCustomer = new TestCustomerWithoutProperties();

                var deltaObjects = DeltaObjectFromObjectGenerator.GetDeltaObjects(propertylessCustomer,
                    new TestCustomerWithoutProperties());

                Assert.NotNull(deltaObjects);
                Assert.IsType<List<DeltaObject>>(deltaObjects);
                Assert.Empty(deltaObjects);
            }

            [Fact]
            public void ThrowsArgumentNullException_WhenFirstArgNull()
            {
                Assert.Throws<ArgumentNullException>(() => 
                    DeltaObjectFromObjectGenerator.GetDeltaObjects(null, new TestCustomer()));
            }

            [Fact]
            public void ThrowsArgumentNullException_WhenSecondArgNull()
            {
                Assert.Throws<ArgumentNullException>(() =>
                    DeltaObjectFromObjectGenerator.GetDeltaObjects(new TestCustomer(), null));
            }
        }
    }
}
