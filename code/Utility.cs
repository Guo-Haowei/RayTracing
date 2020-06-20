using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public class Utility
    {
        public static readonly float Pi = (float)Math.PI;
        public static readonly float HalfPi = (float)(0.5 * Math.PI);
        public static readonly float TwoPi = (float)(2.0f * Math.PI);

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

        public static float TanF(float a)
        {
            return (float)Math.Tan(a);
        }

        public static float Atan2F(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }

        public static float SinF(float a)
        {
            return (float)Math.Sin(a);
        }
        public static float AsinF(float d)
        {
            return (float)Math.Asin(d);
        }

        public static float CosF(float a)
        {
            return (float)Math.Cos(a);
        }

        public static float SqrtF(float d)
        {
            return (float)Math.Sqrt(d);
        }

        public static Vector3 RandomUnitVector()
        {
            float a = RandomF(0.0f, TwoPi);
            float z = RandomF(-1.0f, 1.0f);
            float r = (float)Math.Sqrt(1.0f - z * z);
            return new Vector3(r * CosF(a), r * SinF(a), z);
        }

        public static Vector3 RandomUnitVectorInHemisphere(in Vector3 normal)
        {
            Vector3 ret = RandomUnitVector();
            bool flip = Vector3.Dot(normal, ret) < 0.0f;
            return flip ? -ret : ret;
        }

        public static Vector3 RandomCosineDirection()
        {
            float r1 = RandomF();
            float r2 = RandomF();
            float z = SqrtF(1.0f - r2);
            float phi = TwoPi * r1;
            float sqrtR2 = SqrtF(r2);
            float x = CosF(phi) * sqrtR2;
            float y = SinF(phi) * sqrtR2;
            return new Vector3(x, y, z);
        }

        public static Vector3 RandomColor()
        {
            return new Vector3(RandomF(), RandomF(), RandomF());
        }
        public static Vector3 RandomColor(float min, float max)
        {
            return new Vector3(RandomF(min, max), RandomF(min, max), RandomF(min, max));
        }

        public static Vector2 RandomUnitVectorInDisk()
        {
            float radius = RandomF();
            float angle = RandomF(0.0f, TwoPi);
            return new Vector2(radius * SinF(angle), radius * CosF(angle));
        }

        public static float Clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(min, value), max);
        }

        public static Vector3 Refract(in Vector3 uv, in Vector3 n, float refractionPower)
        {
            float cosTheta = Vector3.Dot(-uv, n);
            Vector3 outParallel = refractionPower * (uv + cosTheta * n);
            Vector3 outPerpendicular = -SqrtF(1.0f - Vector3.Dot(outParallel, outParallel)) * n;
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

