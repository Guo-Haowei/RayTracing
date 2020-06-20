using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public abstract class Pdf
    {
        public abstract float value(in Vector3 direction);
        public abstract Vector3 generate();
    }

    public class CosinePdf : Pdf
    {
        public CosinePdf(in Vector3 w)
        {
            uvw = OrthonormalBasis.CreateFromW(w);
        }

        public override float value(in Vector3 direction)
        {
            float cosine = Vector3.Dot(Vector3.Normalize(direction), uvw.w);
            return Math.Max(cosine / Utility.Pi, 0.0f);
        }

        public override Vector3 generate()
        {
            return uvw.local(Utility.RandomCosineDirection());
        }

        private readonly OrthonormalBasis uvw;
    }
    
    public class HittablePdf : Pdf
    {
        public HittablePdf(in Hittable hittable, in Vector3 origin)
        {
            this.hittable = hittable;
            this.origin = origin;
        }

        public override float value(in Vector3 direction)
        {
            return hittable.pdfValue(origin, direction);
        }

        public override Vector3 generate()
        {
            return hittable.random(origin);
        }

        private readonly Vector3 origin;
        private readonly Hittable hittable;
    }

    public class MixturePdf : Pdf
    {
        public MixturePdf(in Pdf pdf0, in Pdf pdf1)
        {
            this.pdf0 = pdf0;
            this.pdf1 = pdf1;
        }

        public override float value(in Vector3 direction)
        {
            return 0.5f * pdf0.value(direction) + 0.5f * pdf1.value(direction);
        }

        public override Vector3 generate()
        {
            return Utility.RandomF() < 0.5f ? pdf0.generate() : pdf1.generate();
        }

        private readonly Pdf pdf0;
        private readonly Pdf pdf1;
    }
}