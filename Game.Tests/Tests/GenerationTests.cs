using Game.ExtensionMethods;
using Game.GeneticAlgorithm;
using Game.Helpers;
using Game.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Game.Tests
{
    [TestClass()]
    public class GenerationTests
    {
        [TestMethod()]
        public void GenerateIndividualTest()
        {
            TownHelper.Initialize();

            var individual = WorldHelper.GenerateIndividual(Configuration.TownCount);

            // Ensure that individual contains no repeated values
            var uniqueValueGroups = individual.Sequence.GroupBy(s => s);
            Assert.IsTrue(uniqueValueGroups.All(g => g.Count() == 1));
        }

        [TestMethod()]
        public void EnsureCandidateParentsUniqueTest()
        {
            var population = DefaultPopulationHelper.GetTestPopulation();

            for(int i = 0; i < 10; i++)
            {
                var (candidateA, candidateB) = WorldHelper.GetCandidateParents(population);

                Assert.IsFalse(candidateA.Sequence.SequenceEqual(candidateB.Sequence));
            }
        }

        [TestMethod()]
        public void EnsureRankedTournamentSelectionTest()
        {
            var population = DefaultPopulationHelper.GetTestPopulation();

            MultiObjectiveHelper.UpdatePopulationFitness(population);

            // Rank 1
            var individualA = population[1];

            // Rank 2
            var individualB = population[3];

            var fitterIndividualA = WorldHelper.TournamentSelection(individualA, individualB);
            var fitterIndividualB = WorldHelper.TournamentSelection(individualB, individualA);

            Assert.AreEqual(individualA, fitterIndividualA);
            Assert.AreEqual(individualA, fitterIndividualB);
        }

        [TestMethod()]
        public void EnsureCrowdingDistanceTournamentSelectionTest()
        {
            var population = DefaultPopulationHelper.GetTestPopulation();

            MultiObjectiveHelper.UpdatePopulationFitness(population);

            // Rank 1, float.MaxValue crowding distance
            var individualA = population[0];

            // Rank 1, ~5.65 crowding distance
            var individualB = population[1];

            var fitterIndividualA = WorldHelper.TournamentSelection(individualA, individualB);
            var fitterIndividualB = WorldHelper.TournamentSelection(individualB, individualA);

            Assert.AreEqual(individualA, fitterIndividualA);
            Assert.AreEqual(individualA, fitterIndividualB);
        }

        [TestMethod()]
        public void EnsureCrossoverTest()
        {
            TownHelper.Initialize();

            var individualA = new Individual(new List<int> { 0, 9, 1, 8, 2, 7, 3, 6, 4, 5 });
            var individualB = new Individual(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            var crossoverPointA = 3;
            var crossoverPointB = 1;
            var crossoverPointC = 8;

            var childA = WorldHelper.DoCrossover(individualA, individualB, crossoverPointA);
            var childB = WorldHelper.DoCrossover(individualA, individualB, crossoverPointB);
            var childC = WorldHelper.DoCrossover(individualA, individualB, crossoverPointC);

            var expectedChildASequence = new List<int> { 0, 9, 1, 2, 3, 4, 5, 6, 7, 8 };
            var expectedChildBSequence = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expectedChildCSequence = new List<int> { 0, 9, 1, 8, 2, 7, 3, 6, 4, 5 };

            Assert.IsTrue(childA.Sequence.SequenceEqual(expectedChildASequence));
            Assert.IsTrue(childB.Sequence.SequenceEqual(expectedChildBSequence));
            Assert.IsTrue(childC.Sequence.SequenceEqual(expectedChildCSequence));

            Assert.AreEqual(childA.Sequence.Count(), individualA.Sequence.Count());
            Assert.AreEqual(childB.Sequence.Count(), individualA.Sequence.Count());
            Assert.AreEqual(childC.Sequence.Count(), individualA.Sequence.Count());
        }

        [TestMethod()]
        public void EnsureUniqueTownsTest()
        {
            var sequence = new List<int> { 0, 9, 1, 2, 3, 4, 5, 6, 7, 8 };

            for (int i = 0; i < 10; i++)
            {
                var (townA, townB) = WorldHelper.GetUniqueTowns(sequence);

                Assert.AreNotEqual(townA, townB);
            }
        }

        [TestMethod()]
        public void EnsureRotationMutationResultTest()
        {
            TownHelper.Initialize();

            var individual = new Individual(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            var result = WorldHelper.DoRotateMutate(individual);

            // Ensure that all values are still in the list
            var distinctResultCount = result.Sequence.Distinct().Count();
            Assert.AreEqual(distinctResultCount, 10);

            // Ensure that there are no duplicate entries
            var sequences = result.Sequence.GroupBy(s => s);
            Assert.IsTrue(sequences.All(s => s.Count() == 1));
        }

        [TestMethod()]
        public void EnsureSwapMutationResultTest()
        {
            TownHelper.Initialize();

            var individual = new Individual(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            var result = WorldHelper.DoSwapMutate(individual);

            // Ensure that all values are still in the list
            var distinctResultCount = result.Sequence.Distinct().Count();
            Assert.AreEqual(distinctResultCount, 10);

            // Ensure that there are no duplicate entries
            var sequences = result.Sequence.GroupBy(s => s);
            Assert.IsTrue(sequences.All(s => s.Count() == 1));

            // Perform manual swap to ensure result is correct
            var firstIndex = -1;
            var lastIndex = -1;
            for (int i = 0; i < individual.Sequence.Count; i++)
            {
                if (firstIndex == -1 && result.Sequence[i] != individual.Sequence[i])
                {
                    firstIndex = i;
                    continue;
                }

                if (firstIndex != -1 && result.Sequence[i] != individual.Sequence[i])
                {
                    lastIndex = i;
                    break;
                }
            }

            var originalSequence = individual.Sequence.ToList();
            originalSequence.SwapInPlace(firstIndex, lastIndex);

            Assert.IsTrue(originalSequence.SequenceEqual(result.Sequence));
        }
    }
}

