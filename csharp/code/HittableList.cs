using System.Collections.Generic;

namespace RayTracingInOneWeekend {

    public class HittableList : Hittable
    {
        public HittableList()
        {
            hittables = new List<Hittable>();
        }

        public void add(Hittable hittable)
        {
            hittables.Add(hittable);
        }

        public void clear()
        {
            hittables.Clear();
        }

        public override bool hit(in Ray ray, float tMin, float tMax, ref HitRecord record)
        {
            HitRecord tempRecord = new HitRecord();
            bool hit = false;
            float closestSoFar = tMax;
            foreach (Hittable hittable in hittables)
            {
                if (hittable.hit(ray, tMin, closestSoFar, ref tempRecord))
                {
                    hit = true;
                    closestSoFar = tempRecord.t;
                    record = tempRecord;
                }
            }

            return hit;
        }

        private List<Hittable> hittables;
    }

}
