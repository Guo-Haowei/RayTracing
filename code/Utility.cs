using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public class Utility
    {
        public static float RandomF()
        {
            // Returns a random float in [0 1)
            return (float)rand.NextDouble();
        }
        public static float RandomF(float min, float max)
        {
            // Returns a random float in [min max)
            return min + (max - min) * RandomF();
        }

        public static float clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(min, value), max);
        }

        static private Random rand = new Random();
    }

}

