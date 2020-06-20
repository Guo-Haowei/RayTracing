using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public class DiffuseLight : Material
    {
        public DiffuseLight(in Texture emission)
        {
            this.emission = emission;
        }

        public override Vector3 emit(in Ray ray, in HitRecord record)
        {
            // return record.frontFace ? emission.value(record.uv, record.point) : Vector3.Zero;
            return !record.frontFace ? emission.value(record.uv, record.point) : Vector3.Zero;
        }

        private readonly Texture emission;
    }

}