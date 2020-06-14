using Game.ExtensionMethods;
using SFML.Graphics;
using SFML.System;
using System;

namespace Game.Helpers
{
    public static class SFMLGraphicsHelper
    {
        public static ConvexShape GetLine(Vector2f startingPoint, Vector2f endingPoint, int thickness, Color color)
        {
            // Get the vectors required to calculate the vertices of our rectanle (line)
            var lineDirection = (endingPoint - startingPoint);
            var perpLeft = lineDirection.PerendicularCounterClockwise().Normalize();
            var perpRight = lineDirection.PerendicularClockwise().Normalize();

            var shape = new ConvexShape(4)
            {
                FillColor = color
            };

            // Define points clockwise
            shape.SetPoint(0, startingPoint + (thickness * perpLeft));
            shape.SetPoint(1, endingPoint + (thickness * perpLeft));
            shape.SetPoint(2, endingPoint + (thickness * perpRight));
            shape.SetPoint(3, startingPoint + (thickness * perpRight));

            return shape;
        }
    }
}
