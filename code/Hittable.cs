using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public struct HitRecord
    {
        public Vector3 point;        
        public Vector3 normal;
        public float t;

        public void setFaceNormal(in Ray ray, in Vector3 outwardNormal)
        {
            float flipNormal = -Math.Sign(Vector3.Dot(ray.direction, outwardNormal));
            normal = flipNormal * outwardNormal;
        }
    }

    abstract public class Hittable
    {
        public abstract bool hit(in Ray ray, float tMin, float tMax, ref HitRecord record);
    }

}
