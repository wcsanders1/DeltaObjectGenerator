# DeltaObjectGenerator

[![Build status](https://ci.appveyor.com/api/projects/status/5hkp4iq90mwned3c?svg=true)](https://ci.appveyor.com/project/wcsanders1/deltaobjectgenerator) [![Coverage Status](https://coveralls.io/repos/github/wcsanders1/DeltaObjectGenerator/badge.svg?branch=master)](https://coveralls.io/github/wcsanders1/DeltaObjectGenerator?branch=master) ![Nuget](https://img.shields.io/nuget/v/deltaobjectgenerator.svg) ![Nuget](https://img.shields.io/nuget/dt/deltaobjectgenerator.svg)

### Table of Contents

- [Overview](#overview)
- [Details](#details)
- [Examples](#examples)
  - [Calculating deltas using two objects of the same type](#same-type)
  - [Calculating deltas using an object and `JObject`](#jobject)
- [Attributes](#attributes)
  - [`DeltaObjectAlias`](#delta-object-alias)
  - [`DeltaObjectIgnore`](#delta-object-ignore)
  - [`DeltaObjectIgnoreOnDefault`](#delta-object-ignore-on-default)
- [Performance](#performance)
- [License](#license)

## <a id="overview">Overview</a>

This is a library offering the extension method `GetDeltaObjects` on `T` and accepting an argument of `T` or `JObject`. The method returns a collection of objects containing information concerning any deltas, i.e., changes, between the properties of the two objects. This can be useful for creating audit logs or generating SQL. For example, if you have a `PATCH` endpoint receiving an instance of an object and you want to generate SQL updating only the changed properties of the object, this library provides a list of objects--one for each changed property--containing information that can be used to create that SQL.

Compatible with the following:

- .NET Framework 4.5
- .NET Framework 4.6
- .NET Framework 4.6.1
- .NET Standard 1.6
- .NET Standard 2.0

## <a id="details">Details</a>

The extension method `GetDeltaObjects` returns a `List<DeltaObject>`. A `DeltaObject` has the following public properties concerning a property with a delta:

- `PropertyName` The name of the property.
- `PropertyAlias` The alias of the property. This defaults to the property's name, and can be set using the `DeltaObjectAlias` attribute, discussed below.
- `OriginalValue` The original value of the property.
- `NewValue` The new value of the property.
- `StringifiedOriginalValue` The original value of the property as a - `string`.
- `StringifiedNewValue` The new value of the property as a `string`.
- `ConversionStatus` The status of the conversion of the new value of the property into the property's type. This is an `enum` with fields of `Valid` and `Invalid`. Note that this property is only relevant when a `DeltaObject` is calculated using a `JObject`, since it is only in that situation where it is possible that a new value could not be converted into the property's type.

A `DeltaObject` is generated **only** for non-indexed properties of the following types:

- primitives
- `decimal`
- `string`
- `DateTime`
- `DateTimeOffset`
- `TimeSpan`
- `Guid`
- nullables, e.g., `int?`, `DateTime?`
- enums

Any property on an object not among the above types will be ignored by the delta-object generator. In addition, you may add attributes to properties or to a class to have the delta-object generator ignore certain properties in certain situations, discussed below.

## <a id="examples">Examples</a>

#### <a id="same-type">Calculating deltas using two objects of the same type</a>

```cs
public class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Transactions { get; set; }
    public DateTime DateOfBirth { get; set; }
}

var originalCustomer = new Customer
{
    FirstName = "originalFirstName",
    LastName = "originalLastName",
    Transactions = 30,
    DateOfBirth = new DateTime(1919, 10, 10)
};

var updatedCustomer = new Customer
{
    FirstName = "newFirstName",
    LastName = "originalLastName",
    Transactions = 95,
    DateOfBirth = new DateTime(2009, 2, 3)
};

var deltaObjects = originalCustomer.GetDeltaObjects(updatedCustomer);

foreach (var deltaObject in deltaObjects)
{
    Console.WriteLine(
        $"Property name: {deltaObject.PropertyName}\n" +
        $"Property alias: {deltaObject.PropertyAlias}\n" +
        $"Original value: {deltaObject.OriginalValue}\n" +
        $"New value: {deltaObject.NewValue}\n\n" +
        $"********************************************\n");
}
```

The above code will print the following to the console:

```
Property name: FirstName
Property alias: FirstName
Original value: originalFirstName
New value: newFirstName

********************************************

Property name: Transactions
Property alias: Transactions
Original value: 30
New value: 95

********************************************

Property name: DateOfBirth
Property alias: DateOfBirth
Original value: 10/10/1919 12:00:00 AM
New value: 2/3/2009 12:00:00 AM

********************************************
```

As you can see, a `DeltaObject` was generated for each property on `Customer` that had a changed value.

#### <a id="jobject">Calculating deltas using an object and `JObject`</a>

```cs
public class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Transactions { get; set; }
    public DateTime DateOfBirth { get; set; }
}

var originalCustomer = new Customer
{
    FirstName = "originalFirstName",
    LastName = "originalLastName",
    Transactions = 30,
    DateOfBirth = new DateTime(1919, 10, 10)
};

var newCustomer = new
{
    FirstName = "newFirstName",
    LastName = "originalLastName",
    Transactions = "fifty",
    DateOfBirth = "December 8, 1979"
};

var newCustomerJObject = JObject.FromObject(newCustomer);

var customerDeltaObjects = originalCustomer.GetDeltaObjects(newCustomerJObject);

foreach (var deltaObject in customerDeltaObjects)
{
    Console.WriteLine(
        $"Property name: {deltaObject.PropertyName}\n" +
        $"Property alias: {deltaObject.PropertyAlias}\n" +
        $"Original value: {deltaObject.OriginalValue}\n" +
        $"New value: {deltaObject.NewValue}\n" +
        $"Conversion status: {deltaObject.ConversionStatus}\n\n" +
        $"********************************************\n");
}
```

The above code will print the following to the console:

```
Property name: FirstName
Property alias: FirstName
Original value: originalFirstName
New value: newFirstName
Conversion status: Valid

********************************************

Property name: Transactions
Property alias: Transactions
Original value: 30
New value: fifty
Conversion status: Invalid

********************************************

Property name: DateOfBirth
Property alias: DateOfBirth
Original value: 10/10/1919 12:00:00 AM
New value: 12/8/1979 12:00:00 AM
Conversion status: Valid

********************************************
```

Notice that the conversion status of the new value for the `Transactions` property is `Invalid` because the `string` "fifty" cannot be converted into an `int`.

## <a id="attributes">Attributes</a>

#### <a id="delta-object-alias">`DeltaObjectAlias`</a>

This attribute can be applied to a property. It assigns a value to the `PropertyAlias` property of the `DeltaObject`. For example:

```cs
public class Customer
{
    public string FirstName { get; set; }

    [DeltaObjectAlias("last_name")]
    public string LastName { get; set; }
}

var originalCustomer = new Customer
{
    FirstName = "originalFirstName",
    LastName = "originalLastName"
};

var newCustomerWithAlias = new Customer
{
    FirstName = "newFirstName",
    LastName = "newLastName"
};

var deltaObjects = originalCustomer.GetDeltaObject(newCustomerWithAlias);

foreach (var deltaObject in deltaObjects)
{
    Console.WriteLine(
        $"Property name: {deltaObject.PropertyName}\n" +
        $"Property alias: {deltaObject.PropertyAlias}\n" +
        $"Original value: {deltaObject.OriginalValue}\n" +
        $"New value: {deltaObject.NewValue}\n\n" +
        $"********************************************\n");
}
```

The above code will print the following to the console:

```
Property name: FirstName
Property alias: FirstName
Original value: originalFirstName
New value: newFirstName

********************************************

Property name: LastName
Property alias: last_name
Original value: originalLastName
New value: newLastName

********************************************
```

As you can see, the `LastName` property has an alias of `last_name` in its `DeltaObject`, and any property without a specified alias will have an alias on its `DeltaObject` equal to the name of the property. This can be useful if you want to generate SQL and the property name differs from the database column.

#### <a id="delta-object-ignore">`DeltaObjectIgnore`</a>

This attribute can be applied to a property to force the delta-object generator to ignore that property in all cases.

#### <a id="delta-object-ignore-on-default">`DeltaObjectIgnoreOnDefault`</a>

This attribute can be applied to a property to force the delta-object generator to ignore that property when its value is equal to the property type's default value. This attribute can also be applied to a class, which will force the delta-object generator to ignore all properties on the class whose value is default.

## <a id="performance">Performance</a>

The delta-object generator uses reflection to get information about a type and, for the sake of performance, caches the results in a static class. Below are some performance measurements observed on an i7-6700 4.00GHz CPU without running operations in parallel (note that the "Properties on Object" column counts only those properties that the delta-object generator does not ignore):

| Number of Objects | Properties on Object | Seconds to Generate Delta Objects |
| :---------------: | :------------------: | --------------------------------: |
|       1000        |          4           |                             0.011 |
|     1 million     |          4           |                             3.150 |
|     1 million     |          20          |                            11.857 |

## <a id="license">License</a>

[MIT](https://github.com/wcsanders1/DeltaObjectGenerator/blob/master/LICENSE)
