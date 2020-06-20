namespace Game.GeneticAlgorithm
{
    public static class GAConfig
    {
        public static int MaxGenerations => 10000;

        public static double MutationChance => 0.05;

        public static int PopulationCount => 100;

        public static int MaxNoImprovementCount => 20;
    }
}
