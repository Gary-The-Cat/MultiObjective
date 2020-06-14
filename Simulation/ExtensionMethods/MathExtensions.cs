using System;

namespace Game.ExtensionMethods
{
    public static class MathExtensions
    {
        public static float PI = 3.14159265358979323f;

        public static float Sin(double x)
        {
            return (float)Math.Sin(x);
        }

        public static float Cos(double x)
        {
            return (float)Math.Cos(x);
        }

        public static float Atan2(double y, double x)
        {
            return (float)Math.Atan2(y, x);
        }

        public static float Sqrt(double x)
        {
            return (float)Math.Sqrt(x);
        }

        public static float Abs(double value)
        {
            return (float)Math.Abs(value);
        }

        public static float Clamp(float value, float lower, float upper)
        {
            if(value < lower)
            {
                return lower;
            }
            if(value > upper)
            {
                return upper;
            }
            return value;
        }
    }
}
