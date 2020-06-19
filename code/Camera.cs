using System.Numerics;

namespace RayTracingInOneWeekend {

    public class Camera
    {
        public Camera(float aspectRatio)
        {
            float viewportHeight = 2.0f;
            float viewportWidth = aspectRatio * viewportHeight;
            float focalLength = 1.0f;

            origin = Vector3.Zero;
            horizontal = new Vector3(viewportWidth, 0.0f, 0.0f);
            vertical = new Vector3(0.0f, viewportHeight, 0.0f);
            lowerLeft = origin - 0.5f * horizontal - 0.5f * vertical - new Vector3(0.0f, 0.0f, focalLength);
        }

        public Ray getRay(float u, float v)
        {
            return new Ray(origin, lowerLeft + u * horizontal + v * vertical - origin);
        }

        private Vector3 origin;
        private Vector3 lowerLeft;
        private Vector3 horizontal;
        private Vector3 vertical;
    }

}
