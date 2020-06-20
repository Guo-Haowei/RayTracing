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

        public override bool scatter(in Ray ray, in HitRecord hrec, ref ScatterRecord srec)
        {
            Vector3 reflected = Vector3.Reflect(Vector3.Normalize(ray.direction), hrec.normal);
            srec.isSpecular = true;
            srec.specularRay = new Ray(hrec.point, reflected + fuzziness * Utility.RandomUnitVector());
            srec.attenuation = albedo;
            return true;
        }

        private readonly Vector3 albedo;
        private readonly float fuzziness;
    }

}
