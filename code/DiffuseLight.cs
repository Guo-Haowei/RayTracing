using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public class DiffuseLight : Material
    {
        public DiffuseLight(in Texture emission)
        {
            this.emission = emission;
        }

        public override bool scatter(in Ray ray, in HitRecord record, ref Vector3 attenuation, ref Ray scattered)
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