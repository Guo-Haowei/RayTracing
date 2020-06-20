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
            frontFace = -(Vector3.Dot(ray.direction, outwardNormal)) > 0;
            normal = frontFace ? outwardNormal : -outwardNormal;
        }
    }

    public abstract class Hittable
    {
        public abstract bool hit(in Ray ray, float tMin, float tMax, ref HitRecord record);

        public virtual float pdfValue(in Vector3 origin, in Vector3 v)
        {
            return 0.0f;
        }

        public virtual Vector3 random(in Vector3 origin)
        {
            return Vector3.UnitX;
        }
    }

}
