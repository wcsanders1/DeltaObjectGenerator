using DeltaObjectGenerator.Geneators;
using DeltaObjectGenerator.Models;
using DeltaObjectGeneratorTests.TestModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace DeltaObjectGeneratorTests.Manual
{
    public class DeltaObjectFromObjectGeneratorManualTests
    {
        [Trait("Category", "Manual")]
        public class GetDeltaObjects
        {
            [Fact]
            public void ProcessesOneMillionObjectsInLessThanFiveSeconds_WhenObjectsHaveFourDeltaProperties()
            {
                const int objectAmount = 1000000;
                var originalCustomers = new List<TestCustomer>();
                var newCustomers = new List<TestCustomer>();

                for (var i = 0; i < objectAmount; i++)
                {
                    originalCustomers.Add(new TestCustomer
                    {
                        FirstName = Guid.NewGuid().ToString(),
                        LastName = Guid.NewGuid().ToString()
                    });

                    newCustomers.Add(new TestCustomer
                    {
                        FirstName = Guid.NewGuid().ToString(),
                        LastName = Guid.NewGuid().ToString()
                    });
                }

                var stopWatch = new Stopwatch();
                var deltaObjects = new List<DeltaObject>();

                stopWatch.Start();

                for (var i = 0; i < objectAmount; i++)
                {
                    deltaObjects.AddRange(DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCustomers[i], newCustomers[i]));
                };

                stopWatch.Stop();

                Assert.Equal(objectAmount * 2, deltaObjects.Count);
                Assert.True(stopWatch.Elapsed.Seconds < 5);
            }

            [Fact]
            public void ProcessesOneMillionObjectsInLessThanTwentyFiveSeconds_WhenObjectsHaveTwentyDeltaProperties()
            {
                const int objectAmount = 1000000;

                var originalCriminals = new List<TestCriminal>();
                var newCriminals = new List<TestCriminal>();

                for (var i = 0; i < objectAmount; i++)
                {
                    originalCriminals.Add(new TestCriminal
                    {
                        Name1 = Guid.NewGuid().ToString(),
                        Name2 = Guid.NewGuid().ToString(),
                        Name3 = Guid.NewGuid().ToString(),
                        Name4 = Guid.NewGuid().ToString(),
                        Name5 = Guid.NewGuid().ToString(),
                        Name6 = Guid.NewGuid().ToString(),
                        Name7 = Guid.NewGuid().ToString(),
                        Name8 = Guid.NewGuid().ToString(),
                        Name9 = Guid.NewGuid().ToString(),
                        Name10 = Guid.NewGuid().ToString(),
                        Name11 = Guid.NewGuid().ToString(),
                        Name12 = Guid.NewGuid().ToString(),
                        Name13 = Guid.NewGuid().ToString(),
                        Name14 = Guid.NewGuid().ToString(),
                        Name15 = Guid.NewGuid().ToString(),
                        Name16 = Guid.NewGuid().ToString(),
                        Name17 = Guid.NewGuid().ToString(),
                        Name18 = Guid.NewGuid().ToString(),
                        Name19 = Guid.NewGuid().ToString(),
                        Name20 = Guid.NewGuid().ToString()
                    });

                    newCriminals.Add(new TestCriminal
                    {
                        Name1 = Guid.NewGuid().ToString(),
                        Name2 = Guid.NewGuid().ToString(),
                        Name3 = Guid.NewGuid().ToString(),
                        Name4 = Guid.NewGuid().ToString(),
                        Name5 = Guid.NewGuid().ToString(),
                        Name6 = Guid.NewGuid().ToString(),
                        Name7 = Guid.NewGuid().ToString(),
                        Name8 = Guid.NewGuid().ToString(),
                        Name9 = Guid.NewGuid().ToString(),
                        Name10 = Guid.NewGuid().ToString(),
                        Name11 = Guid.NewGuid().ToString(),
                        Name12 = Guid.NewGuid().ToString(),
                        Name13 = Guid.NewGuid().ToString(),
                        Name14 = Guid.NewGuid().ToString(),
                        Name15 = Guid.NewGuid().ToString(),
                        Name16 = Guid.NewGuid().ToString(),
                        Name17 = Guid.NewGuid().ToString(),
                        Name18 = Guid.NewGuid().ToString(),
                        Name19 = Guid.NewGuid().ToString(),
                        Name20 = Guid.NewGuid().ToString()
                    });
                }

                var stopWatch = new Stopwatch();
                var deltaObjects = new List<DeltaObject>();

                stopWatch.Start();

                for (var i = 0; i < objectAmount; i++)
                {
                    deltaObjects.AddRange(DeltaObjectFromObjectGenerator.GetDeltaObjects(originalCriminals[i], newCriminals[i]));
                }

                stopWatch.Stop();

                Assert.Equal(objectAmount * 20, deltaObjects.Count);
                Assert.True(stopWatch.Elapsed.Seconds < 25);
            }
        }
    }
}
