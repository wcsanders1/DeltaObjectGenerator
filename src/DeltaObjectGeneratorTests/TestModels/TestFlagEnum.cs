using System;

namespace DeltaObjectGeneratorTests.TestModels
{
    [Flags]
    public enum TestFlagEnum
    {
        OneThing    = 0b00000000,
        SecondThing = 0b00000001,
        ThirdThing  = 0b00000010
    }
}
