using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public struct HitRecord
    {
        public Vector3 point;        
        public Vector3 normal;
        public Material material;
        public Vector2 uv;
        public float t;
        public bool frontFace;

        public void setFaceNormal(in Ray ray, in Vector3 outwardNormal)
        {
            int sign = -Math.Sign(Vector3.Dot(ray.direction, outwardNormal));
            frontFace = sign > 0; 
            normal = sign * outwardNormal;
        }
    }

    public abstract class Hittable
    {
        public abstract bool hit(in Ray ray, float tMin, float tMax, ref HitRecord record);
    }

}
