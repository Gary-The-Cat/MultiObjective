using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using Game.ExtensionMethods;

namespace Game.Helpers
{
    public static class TownHelper
    {
        private const int Linethickness = 4;
        private const int PathOffsetFromTown = 180;
        private const int MinimumSpeedInPixels = 10;
        private const int MaximumSpeedInPixels = 100;
        private const int SpeedRangeInPixels = MaximumSpeedInPixels - MinimumSpeedInPixels;
        private static Random random = new Random();

        /// <summary>
        /// To draw the path of our current sequence, we have to create a bunch of convex shapes.
        /// SFML has a native way to draw lines, but they are 1px wide, and do not show up well
        /// in recordings, so there's a little extra work to calculate the line.
        /// </summary>
        /// <param name="townSequence">The genome of the sequence we want to display</param>
        /// <returns>The line visuals for the requested path</returns>
        public static List<ConvexShape> GetTownSequencePath(List<int> townSequence)
        {
            var paths = new List<ConvexShape>();

            for(int i = 1; i < townSequence.Count; i++)
            {
                // Get the two towns that our line will be joining
                var fromTown = TownPositions[townSequence[i - 1]];
                var toTown = TownPositions[townSequence[i]];

                // Get the normalized vector in the direction of fromTown to toTown
                var directionVector = (toTown - fromTown).Normalize();

                // Now that we have the vector pointing from fromTown to toTown, we can traverse it to give our towns
                // some space around them when we draw our line.
                var startingPoint = fromTown + (directionVector * PathOffsetFromTown);
                var endingPoint = toTown - (directionVector * PathOffsetFromTown);

                // We want to fade the lines from black - grey to show the direction of the path
                var lumination = Convert.ToByte((200.0 / TownPositions.Count) * (i - 1));

                // Convert the points we have into a 'ConvexShape' :( damn SFML.
                paths.Add(SFMLGraphicsHelper.GetLine(startingPoint, endingPoint, Linethickness, new Color(lumination, lumination, lumination)));
            }

            return paths;
        }

        public static void Initialize()
        {
            PopulateTowns();
            PopulateSpeedLimits();
        }

        private static void PopulateSpeedLimits()
        {
            var localRandom = new Random(17);
            PathSpeedLimits = new Dictionary<(int, int), float>();

            for (int fromTown = 0; fromTown < Configuration.TownCount; fromTown++)
            {
                for (int toTown = 0; toTown < Configuration.TownCount; toTown++)
                {
                    // If our from town is our to town, no need to calculate a path
                    if (fromTown == toTown)
                    {
                        continue;
                    }

                    // Calculate the path distance as speed is distance dependent
                    var pathDistance = TownPositions[toTown].Distance(TownPositions[fromTown]);

                    // Add the speed for this directional path
                    PathSpeedLimits.Add(
                        (fromTown, toTown), 
                        (float)(MinimumSpeedInPixels + SpeedRangeInPixels * localRandom.NextDouble() * pathDistance / 1000));
                }
            }
        }

        private static void PopulateTowns()
        {
            if (Configuration.UseRandomTowns)
            {
                for(int i = 0; i < Configuration.RandomTownCount; i++)
                {
                    // Note that random town placements can overlap
                    TownPositions.Add(GeneratRandomTownPosition());
                }
            }
            else
            {
                TownPositions.AddRange(townPositions);
            }
        }

        private static Vector2f GeneratRandomTownPosition()
        {
            return new Vector2f
            {
                X = 100 + ((float)random.NextDouble() * (Configuration.Width - 100)),
                Y = 100 + ((float)random.NextDouble() * (Configuration.Height - 100))
            };
        }

        /// <summary>
        /// Hard coded town positions - They are hard coded for a 4K screen space, but with the new camera system this 
        /// should not impact you if you have a smaller screen as it should scale!
        /// </summary>
        public static List<Vector2f> TownPositions = new List<Vector2f>();

        public static Dictionary<(int, int), float> PathSpeedLimits { get; set; }

        private static List<Vector2f> townPositions = new List<Vector2f>()
        {
            new Vector2f(3060, 1300),
            new Vector2f(1050, 450),
            new Vector2f(450, 750),
            new Vector2f(690, 1890),
            new Vector2f(1410, 1830),
            new Vector2f(2070, 1560),
            new Vector2f(1725, 1080),
            new Vector2f(3360, 810),
            new Vector2f(3450, 1770),
            new Vector2f(2460, 240),
        };
    }
}
