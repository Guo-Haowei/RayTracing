using System;
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
        readonly protected Material material;
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
