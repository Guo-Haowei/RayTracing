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

        public override bool scatter(in Ray ray, in HitRecord hrec, ref ScatterRecord srec)
        {
            srec.isSpecular = false;
            srec.attenuation = albedo.value(hrec.uv, hrec.point);
            srec.pdf = new CosinePdf(hrec.normal);
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
