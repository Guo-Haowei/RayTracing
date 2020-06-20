using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public abstract class AxisAlignedRect : Hittable
    {
        protected AxisAlignedRect(in Vector3 min, in Vector3 max, float k, in Material material)
        {
            this.min = min;
            this.max = max;
            this.k = k;
            this.material = material;
        }

        public abstract float area();
        public sealed override float pdfValue(in Vector3 origin, in Vector3 v)
        {
            HitRecord record = new HitRecord();
            if (!hit(new Ray(origin, v), Ray.ZMin, Ray.ZMax, ref record))
                return 0.0f;
            
            float A = area();
            float vLength = Vector3.Dot(v, v);
            float distSqr = record.t * record.t * vLength;
            float cosine = Math.Abs(Vector3.Dot(v, record.normal) / Utility.SqrtF(vLength));
            return distSqr / (cosine * A);
        }

        protected readonly Vector3 min;
        protected readonly Vector3 max;
        protected readonly float k;
        protected readonly Material material;
    }

    public class XYRect : AxisAlignedRect
    {
        public XYRect(in Vector3 min, in Vector3 max, float k, in Material material)
            : base(min, max, k, material) { }

        public override bool hit(in Ray ray, float tMin, float tMax, ref HitRecord record)
        {
            float t = (k - ray.origin.Z) / ray.direction.Z;
            if (t <= tMin || t >= tMax)
                return false;

            Vector3 p = ray.at(t);
            if (p.X < min.X || p.X > max.X || p.Y < min.Y || p.Y > max.Y)
                return false;
            
            // uv
            record.t = t;
            record.point = p;
            record.setFaceNormal(ray, Vector3.UnitZ);
            record.material = material;
            
            return true;
        }

        public sealed override float area()
        {
            return (max.X - min.X) * (max.Y - min.Y);
        }

        public override Vector3 random(in Vector3 origin)
        {
            var randomPoint = new Vector3(
                Utility.RandomF(min.X, max.X),
                Utility.RandomF(min.Y, max.Y),
                k
            );

            return randomPoint - origin;
        }
    }

    public class XZRect : AxisAlignedRect
    {
        public XZRect(in Vector3 min, in Vector3 max, float k, in Material material)
            : base(min, max, k, material) { }

        public override bool hit(in Ray ray, float tMin, float tMax, ref HitRecord record)
        {
            float t = (k - ray.origin.Y) / ray.direction.Y;
            if (t <= tMin || t >= tMax)
                return false;

            Vector3 p = ray.at(t);
            if (p.X < min.X || p.X > max.X || p.Z < min.Z || p.Z > max.Z)
                return false;
            
            // uv
            record.t = t;
            record.point = p;
            record.setFaceNormal(ray, Vector3.UnitY);
            record.material = material;
            
            return true;
        }

        public sealed override float area()
        {
            return (max.X - min.X) * (max.Z - min.Z);
        }

        public override Vector3 random(in Vector3 origin)
        {
            var randomPoint = new Vector3(
                Utility.RandomF(min.X, max.X),
                k,
                Utility.RandomF(min.Z, max.Z)
            );

            return randomPoint - origin;
        }
    }

    public class YZRect : AxisAlignedRect
    {
        public YZRect(in Vector3 min, in Vector3 max, float k, in Material material)
            : base(min, max, k, material) { }

        public override bool hit(in Ray ray, float tMin, float tMax, ref HitRecord record)
        {
            float t = (k - ray.origin.X) / ray.direction.X;
            if (t <= tMin || t >= tMax)
                return false;

            Vector3 p = ray.at(t);
            if (p.Y < min.Y || p.Y > max.Y || p.Z < min.Z || p.Z > max.Z )
                return false;
            
            // uv
            record.t = t;
            record.point = p;
            record.setFaceNormal(ray, Vector3.UnitX);
            record.material = material;
            
            return true;
        }
    
        public sealed override float area()
        {
            return (max.Y - min.Y) * (max.Z - min.Z);
        }

        public override Vector3 random(in Vector3 origin)
        {
            var randomPoint = new Vector3(
                k,
                Utility.RandomF(min.Y, max.Y),
                Utility.RandomF(min.Z, max.Z)
            );

            return randomPoint - origin;
        }
    }

}
