using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.GeneticAlgorithm
{
    public static class GAConfig
    {
        public static int MaxGenerations => 10000;

        public static double MutationChance => 0.05;

        public static int PopulationCount => 1000;

        public static int MaxNoImprovementCount => 20;
    }
}
