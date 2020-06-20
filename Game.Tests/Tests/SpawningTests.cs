using Game.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Game.Tests
{
    [TestClass()]
    public class SpawningTests
    {
        [TestMethod()]
        public void EnsureSpawningUniqueTest()
        {
            TownHelper.Initialize();

            var population = WorldHelper.SpawnPopulation();

            var populationSequences = population.Select(i => i.Sequence);

            // Manually check that each individual in the population is unique
            foreach(var sequenceA in populationSequences)
            {
                foreach (var sequenceB in populationSequences)
                {
                    if(sequenceA == sequenceB)
                    {
                        continue;
                    }

                    Assert.IsTrue(!sequenceA.SequenceEqual(sequenceB));
                }
            }
        }
    }
}

