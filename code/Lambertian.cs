using System;
using System.Numerics;
using System.Diagnostics;

namespace RayTracingInOneWeekend {

    public class Lambertian : Material
    {
        public Lambertian(in Texture albedo)
        {
            this.albedo = albedo;
        }

        public override bool scatter(in Ray ray, in HitRecord record, ref Vector3 albedo, ref Ray scattered, ref float pdf)
        {
            Vector3 direction = Utility.RandomUnitVectorInHemisphere(record.normal);
            scattered.origin = record.point;
            scattered.direction = direction;
            scattered.time = ray.time;
            albedo = this.albedo.value(record.uv, record.point);
            pdf = 0.5f / Utility.Pi;
            return true;
        }

        public override float scatterPdf(in Ray ray, in HitRecord record, in Ray scattered)
        {
            float cosine = Vector3.Dot(record.normal, Vector3.Normalize(scattered.direction));
            return Math.Max(cosine / Utility.Pi, 0.0f);
        }

        public override Vector3 emit(in Vector2 uv, in Vector3 point)
        {
            return Vector3.Zero;
        }

        private readonly Texture albedo;
    }

}
