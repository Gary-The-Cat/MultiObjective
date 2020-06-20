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

        public int Rank { get; set; }

        public double CrowdingDistance { get; set; }

        public float DistanceFitness { get; set; }

        public float TimeFitness { get; set; }

        public float NormalizedDistanceFitness { get; set; }

        public float NormalizedTimeFitness { get; set; }

        public Individual(List<int> sequence)
        {
            this.Sequence = sequence;

            DistanceFitness = GetTotalDistance();
            TimeFitness = GetTotalTime();
        }

        /// <summary>
        /// Gets the distance that results from decoding our individual.
        /// AKA Converting the genome (sequence) into the resultant path.
        /// </summary>
        /// <returns>A float value being the distance of the path in pixels.</returns>
        public float GetTotalDistance()
        {
            var totalDistance = 0.0f;

            // Loop over each of the line segments and add them up to get the total path distance.
            for (int i = 1; i < this.Sequence.Count(); i++)
            {
                var fromTown = TownHelper.TownPositions[Sequence[i - 1]];
                var toTown = TownHelper.TownPositions[Sequence[i]];

                var x = toTown.X - fromTown.X;
                var y = toTown.Y - fromTown.Y;

                var d = (float)Math.Sqrt(x * x + y * y);

                totalDistance += d;
            }

            return totalDistance;
        }

        /// <summary>
        /// Gets the time taken that results from decoding our individual.
        /// AKA Converting the genome (sequence) into the time taken to traverse the resultant path.
        /// </summary>
        /// <returns>A float value indicating the time taken to travel the path in seconds.</returns>
        public float GetTotalTime()
        {
            var totalTime = 0.0f;

            // Loop over each of the line segments and add them up to get the total path distance.
            for (int i = 1; i < this.Sequence.Count(); i++)
            {
                var fromTown = TownHelper.TownPositions[Sequence[i - 1]];
                var toTown = TownHelper.TownPositions[Sequence[i]];

                var x = toTown.X - fromTown.X;
                var y = toTown.Y - fromTown.Y;

                var d = (float)Math.Sqrt(x * x + y * y);

                totalTime += d / TownHelper.PathSpeedLimits[(i-1, i)];
            }

            return totalTime;
        }

        public override bool Equals(object obj)
        {
            if (obj is Individual individual)
            {
                return this.Sequence.SequenceEqual(individual.Sequence);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(Individual a, Individual b)
        {
            return a.Sequence.SequenceEqual(b.Sequence);
        }

        public static bool operator !=(Individual a, Individual b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
