using Game.GeneticAlgorithm;
using System.Collections.Generic;

namespace Game.Tests.Helpers
{
    public static class DefaultPopulationHelper
    {
        public static List<Individual> GetTestPopulation()
        {
            // Create our individuals
            var a = new Individual(new List<int>() { 0 }) { TimeFitness = 1, DistanceFitness = 5 };
            var b = new Individual(new List<int>() { 1 }) { TimeFitness = 3, DistanceFitness = 3 };
            var c = new Individual(new List<int>() { 2 }) { TimeFitness = 5, DistanceFitness = 1 };
            var d = new Individual(new List<int>() { 3 }) { TimeFitness = 2, DistanceFitness = 6 };
            var e = new Individual(new List<int>() { 4 }) { TimeFitness = 4, DistanceFitness = 4 };
            var f = new Individual(new List<int>() { 5 }) { TimeFitness = 6, DistanceFitness = 2 };
            var g = new Individual(new List<int>() { 6 }) { TimeFitness = 5, DistanceFitness = 4 };

            var population = new List<Individual>()
            {
                a, b, c, d, e, f, g
            };

            return population;
        }
    }
}

