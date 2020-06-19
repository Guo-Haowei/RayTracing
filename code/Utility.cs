using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public class Utility
    {
        public static float RandomF(Random random)
        {
            // Returns a random float in [0 1)
            return (float)random.NextDouble();
        }
        public static float RandomF(float min, float max, Random random)
        {
            // Returns a random float in [min max)
            return min + (max - min) * RandomF(random);
        }

        public static float clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(min, value), max);
        }

    }

}

