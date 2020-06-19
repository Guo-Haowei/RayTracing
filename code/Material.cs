using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public abstract class Material
    {
        public abstract bool scatter(in Ray ray, in HitRecord record, ref Vector3 attenuation, ref Ray scattered);

        public abstract Vector3 emit(in Vector2 uv, in Vector3 point);
    }

}
