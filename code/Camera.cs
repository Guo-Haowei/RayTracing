using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public class Camera
    {
        public Camera(
            in Vector3 lookFrom,
            in Vector3 lookAt,
            in Vector3 up,
            float fov,
            float aspectRatio,
            float aperture,
            float focusDistance)
        {
            float theta = Utility.DegreeToRadians(fov);
            float h = Utility.TanF(0.5f * theta);
            float viewportHeight = 2.0f * h;
            float viewportWidth = aspectRatio * viewportHeight;

            w = Vector3.Normalize(lookFrom - lookAt);
            u = Vector3.Normalize(Vector3.Cross(up, w));
            v = Vector3.Cross(w, u);

            origin = lookFrom;
            horizontal = focusDistance * viewportWidth * u;
            vertical = focusDistance * viewportHeight * v;
            lowerLeft = origin - 0.5f * horizontal - 0.5f * vertical - focusDistance * w;
            lensRadius = 0.5f * aperture;
        }

        public Ray getRay(float s, float t)
        {
            Vector2 rd = lensRadius * Utility.RandomUnitVectorInDisk();
            Vector3 offset = u * rd.X + v * rd.Y;
            Ray ray = new Ray();
            ray.origin = origin + offset;
            ray.direction = lowerLeft + s * horizontal + t * vertical - ray.origin;
            return ray;
        }

        private readonly Vector3 origin;
        private readonly Vector3 lowerLeft;
        private readonly Vector3 horizontal;
        private readonly Vector3 vertical;
        private readonly Vector3 u;
        private readonly Vector3 v;
        private readonly Vector3 w;
        private readonly float lensRadius;
    }

}
