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

        public static float Clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(min, value), max);
        }

        public static Vector3 Refract(in Vector3 uv, in Vector3 n, float refractionPower)
        {
            float cosTheta = Vector3.Dot(-uv, n);
            Vector3 outParallel = refractionPower * (uv + cosTheta * n);
            Vector3 outPerpendicular = -(float)Math.Sqrt(1.0f - Vector3.Dot(outParallel, outParallel)) * n;
            return outParallel + outPerpendicular;
        }

        public static float Schlick(float cosine, float reflectionPower)
        {
            float r0 = (1.0f - reflectionPower) / (1.0f + reflectionPower);
            r0 = r0 * r0; 
            return r0 + (1.0f - r0) * (float)Math.Pow(1 - cosine, 5.0f);
        }

        public static float DegreeToRadians(float degree)
        {
            return ((float)Math.PI / 180.0f) * degree;
        }

        [ThreadStatic]
        private static Random random;
    }
}

