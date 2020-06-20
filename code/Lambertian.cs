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
            OrthonormalBasis uvw = OrthonormalBasis.CreateFromW(record.normal);
            Vector3 direction = uvw.local(Utility.RandomCosineDirection());
            scattered.origin = record.point;
            scattered.direction = direction;
            scattered.time = ray.time;
            albedo = this.albedo.value(record.uv, record.point);
            pdf = Vector3.Dot(uvw.w, direction) / Utility.Pi;
            return true;
        }

        public override float scatterPdf(in Ray ray, in HitRecord record, in Ray scattered)
        {
            float cosine = Vector3.Dot(record.normal, Vector3.Normalize(scattered.direction));
            return Math.Max(cosine / Utility.Pi, 0.0f);
        }

        private readonly Texture albedo;
    }

}
