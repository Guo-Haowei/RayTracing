using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public class Utility
    {
        // Returns a random float in [0 1)
        public static float RandomF()
        {
            if (random == null)
            {
                random = new Random();
            }
            return (float)random.NextDouble();
        }

        // Returns a random float in [min max)
        public static float RandomF(float min, float max)
        {
            return min + (max - min) * RandomF();
        }

        public static Vector3 RandomUnitVector()
        {
            float a = RandomF(0, 2.0f * (float)Math.PI);
            float z = RandomF(-1.0f, 1.0f);
            float r = (float)Math.Sqrt(1.0f - z * z);
            return new Vector3(r * (float)Math.Cos(a), r * (float)Math.Sin(a), z);
        }

        public static Vector3 RandomUnitVector(in Vector3 normal)
        {
            Vector3 ret = RandomUnitVector();
            float flip = Math.Sign(Vector3.Dot(normal, ret));
            return flip * ret;
        }

        public static float clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(min, value), max);
        }

        [ThreadStatic]
        private static Random random;
    }
}

