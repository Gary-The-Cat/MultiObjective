using Game.GeneticAlgorithm;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Screens
{
    public class ParetoVisualScreen : Screen
    {
        private List<Individual> population;
        private RectangleShape fitnessGraph;
        private RectangleShape fitnessGraphBackdrop;
        private List<CircleShape> individualVisuals;

        // Hold static offsets to account for the size of the graph...
        // The graph is just a png coz I'm lazy - sorry.
        private static Vector2f zeroOffset = new Vector2f(1508, 1304);
        private static Vector2f graphSize = new Vector2f(670, 670);

        /// <summary>
        /// Pareto Visual Screen - A simple screen that can display a 2D graph with individual
        /// fitness values graphed on it.
        /// </summary>
        /// <param name="window">Reference to our window we want to draw to</param>
        /// <param name="configuration">The position of our screen inside the window</param>
        /// <param name="population">The population that we want to display</param>
        public ParetoVisualScreen(
            RenderWindow window, 
            FloatRect configuration,
            List<Individual> population) : base(window, configuration)
        {
            // Grab a reference to the population
            this.population = population;

            // Create our container to hold our visuals for our fitness values
            this.individualVisuals = new List<CircleShape>();

            // We will create all our visuals up front to save on the overhead of creating them each frame.
            for (int i = 0; i < GAConfig.PopulationCount; i++)
            {
                individualVisuals.Add(new CircleShape(10));
            }

            // Create a simple rectangle to hold our fitness graph.
            // This could just be a sprite, but this works.
            this.fitnessGraph = new RectangleShape(new Vector2f(960, 960))
            {
                Texture = new Texture("Graph.png"),
                Position = new Vector2f(1400, 500)
            };

            // Light backdrop with black border for our graph visualisation.
            // Slightly offset to look a bit nicer.
            this.fitnessGraphBackdrop = new RectangleShape(new Vector2f(1060, 960))
            {
                Position = new Vector2f(1350, 500),
                FillColor = new Color(0xee, 0xee, 0xee),
                OutlineColor = new Color(0x1e, 0x1e, 0x1e),
                OutlineThickness = 2
            };
        }

        public override void Update(float deltaT)
        {
            // Normalized fitness values causes our least fit individual in the population to 
            // have a fittness of 1. But our most fit individual could have a value of anywhere
            // between 0 -> 1. So we need to stretch our normalized fitness values over the
            // range 0 -> 1 for visualisaiton purposes. This means our most fit individual will
            // always be 0 on their respective axis.
            // Time
            var minTime = population.Min(i => i.NormalizedTimeFitness);
            var maxTime = population.Max(i => i.NormalizedTimeFitness);
            var diffTime = maxTime - minTime;

            // Distance
            var minDistance = population.Min(i => i.NormalizedDistanceFitness);
            var maxDistance = population.Max(i => i.NormalizedDistanceFitness);
            var diffdistance = maxDistance - minDistance;

            // As we are coloruing by rank, we want to spread our colours over the available range.
            var maxRank = population.Max(p => p.Rank);
            var step = 255 / maxRank;

            for (int i = 0; i < population.Count; i++)
            {
                // Grab a reference to our current individual and its visual - just cleaner below.
                var visual = individualVisuals[i];
                var individual = population[i];

                // Maps from [0,1] [0,1] to the bottom left of the axis to the top right.
                var x = (individual.NormalizedTimeFitness - minTime) / diffTime * graphSize.X;
                var y = -(individual.NormalizedDistanceFitness - minDistance) / diffdistance * graphSize.Y;
                visual.Position = new Vector2f(x, y) + zeroOffset;

                // Get the intensity of the colour value & update it.
                var intensity = individual.Rank * step;
                visual.FillColor = new Color((byte)intensity, 0, (byte)(255 -intensity));
            }
        }

        public override void Draw(float deltaT)
        {
            // Draw the fitness backgrop & outline
            window.Draw(this.fitnessGraphBackdrop);

            // Draw the graph
            window.Draw(this.fitnessGraph);

            // Draw each individual on the graph
            individualVisuals.ForEach(v => window.Draw(v));
        }
    }
}

