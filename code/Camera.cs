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
            float aspectRatio)
        {
            float theta = Utility.DegreeToRadians(fov);
            float h = (float)Math.Tan(0.5f * theta);
            float viewportHeight = 2.0f * h;
            float viewportWidth = aspectRatio * viewportHeight;

            Vector3 w = Vector3.Normalize(lookFrom - lookAt);
            Vector3 u = Vector3.Normalize(Vector3.Cross(up, w));
            Vector3 v = Vector3.Cross(w, u);

            origin = lookFrom;
            horizontal = viewportWidth * u;
            vertical = viewportHeight * v;
            lowerLeft = origin - 0.5f * horizontal - 0.5f * vertical - w;
        }

        public Ray getRay(float u, float v)
        {
            return new Ray(origin, lowerLeft + u * horizontal + v * vertical - origin);
        }

        private readonly Vector3 origin;
        private readonly Vector3 lowerLeft;
        private readonly Vector3 horizontal;
        private readonly Vector3 vertical;
    }

}
