# DeltaObjectGenerator

This is a work in progress. Plese check back again later.

## Things to do:

- Add attribute for `enum` to determine how it should be compared, either by string, int value, or hash code (for flags?)
- Add attribute to allow property names to be different in delta object
- Add attribute to ignore specific properties
- Instead of making the update object a `Dictionary`, make it a custome object containing the original value, the updated value, the property name, and any alternative property name as specified in the attribute mentioned above
