using Game.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.GeneticAlgorithm
{
    public class Individual
    {
        // The Genome of our individual.
        public List<int> Sequence { get; set; }

        public Individual(List<int> sequence)
        {
            this.Sequence = sequence;
        }

        /// <summary>
        /// Gets the fitness that results from 'decoding' our individual.
        /// AKA Converting the genome (sequence) into the resultant path.
        /// </summary>
        /// <returns></returns>
        public double GetFitness()
        {
            var totalDistance = 0.0;

            // Loop over each of the line segments and add them up to get the total path distance.
            for (int i = 1; i < this.Sequence.Count(); i++)
            {
                var fromTown = TownHelper.TownPositions[Sequence[i - 1]];
                var toTown = TownHelper.TownPositions[Sequence[i]];

                var x = toTown.X - fromTown.X;
                var y = toTown.Y - fromTown.Y;

                var d = Math.Sqrt(x * x + y * y);

                totalDistance += d;
            }

            return totalDistance;
        }
    }
}
