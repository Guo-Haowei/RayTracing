using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public class Lambertian : Material
    {
        public Lambertian(in Texture albedo)
        {
            this.albedo = albedo;
        }

        public override bool scatter(in Ray ray, in HitRecord record, ref Vector3 attenuation, ref Ray scattered)
        {
            scattered.origin = record.point;
            scattered.direction = record.normal + Utility.RandomUnitVector();
            attenuation = albedo.value(record.uv, record.point);
            return true;
        }

        public override Vector3 emit(in Vector2 uv, in Vector3 point)
        {
            return Vector3.Zero;
        }

        private readonly Texture albedo;
    }

}
