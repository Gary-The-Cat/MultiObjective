using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ExtensionMethods
{
    public static class Vector2fExtensions
    {
        // Sets x and y to zero
        public static void Zero(this Vector2f v)
        {
            v.X = 0;
            v.Y = 0;
        }

        // Returns true if both x and y are zero
        public static bool IsZero(this Vector2f v)
        {
            return v.X == 0 && v.Y == 0;
        }

        //returns the length of the vector
        public static float Magnitude(this Vector2f v)
        {
            return MathExtensions.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public static float GetAngle(this Vector2f v)
        {
            return MathExtensions.Atan2(v.Y, v.X);
        }

        //returns the length of the vector
        public static float Magnitude(this Vector2f v, Vector2f v2)
        {
            return MathExtensions.Sqrt(Math.Pow(v.X - v2.X, 2) + Math.Pow(v.Y - v2.Y, 2));
        }

        //returns the squared length of the vector (thereby avoiding the sqrt)
        public static float LengthSq(this Vector2f v)
        {
            return v.X * v.X + v.Y * v.Y;
        }

        //returns the length of the vector
        public static float MagnitudeSquared(this Vector2f v, Vector2f v2)
        {
            return v.X * v2.X + v.Y * v2.Y;
        }

        //returns the length of the vector
        public static float MagnitudeSquared(this Vector2f v)
        {
            return v.X * v.X + v.Y * v.Y;
        }

        public static Vector2f Normalize(this Vector2f v)
        {
            var magnitude = v.Magnitude();
            if (magnitude == 0)
            {
                return new Vector2f();
            }

            return new Vector2f(v.X / magnitude, v.Y / magnitude);
        }

        // Scale the vector by 'scale'
        public static void Scale(this Vector2f v, float scale)
        {
            v.X *= scale;
            v.Y *= scale;
        }

        //returns the dot product of this and v2
        public static float Dot(this Vector2f v, Vector2f vector)
        {
            return v.X * vector.X + v.Y * vector.Y;
        }
        
        //returns the vector that is perpendicular to this one
        public static Vector2f PerendicularClockwise(this Vector2f v)
        {
            return new Vector2f(v.Y, -v.X);
        }

        //returns the vector that is perpendicular to this one
        public static Vector2f PerendicularCounterClockwise(this Vector2f v)
        {
            return new Vector2f(-v.Y, v.X);
        }

        //adjusts x and y so that the length of the vector does not exceed max
        public static void Truncate(this Vector2f v, float max)
        {
            if (v.Magnitude() < max)
            {
                return;
            }

            var normalized = v.Normalize();
            v.X = normalized.X;
            v.Y = normalized.Y;

            v.Scale(max);
        }

        // returns the distance between this vector and the one passed as a parameter
        public static float Distance(this Vector2f v, Vector2f v2)
        {
            return MathExtensions.Sqrt(Math.Pow(v.X - v2.X, 2) + Math.Pow(v.Y - v2.Y, 2));
        }

        ////squared version of above
        public static float DistanceSquared(this Vector2f v, Vector2f v2)
        {
            return (float)(Math.Pow(v.X - v2.X, 2) + Math.Pow(v.Y - v2.Y, 2));
        }

        //returns the vector that is the reverse of this vector
        public static Vector2f GetReverse(this Vector2f v)
        {
            return new Vector2f(-v.X, -v.Y);
        }
    }
}
