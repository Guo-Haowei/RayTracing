using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public class Sphere : Hittable
    {
        public Sphere(in Vector3 center, float radius, Material material)
        {
            this.center = center;
            this.radius = radius;
            this.material = material;
        }
        public override bool hit(in Ray ray, float tMin, float tMax, ref HitRecord record)
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

        public Vector3 center;
        public float radius;
        readonly protected Material material;
    }

}
