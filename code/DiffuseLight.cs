using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public class DiffuseLight : Material
    {
        public DiffuseLight(in Texture emission)
        {
            this.emission = emission;
        }

        public override bool scatter(in Ray ray, in HitRecord record, ref Vector3 albedo, ref Ray scattered, ref float pdf)
        {
            return false;
        }

        public override Vector3 emit(in Vector2 uv, in Vector3 point)
        {
            return emission.value(uv, point);
        }

        private readonly Texture emission;
    }

}