using System.Numerics;

namespace RayTracingInOneWeekend {

    public class Sphere : Hittable
    {
        public static bool RayHitSphere(in Ray ray, float tMin, float tMax, ref HitRecord record, in Vector3 center, float radius, in Material material)
        {
            Vector3 oc = ray.origin - center;
            float a = Vector3.Dot(ray.direction, ray.direction);
            float halfB = Vector3.Dot(oc, ray.direction);
            float c = Vector3.Dot(oc, oc) - radius * radius;
            float discriminant = halfB * halfB - a * c;

            if (discriminant > 0.0f)
            {
                float discriminantSqrt = Utility.SqrtF(discriminant);
                float root = (-halfB - discriminantSqrt) / a;
                if (root <= tMin || root >= tMax)
                {
                    root = (-halfB + discriminantSqrt) / a;
                    if (root <= tMin || root >= tMax)
                        return false;
                }
                
                record.t = root;
                record.point = ray.at(root);
                Vector3 outwardNormal = (record.point - center) / radius;
                record.setFaceNormal(ray, outwardNormal);
                record.material = material;
                return true;
            }

            return false;
        }

        public static Vector2 getUv(in Vector3 point)
        {
            float phi = Utility.Atan2F(point.Z, point.X);
            float theta = Utility.AsinF(point.Y);
            float u = 1.0f - (phi + Utility.Pi) / Utility.TwoPi;
            float v = (theta + Utility.HalfPi) / Utility.Pi;
            return new Vector2(u, v);
        }
        public override float pdfValue(in Vector3 origin, in Vector3 v)
        {
            HitRecord hrec = new HitRecord();
            if (!hit(new Ray(origin, v), Ray.ZMin, Ray.ZMax, ref hrec))
                return 0.0f;
            
            Vector3 d = center - origin;
            float distSqr = Vector3.Dot(d, d);
            float cosTheta = Utility.SqrtF(1.0f - radius * radius / distSqr);
            float solidAngle = Utility.TwoPi * (1.0f - cosTheta);
            return 1.0f / solidAngle;
        }

        public override Vector3 random(in Vector3 origin)
        {
            Vector3 d = center - origin;
            float distSqr = Vector3.Dot(d, d);
            var uvw = OrthonormalBasis.CreateFromW(d);
            return uvw.local(Utility.RandomToSphere(radius, distSqr));
        }

        public Sphere(in Vector3 center, float radius, Material material)
        {
            this.center = center;
            this.radius = radius;
            this.material = material;
        }

        public override bool hit(in Ray ray, float tMin, float tMax, ref HitRecord record)
        {
            return RayHitSphere(ray, tMin, tMax, ref record, center, radius, material);
        }

        public Vector3 center;
        public float radius;
        protected readonly Material material;
    }

    public class MovingSphere : Hittable
    {
        public MovingSphere(in Vector3 center0,
                            in Vector3 center1,
                            float time0,
                            float time1,
                            float radius,
                            Material material)
        {
            this.center0 = center0;
            this.center1 = center1;
            this.time0 = time0;
            this.time1 = time1;
            this.radius = radius;
            this.material = material;
        }
        public override bool hit(in Ray ray, float tMin, float tMax, ref HitRecord record)
        {
            Vector3 center = centerAt(ray.time);
            return Sphere.RayHitSphere(ray, tMin, tMax, ref record, center, radius, material);
        }

        public Vector3 centerAt(float time)
        {
            float percent = (time - time0) / (time1 - time0);
            return Vector3.Lerp(center0, center1, percent);
        }

        readonly public Vector3 center0, center1;
        readonly public float time0, time1;
        readonly public float radius;
        readonly public Material material;
    }

}
