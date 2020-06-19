using System.Numerics;

namespace RayTracingInOneWeekend {

    public struct Ray
    {
        public Ray(in Vector3 origin, in Vector3 direction, float time = 0.0f)
        {
            this.origin = origin;
            this.direction = direction;
            this.time = time;
        }

        public Vector3 at(float t)
        {
            return origin + t * direction;
        }

        public Vector3 origin;
        public Vector3 direction;
        public float time;
    }

}
