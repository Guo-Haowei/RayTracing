using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public class Dielectric : Material
    {
        public Dielectric(float reflectionPower)
        {
            this.reflectionPower = reflectionPower;
        }

        public override bool scatter(in Ray ray, in HitRecord record, ref Vector3 albedo, ref Ray scattered)
        {
            albedo = Vector3.One;
            float factor = record.frontFace ? 1.0f / reflectionPower : reflectionPower;
            Vector3 unitDirection =  Vector3.Normalize(ray.direction);

            scattered.origin = record.point;

            float cosTheta = Math.Min(-Vector3.Dot(unitDirection, record.normal), 1.0f);
            float sinTheta = Utility.SqrtF(1.0f - cosTheta * cosTheta);
            float reflectionProbability = Utility.Schlick(cosTheta, factor);

            if (factor * sinTheta > 1.0f || Utility.RandomF() < reflectionProbability)
                scattered.direction = Vector3.Reflect(unitDirection, record.normal);
            else
                scattered.direction = Utility.Refract(unitDirection, record.normal, factor);

            return true;
        }

        public override Vector3 emit(in Vector2 uv, in Vector3 point)
        {
            return Vector3.Zero;
        }

        private readonly float reflectionPower;
    }

}
