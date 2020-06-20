using System.Numerics;

namespace RayTracingInOneWeekend {

    public struct ScatterRecord
    {
        public Ray specularRay;
        public bool isSpecular;
        public Vector3 attenuation;
        public Pdf pdf;
    }

    public class Material
    {
        public virtual bool scatter(in Ray ray, in HitRecord hrec, ref ScatterRecord srec)
        {
            return false;
        }

        public virtual float scatterPdf(in Ray ray, in HitRecord record, in Ray scattered)
        {
            return 0.0f;
        }

        public virtual Vector3 emit(in Ray ray, in HitRecord record)
        {
            return Vector3.Zero;
        }
    }

}
