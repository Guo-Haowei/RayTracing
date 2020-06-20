using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public class Metal : Material
    {
        public Metal(in Vector3 albedo, float fuzziness = 0.0f)
        {
            this.albedo = albedo;
            this.fuzziness = fuzziness;
        }

        public override bool scatter(in Ray ray, in HitRecord record, ref Vector3 albedo, ref Ray scattered, ref float pdf)
        {
            Vector3 reflected = Vector3.Reflect(Vector3.Normalize(ray.direction), record.normal);
            scattered.origin = record.point;
            scattered.direction = reflected + fuzziness * Utility.RandomUnitVector();
            scattered.time = ray.time;
            albedo = this.albedo;
            return Vector3.Dot(scattered.direction, record.normal) > 0.0f;
        }

        private readonly Vector3 albedo;
        private readonly float fuzziness;
    }

}
