using Game.ExtensionMethods;
using Game.Helpers;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Game.GeneticAlgorithm
{
    public class World
    {
        private static Random random = new Random();

        public List<Individual> Population { get; set; }

        public List<double> FitnessOverTime { get; private set; }

        public int GenerationCount { get; private set; } = 0;

        public int NoImprovementCount { get; private set; } = 0;

        public bool HasConverged =>
              GenerationCount > GAConfig.MaxGenerations
            || NoImprovementCount > GAConfig.MaxNoImprovementCount;

        public World()
        {
            Population = new List<Individual>();
            FitnessOverTime = new List<double>();
        }

        public void Spawn()
        {
            this.Population.AddRange(WorldHelper.SpawnPopulation());
        }

        public void DoGeneration()
        {
            GenerationCount++;

            // Create a list to hold our new offspring
            var offspring = new List<Individual>();

            // While our offspring are less than our current population count, create new offspring
            while (offspring.Count < GAConfig.PopulationCount)
            {
                // Get parents
                var mother = GetParent();
                var father = GetParent();

                // Handle the case where we have picked the same individual as both parents
                while (mother == father)
                {
                    father = GetParent();
                }

                // Perform Crossover
                var (offspringA, offspringB) = GetOffspring(mother, father);

                // Mutate
                (offspringA, offspringB) = Mutate(offspringA, offspringB);

                // Add offspring to population
                offspring.Add(offspringA);
                offspring.Add(offspringB);
            }

            // Add all the offspring to our existing population
            Population.AddRange(offspring);

            MultiObjectiveHelper.UpdatePopulationFitness(Population);

            // Take the best 'PopulationCount' worth of individuals
            var newPopulation = new List<Individual>();
            
            foreach (var individual in Population.OrderBy(i => i.Rank).ThenByDescending(i => i.CrowdingDistance))
            {
                if (!newPopulation.Contains(individual))
                {
                    newPopulation.Add(individual);
                }
            }

            newPopulation = newPopulation.Take(GAConfig.PopulationCount).ToList();

            Population.Clear();

            newPopulation.ForEach(i => Population.Add(i));
        }

        public Individual GetBestIndividual()
        {
            // We no longer have a 'best' individual, so we are going to show a random one from the first front.
            var firstRank = Population.GroupBy(i => i.Rank).First().ToArray();
            return firstRank[random.Next(firstRank.Length)];
        }

        private (Individual, Individual) Mutate(Individual individualA, Individual individualB)
        {
            return WorldHelper.Mutate(individualA, individualB);
        }

        private (Individual, Individual) GetOffspring(Individual individualA, Individual individualB)
        {
            // Generate the offspring from our selected parents
            var offspringA = DoCrossover(individualA, individualB);
            var offspringB = DoCrossover(individualB, individualA);

            return (offspringA, offspringB);
        }

        private Individual DoCrossover(Individual individualA, Individual individualB)
        {
            return WorldHelper.DoCrossover(individualA, individualB);
        }

        private Individual GetParent()
        {
            // Grab two candidate parents from the population.
            var (candidate1, candidate2) = WorldHelper.GetCandidateParents(this.Population);

            // Perform the tournament selection
            return WorldHelper.TournamentSelection(candidate1, candidate2);
        }
    }
}