using System;
using System.Numerics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RayTracingInOneWeekend
{
    class Program
    {
        static bool hitSphere(in Vector3 center, float radius, in Ray ray)
        {
            Vector3 oc = ray.origin - center;
            float a = Vector3.Dot(ray.direction, ray.direction);
            float halfB = Vector3.Dot(oc, ray.direction);
            float c = Vector3.Dot(oc, oc) - radius * radius;
            float d = halfB * halfB - a * c;

            return d > 0.0f;
        }

        static Vector3 rayColor(in Ray ray)
        {
            if (hitSphere(-Vector3.UnitZ, 0.5f, ray))
                return Vector3.UnitX;

            Vector3 unitDirection = Vector3.Normalize(ray.direction);

            float t = 0.5f * unitDirection.Y + 0.5f;
            Vector3 white = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 blue = new Vector3(0.5f, 0.7f, 1.0f);

            return (1.0f - t) * white + t * blue;
        }

        static void Main()
        {
            const float aspectRatio = 16.0f / 9.0f;
            const int imageWidth = 384;
            const int imageHeight = (int)(imageWidth / aspectRatio);

            const int component = 3;
            const int stride = component * imageWidth;

            byte[] imageBuffer = new byte[stride * imageHeight];

            const float viewportHeight = 2.0f;
            const float viewportWidth = aspectRatio * viewportHeight;
            const float focalLength = 1.0f;

            Vector3 origin = Vector3.Zero;
            Vector3 horizontal = new Vector3(viewportWidth, 0.0f, 0.0f);
            Vector3 vertical = new Vector3(0.0f, viewportHeight, 0.0f);
            Vector3 lowerLeft = origin - 0.5f * horizontal - 0.5f * vertical - new Vector3(0.0f, 0.0f, focalLength);

            int index = 0;
            for (int j = imageHeight - 1; j >= 0; --j)
            {
                for (int i = 0; i < imageWidth; ++i)
                {
                    float u = (float)i / (imageWidth - 1);
                    float v = (float)j / (imageHeight - 1);
                    Ray ray = new Ray(origin, lowerLeft + u * horizontal + v * vertical - origin);

                    Vector3 color = rayColor(ray);

                    byte br = (byte)(255.999f * color.X);
                    byte bg = (byte)(255.999f * color.Y);
                    byte bb = (byte)(255.999f * color.Z);

                    imageBuffer[index++] = bb;
                    imageBuffer[index++] = bg;
                    imageBuffer[index++] = br;
                }
            }

            Bitmap bitmap = new Bitmap(imageWidth, imageHeight, stride, PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(imageBuffer, 0));
            bitmap.Save("../image.png", ImageFormat.Png);
            bitmap.Dispose();
        }
    }
}
