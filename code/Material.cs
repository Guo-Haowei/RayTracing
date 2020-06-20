using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public abstract class Material
    {
        public abstract bool scatter(in Ray ray, in HitRecord record, ref Vector3 albedo, ref Ray scattered, ref float pdf);
        public virtual float scatterPdf(in Ray ray, in HitRecord record, in Ray scattered) { return 0.0f; }
        public abstract Vector3 emit(in Vector2 uv, in Vector3 point);
    }

}
