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

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Single(deltaGroup.DeltaObjects);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects[0].ValueConversionStatus);
                Assert.Equal(newCustomer.FirstName, deltaGroup.DeltaObjects[0].NewValue);
            }

            [Fact]
            public void IgnoresProperty_IfNotInJObject()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "originalFirstName",
                    LastName = "originalLastName"
                };

                var newCustomer = new
                {
                    FirstName = "newFirstName"
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Single(deltaGroup.DeltaObjects);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects[0].ValueConversionStatus);
                Assert.Equal(newCustomer.FirstName, deltaGroup.DeltaObjects[0].NewValue);
            }

            [Fact]
            public void IsCaseInsensitive_WhenDetectingDelta()
            {
                var originalCustomer = new TestCustomer
                {
                    FirstName = "originalFirstName",
                    LastName = "originalLastName"
                };

                var newCustomer = new
                {
                    firstName = "newFirstName"
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Single(deltaGroup.DeltaObjects);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects[0].ValueConversionStatus);
                Assert.Equal(newCustomer.firstName, deltaGroup.DeltaObjects[0].NewValue);
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

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Single(deltaGroup.DeltaObjects);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects[0].ValueConversionStatus);
                Assert.Equal(newCustomer.Age, deltaGroup.DeltaObjects[0].NewValue);
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

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Fail, deltaGroup.ValueConversionStatus);
                Assert.Single(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Empty(deltaGroup.DeltaObjects);
                Assert.Equal(ValueConversionStatus.Fail, deltaGroup.DeltaObjectsValueConversionFail[0].ValueConversionStatus);
                Assert.Equal(newCustomer.Age, deltaGroup.DeltaObjectsValueConversionFail[0].NewValue);
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

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(2, deltaGroup.DeltaObjects.Count);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.FirstName)).ValueConversionStatus);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.DateOfBirth)).ValueConversionStatus);
                Assert.Equal(newCustomer.FirstName, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.FirstName)).NewValue);
                Assert.Equal(newCustomer.DateOfBirth, deltaGroup.DeltaObjects.First(o =>
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

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(2, deltaGroup.DeltaObjects.Count);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.FirstName)).ValueConversionStatus);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.DateOfBirth)).ValueConversionStatus);
                Assert.Equal(newCustomer.FirstName, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.FirstName)).NewValue);
                Assert.Equal(new DateTime(1979, 12, 8), deltaGroup.DeltaObjects.First(o =>
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

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Partial, deltaGroup.ValueConversionStatus);
                Assert.Single(deltaGroup.DeltaObjects);
                Assert.Single(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.FirstName)).ValueConversionStatus);
                Assert.Equal(ValueConversionStatus.Fail, deltaGroup.DeltaObjectsValueConversionFail.First(o =>
                    o.PropertyName == nameof(TestCustomer.DateOfBirth)).ValueConversionStatus);
                Assert.Equal(newCustomer.FirstName, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.FirstName)).NewValue);
                Assert.Equal(newCustomer.DateOfBirth, deltaGroup.DeltaObjectsValueConversionFail.First(o =>
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

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Equal(2, deltaGroup.DeltaObjects.Count);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.DateOfBirth)).ValueConversionStatus);
                Assert.Equal(newCustomer.DateOfBirth, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.DateOfBirth)).NewValue);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomer.Age)).ValueConversionStatus);
                Assert.Equal(newCustomer.Age, deltaGroup.DeltaObjects.First(o =>
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

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Single(deltaGroup.DeltaObjects);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Null(deltaGroup.DeltaObjects[0].NewValue);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects[0].ValueConversionStatus);
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

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Single(deltaGroup.DeltaObjects);
                Assert.Equal(string.Empty, deltaGroup.DeltaObjects[0].NewValue);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects[0].ValueConversionStatus);
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

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Empty(deltaGroup.DeltaObjects);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
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

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Empty(deltaGroup.DeltaObjects);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
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

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Single(deltaGroup.DeltaObjects);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Equal(string.Empty, deltaGroup.DeltaObjects[0].NewValue);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects[0].ValueConversionStatus);
            }

            [Fact]
            public void DetectsDelta_WhenPropertyIsEnum()
            {
                var originalCustomer = new TestCustomerWithEnum
                {
                    Age = 40,
                    SomeEnum = TestEnum.Nothing
                };

                var newCustomer = new
                {
                    Age = 41,
                    SomeEnum = TestEnum.Something
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Equal(2, deltaGroup.DeltaObjects.Count);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithEnum.Age)).ValueConversionStatus);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithEnum.SomeEnum)).ValueConversionStatus);
                Assert.Equal(newCustomer.Age, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithEnum.Age)).NewValue);
                Assert.Equal(TestEnum.Something, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithEnum.SomeEnum)).NewValue);
            }

            [Fact]
            public void DetectsDelta_WhenPropertyIsFlagEnum()
            {
                var originalCustomer = new TestCustomerWithFlagEnum
                {
                    SomeFlagEnum = TestFlagEnum.OneThing | TestFlagEnum.SecondThing
                };

                var newCustomer = new
                {
                    SomeNeedlessStupidProperty = "bad things",
                    SomeFlagEnum = TestFlagEnum.OneThing | TestFlagEnum.ThirdThing
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Single(deltaGroup.DeltaObjects);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithFlagEnum.SomeFlagEnum)).ValueConversionStatus);
                Assert.Equal(TestFlagEnum.OneThing | TestFlagEnum.ThirdThing, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithFlagEnum.SomeFlagEnum)).NewValue);
            }

            [Fact]
            public void IgnoresDefaultFlagEnum_WhenClassHasIgnoreOnDefaultAttribute()
            {
                var originalCustomer = new TestCustomerWithIgnoreOnDefaultAttributeOnClass
                {
                    SomeFlagStuff = TestFlagEnum.OneThing | TestFlagEnum.SecondThing
                };

                var newCustomer = new
                {
                    SomeNeedlessStupidProperty = "bad things",
                    SomeFlagStuff = TestFlagEnum.OneThing
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Empty(deltaGroup.DeltaObjects);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
            }

            [Fact]
            public void DetectsDeltaOnDefaultFlagEnum_WhenClassLacksIgnoreOnDefaultFlag()
            {
                var originalCustomer = new TestCustomerWithFlagEnum
                {
                    SomeFlagEnum = TestFlagEnum.OneThing | TestFlagEnum.SecondThing
                };

                var newCustomer = new
                {
                    SomeNeedlessStupidProperty = "bad things",
                    SomeFlagEnum = TestFlagEnum.OneThing
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Single(deltaGroup.DeltaObjects);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithFlagEnum.SomeFlagEnum)).ValueConversionStatus);
                Assert.Equal(TestFlagEnum.OneThing, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithFlagEnum.SomeFlagEnum)).NewValue);
            }

            [Fact]
            public void DetectsDelta_WhenDeltaOnNullableProperty()
            {
                var originalCustomer = new TestCustomerWithNullable
                {
                    Account = new TestAccount
                    {
                        Balance = 123
                    }
                };

                var newCustomer = new
                {
                    Account = new
                    {
                        Balance = 321
                    },
                    Salary = 12M
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
                Assert.Single(deltaGroup.DeltaObjects);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Equal(ValueConversionStatus.Success, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithNullable.Salary)).ValueConversionStatus);
                Assert.Equal(newCustomer.Salary, deltaGroup.DeltaObjects.First(o =>
                    o.PropertyName == nameof(TestCustomerWithNullable.Salary)).NewValue);
            }

            [Fact]
            public void DoesNotDetectFalseDelta_WhenDecimalUnchanged()
            {
                var originalCustomer = new TestCustomerWithNullable
                {
                    Account = new TestAccount
                    {
                        Balance = 123
                    },
                    Salary = 12
                };

                var newCustomer = new
                {
                    Account = new
                    {
                        Balance = 321
                    },
                    Salary = 12
                };

                var newCustomerJObj = JObject.FromObject(newCustomer);

                var deltaGroup = DeltaObjectFromJObjectGenerator.GetDeltaObjects(originalCustomer, newCustomerJObj);

                Assert.NotNull(deltaGroup);
                Assert.Empty(deltaGroup.DeltaObjects);
                Assert.Empty(deltaGroup.DeltaObjectsValueConversionFail);
                Assert.Equal(GroupValueConversionStatus.Success, deltaGroup.ValueConversionStatus);
            }
        }
    }
}
