using System;
using System.Numerics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RayTracingInOneWeekend
{
    class Program
    {
        static Vector3 rayColor(in Ray ray, in HittableList world)
        {
            HitRecord hitRecord = new HitRecord();
            if (world.hit(ray, 0.0f, float.PositiveInfinity, ref hitRecord))
            {
                return 0.5f * (hitRecord.normal + Vector3.One);
            }

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

            HittableList world = new HittableList();
            world.add(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f));
            world.add(new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f));

            int index = 0;
            for (int j = imageHeight - 1; j >= 0; --j)
            {
                for (int i = 0; i < imageWidth; ++i)
                {
                    float u = (float)i / (imageWidth - 1);
                    float v = (float)j / (imageHeight - 1);
                    Ray ray = new Ray(origin, lowerLeft + u * horizontal + v * vertical - origin);

                    Vector3 color = rayColor(ray, world);

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
