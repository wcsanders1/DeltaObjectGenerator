using System;

namespace DeltaObjectGeneratorTests.TestModels
{
    [Flags]
    public enum TestFlagEnum
    {
        OneThing    = 0b00000001,
        SecondThing = 0b00000010,
        ThirdThing  = 0b00000100
    }
}
