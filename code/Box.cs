using System.Numerics;

namespace RayTracingInOneWeekend {

    public class Box : Hittable
    {
        public Box(in Vector3 min, in Vector3 max, in Material mat)
        {
            this.min = min;
            this.max = max;
            sides = new HittableList();
            sides.add(new XYRect(min, max, min.Z, mat));
            sides.add(new XYRect(min, max, max.Z, mat));
            sides.add(new XZRect(min, max, min.Y, mat));
            sides.add(new XZRect(min, max, max.Y, mat));
            sides.add(new YZRect(min, max, min.X, mat));
            sides.add(new YZRect(min, max, max.X, mat));
        }

        public override bool hit(in Ray ray, float tMin, float tMax, ref HitRecord record)
        {
            return sides.hit(ray, tMin, tMax, ref record);
        }

        private readonly Vector3 min;
        private readonly Vector3 max;
        private readonly Material material;
        private readonly HittableList sides;
    }

}