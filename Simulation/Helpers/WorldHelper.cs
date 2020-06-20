using Game.ExtensionMethods;
using Game.GeneticAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Helpers
{
    /// <summary>
    /// A set of static helper methods to interact with a sequence based genetic algorithm.
    /// </summary>
    public static class WorldHelper
    {
        private static Random random = new Random();

        public static List<Individual> SpawnPopulation()
        {
            var population = new List<Individual>();

            // Generate {PopulationCount} individuals
            while (population.Count < GAConfig.PopulationCount)
            {
                var individual = GenerateIndividual(Configuration.TownCount);
                if (!population.Contains(individual))
                {
                    population.Add(individual);
                }
            }

            return population;
        }

        public static Individual GenerateIndividual(int sequenceLength)
        {
            // Generate a list of numbers [0, 1, 2, 3... 9]
            var sequence = Enumerable.Range(0, sequenceLength).ToList();

            // Randomly shuffle the list [3, 1, 5, 9... 4]
            sequence.Shuffle();

            // Create a new individual with our random sequence
            return new Individual(sequence);
        }

        public static (Individual, Individual) GetCandidateParents(List<Individual> population)
        {
            // Grab two random individuals from the population
            var candidateA = population[random.Next(population.Count())];
            var candidateB = population[random.Next(population.Count())];

            // Ensure that the two individuals are unique
            while (candidateA == candidateB)
            {
                candidateB = population[random.Next(population.Count())];
            }

            return (candidateA, candidateB);
        }

        public static Individual TournamentSelection(Individual candidateA, Individual candidateB)
        {
            // Return the individual that has the higher fitness value
            if (candidateA.Rank < candidateB.Rank)
            {
                return candidateA;
            }
            else if (candidateA.Rank == candidateB.Rank)
            {
                return candidateA.CrowdingDistance > candidateB.CrowdingDistance
                    ? candidateA
                    : candidateB;
            }
            else
            {
                return candidateB;
            }
        }

        public static Individual DoCrossover(Individual individualA, Individual individualB, int crossoverPosition = -1)
        {
            // Generate a number between 1 and sequence length - 1 to be our crossover position
            crossoverPosition = crossoverPosition == -1 
                ? random.Next(1, individualA.Sequence.Count - 1)
                : crossoverPosition;

            // Grab the head from the first individual
            var offspringSequence = individualA.Sequence.Take(crossoverPosition).ToList();

            // Create a hash for quicker 'exists in head' checks
            var appeared = offspringSequence.ToHashSet();

            // Append individualB to the head, skipping any values that have already shown up in the head
            foreach (var town in individualB.Sequence)
            {
                if (appeared.Contains(town))
                {
                    continue;
                }

                offspringSequence.Add(town);
            }

            // Return our new offspring!
            return new Individual(offspringSequence);
        }

        public static (int, int) GetUniqueTowns(List<int> sequence)
        {
            // Randomly select two towns
            var townA = random.Next(sequence.Count());
            var townB = random.Next(sequence.Count());

            // Ensure that the two towns are not the same
            while (townB == townA)
            {
                townB = random.Next(sequence.Count());
            }

            return (townA, townB);
        }

        public static Individual DoRotateMutate(Individual individual)
        {
            // Grab two unique towns
            var (townA, townB) = GetUniqueTowns(individual.Sequence);

            // Grab a reference to the sequence - just to make code below tidier
            var sequence = individual.Sequence;

            // Determine which of the indices chosen comes before the other
            int firstIndex = townA < townB ? townA : townB;
            int secondIndex = townA > townB ? townA : townB;

            // Grab the head of the sequence
            var newSequence = sequence.Take(firstIndex).ToList();

            // Grab the centre and rotate it
            var middle = sequence.Skip(firstIndex).Take(secondIndex - firstIndex).Reverse();

            // Grab the end of the sequence
            var end = sequence.Skip(secondIndex).ToList();

            // Add all components of the new sequence together
            newSequence.AddRange(middle);
            newSequence.AddRange(end);

            // Return a new individual with our new sequence
            return new Individual(newSequence);
        }

        public static Individual DoSwapMutate(Individual individual)
        {
            // Grab a copy of our current sequence
            var sequence = individual.Sequence.ToList();

            // Get the indices of the towns we want to swap
            var (townA, townB) = GetUniqueTowns(individual.Sequence);

            sequence.SwapInPlace(townA, townB);

            return new Individual(sequence);
        }

        public static (Individual, Individual) Mutate(Individual individualA, Individual individualB)
        {
            // Grab a copy of our individual in its current state, not the most efficient way
            // but certainly a very testable way.
            var newIndividualA = new Individual(individualA.Sequence);
            var newindividualB = new Individual(individualB.Sequence);

            // Generate a number between 0-1, if it is lower than our mutation chance (0.05 - 5%), mutate!
            if (random.NextDouble() < GAConfig.MutationChance)
            {
                newIndividualA = DoMutate(individualA);
            }

            // Generate a number between 0-1, if it is lower than our mutation chance (0.05 - 5%), mutate!
            if (random.NextDouble() < GAConfig.MutationChance)
            {
                newindividualB = DoMutate(individualB);
            }

            return (newIndividualA, newindividualB);
        }

        private static Individual DoMutate(Individual individual)
        {
            // Half the time, use one mutation method, and other half use the other.
            if (random.NextDouble() > 0.5)
            {
                return DoSwapMutate(individual);
            }
            else
            {
                return DoRotateMutate(individual);
            }
        }
    }
}
